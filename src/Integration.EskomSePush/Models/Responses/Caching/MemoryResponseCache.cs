using Microsoft.Extensions.Caching.Memory;

namespace Integration.EskomSePush.Models.Responses.Caching;

public class MemoryResponseCache : IResponseCache
{
    private readonly MemoryCache _memoryCache;
    private readonly TimeSpan _cacheDuration;

        public MemoryResponseCache(TimeSpan? cacheDuration)
    {
        _memoryCache = new MemoryCache(new MemoryCacheOptions());
        _cacheDuration = cacheDuration ?? TimeSpan.FromHours(Constants.DefaultCacheHours);
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
