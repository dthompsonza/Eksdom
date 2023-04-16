namespace Integration.EskomSePush.Models.Responses.Caching;

public interface IResponseCache : IDisposable
{
    public void Add<TResponse>(string key, TResponse value, TimeSpan duration)
        where TResponse : ResponseModel;

    public bool TryGet<TResponse>(string key, out TResponse? value)
        where TResponse : ResponseModel;

    public void Remove<TResponse>(string key)
        where TResponse : ResponseModel;
}
