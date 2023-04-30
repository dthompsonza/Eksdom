using Microsoft.Extensions.Caching.Memory;

namespace Eksdom.Client.Models.Caching;

/// <summary>
/// In-memory caching, implemented by default in <see cref="EspClient"/>
/// </summary>
public class MemoryResponseCache : IResponseCache
{
    private readonly MemoryCache _memoryCache;
    private readonly TimeSpan _cacheDuration;

    public string PartitionKey { get; init; }

    public MemoryResponseCache(TimeSpan? cacheDuration, string? partitionKey = null)
    {
        _memoryCache = new MemoryCache(new MemoryCacheOptions());
        _cacheDuration = cacheDuration ?? TimeSpan.FromHours(Constants.DefaultCacheHours);
        PartitionKey = partitionKey ?? "NONE";
    }

    public TimeSpan CacheDuration => _cacheDuration;

    public virtual void Add<TResponse>(string key, TResponse item, TimeSpan duration = default) 
        where TResponse : ResponseModel
    {
        _memoryCache.Set(key, item, CalculateCacheDuration(duration));
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
            return null;
        }

        return (TResponse)objValue;
    }

    public virtual void Remove<TResponse>(string key)
        where TResponse : ResponseModel
    {
        _memoryCache.Remove(key);
    }

    public virtual void Clear()
    {
        _memoryCache.Clear();
    }

    public void Dispose()
    {
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
