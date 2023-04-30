﻿using System.Text.Json;
using Eksdom.Client.Caching;

namespace Eksdom.Service.Caching;

/// <summary>
/// Hooks into the <see cref="MemoryResponseCache"/> and allows for the cache to be refreshed from disk
/// in between service restarts, to eliminate wasting API allowance.
/// </summary>
internal class FileResponseCache : MemoryResponseCache
{
    private readonly object _lockObject = new object();

    public FileResponseCache(string? partitionKey)
        : base(cacheDuration: null, partitionKey: partitionKey) // Let the MemoryResponseCache deal with the cache timeout (2 hrs)
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
        return item;
    }

    public override void Clear()
    {
        var directoryInfo = new DirectoryInfo(Path.GetTempPath());
        var jsonFiles = directoryInfo.GetFiles($"{PartitionKey}*.json", SearchOption.TopDirectoryOnly);

        lock(_lockObject)
        {
            try
            {
                foreach (FileInfo file in jsonFiles)
                {
                    file.Delete();
                }
            }
            catch
            {
                // oh well
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
            }
            catch
            {
                // oh well
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
            }
            catch
            {
                // ILogger this laterrr
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
