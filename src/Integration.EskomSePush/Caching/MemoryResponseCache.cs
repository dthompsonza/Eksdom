using Eksdom.Client.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Eksdom.Client.Caching;

/// <summary>
/// In-memory caching, implemented by default in <see cref="EspClient"/>
/// </summary>
public class MemoryResponseCache : IResponseCache
{
    private readonly MemoryCache _memoryCache;
    private readonly TimeSpan _cacheDuration;

    public string PartitionKey { get; init; }

    public ILogger<MemoryResponseCache> Logger { get; init; }

    public MemoryResponseCache(TimeSpan? cacheDuration, 
        string? partitionKey = null,
        ILogger<MemoryResponseCache>? logger = null)
    {
        _memoryCache = new MemoryCache(new MemoryCacheOptions());
        _cacheDuration = cacheDuration ?? TimeSpan.FromHours(Constants.DefaultCacheHours);
        PartitionKey = partitionKey ?? "NONE";
        Logger = logger ?? LoggerFactory.Create(builder =>
        {
            builder.AddConsole();
        }).CreateLogger<MemoryResponseCache>();
    }

    public TimeSpan CacheDuration => _cacheDuration;

    public virtual void Add<TResponse>(string key, TResponse item, TimeSpan duration = default)
        where TResponse : ResponseModel
    {
        var cacheDuration = CalculateCacheDuration(duration);
        Logger.LogTrace("Setting memory cache {Key} with {Item} for {CacheDuration}", key, item, cacheDuration);
        _memoryCache.Set(key, item, cacheDuration);
    }

    public bool TryGet<TResponse>(string key, out TResponse? item)
        where TResponse : ResponseModel
    {
        var objValue = Get<TResponse>(key);

        if (objValue is null)
        {
            item = default;
            return false;
        }

        item = objValue;
        return true;
    }

    public virtual TResponse? Get<TResponse>(string key)
        where TResponse : ResponseModel
    {
        var objValue = _memoryCache.Get(key);

        if (objValue is null)
        {
            Logger.LogTrace("Getting memory cache {Key} found no item in cache", key);
            return null;
        }

        var item = (TResponse)objValue;
        Logger.LogTrace("Getting memory cache {Key} found {Item} in cache", key, item);
        return item;
    }

    public virtual void Remove<TResponse>(string key)
        where TResponse : ResponseModel
    {
        Logger.LogTrace("Removing memory cache {Key} item", key);
        _memoryCache.Remove(key);
    }

    public virtual void Clear()
    {
        Logger.LogTrace("Clearing memory cache");
        _memoryCache.Clear();
    }

    public void Dispose()
    {
        Logger.LogTrace("Memory cache disposed");
        _memoryCache.Dispose();
    }

    private TimeSpan CalculateCacheDuration(TimeSpan duration)
    {
        if (duration == default && _cacheDuration == default)
        {
            return TimeSpan.FromHours(Constants.DefaultCacheHours);
        }

        if (duration.TotalSeconds > 0)
        {
            return duration;
        }

        if (_cacheDuration.TotalSeconds > 0)
        {
            return _cacheDuration;
        }

        return TimeSpan.FromHours(Constants.DefaultCacheHours);
    }
}
