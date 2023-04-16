using Microsoft.Extensions.Caching.Memory;

namespace Integration.EskomSePush.Models.Responses.Caching;

public class MemoryResponseCache : IResponseCache
    
{
    private readonly MemoryCache _memoryCache;

    public MemoryResponseCache()
    {
        _memoryCache = new MemoryCache(new MemoryCacheOptions());
    }

    public void Add<TResponse>(string key, TResponse value, TimeSpan duration) 
        where TResponse : ResponseModel
    {
        _memoryCache.Set(key, value, duration);
    }

    public bool TryGet<TResponse>(string key, out TResponse? value)
        where TResponse : ResponseModel
    {
        var objValue = _memoryCache.Get(key);

        if (objValue is null)
        {
            value = default;
            return false;
        }

        value = (TResponse)objValue;
        return true;
    }

    public void Remove<TResponse>(string key)
        where TResponse : ResponseModel
    {
        _memoryCache.Remove(key);
    }

    public void Dispose()
    {
        _memoryCache.Dispose();
    }
}
