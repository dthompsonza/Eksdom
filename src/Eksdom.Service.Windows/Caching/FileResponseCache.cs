using System.Text.Json;
using Eksdom.Client.Caching;
using Microsoft.Extensions.Logging;

namespace Eksdom.Service.Caching;

/// <summary>
/// Hooks into the <see cref="MemoryResponseCache"/> and allows for the cache to be refreshed from disk
/// in between service restarts, to eliminate wasting API allowance.
/// </summary>
internal sealed class FileResponseCache : MemoryResponseCache
{
    private readonly object _lockObject = new object();

    public FileResponseCache(string? partitionKey, ILogger<FileResponseCache>? logger = null)
        : base(cacheDuration: null, partitionKey, logger) 
    {
    }

    public override void Add<TResponse>(string key, TResponse item, TimeSpan duration = default)
    {
        base.Add(key, item, duration);
        CacheToFile(key, item);
    }

    public override TResponse Get<TResponse>(string key)
    {
        var item = base.Get<TResponse>(key);
        if (item is null)
        {
            item = ReadCacheFile<TResponse>(key, out var cacheAge);
            if (item is not null)
            {
                base.Add(key, item, base.CacheDuration - cacheAge);
            }
        }
        return item!;
    }

    public override void Clear()
    {
        var directoryInfo = new DirectoryInfo(Path.GetTempPath());
        var jsonFiles = directoryInfo.GetFiles($"{PartitionKey}*.json", SearchOption.TopDirectoryOnly);

        lock(_lockObject)
        {
            var cachingFilename = string.Empty;

            try
            {
                foreach (FileInfo file in jsonFiles)
                {
                    cachingFilename = file.FullName;
                    file.Delete();
                    Logger.LogTrace("Deleted caching file {Filename}", cachingFilename);
                }
            }
            catch
            {
                Logger.LogWarning("Failed to delete caching file {Filename}", cachingFilename);
            }
        }
        base.Clear();
    }

    private void CacheToFile<TResponse>(string key, TResponse? item)
    {
        if (item is null)
        {
            return;
        }

        var json = JsonSerializer.Serialize(item);
        var filename = BuildFilename<TResponse>(key, PartitionKey);

        lock (_lockObject)
        {
            try
            {
                File.WriteAllText(filename, json);
                Logger.LogTrace("Saved data to caching file {Filename}", filename);
            }
            catch (Exception ex)
            {
                Logger.LogWarning("Failed to write to caching file {Filename} - {Error}", filename, ex);
            }
        }
    }

    private TResponse? ReadCacheFile<TResponse>(string key, out TimeSpan cacheAge)
    {
        var filename = BuildFilename<TResponse>(key, PartitionKey);
        
        if (!File.Exists(filename))
        {
            cacheAge = default;
            return default;
        }
        cacheAge = DateTime.Now - LastWriteTime(filename);
        if (cacheAge >= base.CacheDuration)
        {
            return default;
        }

        TResponse? item = default;

        lock (_lockObject)
        {
            try
            {
                var json = File.ReadAllText(filename);
                item = JsonSerializer.Deserialize<TResponse>(json);
                Logger.LogTrace("Read data from caching file {Filename}, found {Item}", filename, item);
            }
            catch (Exception ex)
            {
                Logger.LogWarning("Failed to read caching file {Filename} - {Exception}", filename, ex);
            }
        }

        return item is null ? default : item;
    }

    private static string BuildFilename<TResponse>(string key, string partitionKey)
    {
        var filename = Path.Combine(Path.GetTempPath(), $"{AppDomain.CurrentDomain.FriendlyName}-{partitionKey}-{typeof(TResponse).Name}-{key}.json");
        return filename;
    }

    private static DateTime LastWriteTime(string filename)
    {
        var fileInfo = new FileInfo(filename);
        return fileInfo.LastWriteTime;
    }
}
