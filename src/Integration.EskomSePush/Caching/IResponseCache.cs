using Eksdom.Client.Models;

namespace Eksdom.Client.Caching;

public interface IResponseCache : IDisposable
{
    public string PartitionKey { get; init; }

    /// <summary>
    /// Add item to the cache
    /// </summary>
    public void Add<TResponse>(string key, TResponse value, TimeSpan duration = default)
        where TResponse : ResponseModel;

    /// <summary>
    /// Try and retrieve an item from cache. Null if not found.
    /// </summary>
    public bool TryGet<TResponse>(string key, out TResponse? value)
        where TResponse : ResponseModel;

    /// <summary>
    /// Removes an item from the cache
    /// </summary>
    public void Remove<TResponse>(string key)
        where TResponse : ResponseModel;

    /// <summary>
    /// Clears the whole cache
    /// </summary>
    public void Clear();
}
