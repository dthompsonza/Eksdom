using System.Collections.Specialized;
using System.Net;
using System.Text.Json;
using Eksdom.EskomSePush.Client;
using EnsureThat;
using Integration.EskomSePush.Models.Responses;
using Integration.EskomSePush.Models.Responses.Caching;

namespace Integration.EskomSePush;

public sealed partial class ApiClient : IDisposable
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private static ApiClient _instance;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    private readonly HttpClient _httpClient;
    private readonly ApiTestModes _testMode;
    private readonly TimeSpan? _cacheDuration;
    private readonly IResponseCache? _responseCache;
    private readonly string _licenceKey;

    private ApiClient(ApiClientOptions clientOptions)
    {
        Ensure.That(clientOptions.LicenceKey).IsNotNullOrEmpty();
        _licenceKey = clientOptions.LicenceKey;

        _testMode = clientOptions.TestMode;
        _httpClient = clientOptions.httpClient ?? new HttpClient
        {
            BaseAddress = new Uri(Constants.Api20Uri),
        };

        _httpClient.DefaultRequestHeaders.Add(Constants.Headers.Token, _licenceKey);
        _httpClient.DefaultRequestHeaders.Add(Constants.Headers.Client, Constants.ClientDescription);

        if (clientOptions.ResponseCache is not null)
        {
            _responseCache = clientOptions.ResponseCache;
            _cacheDuration = clientOptions.CacheDuration;
        }
    }

    private ResponseModel? GetResponse<TResponse>(string path, string? id = null, bool cache = true)
        where TResponse : ResponseModel
    {
        var task = InternalGetAsync<TResponse>(BuildPath(path, id!), cache);

        task.Wait();

        if (task.IsCompleted &&
            task.Result.success &&
            task.Result.model is not null)
        {
            return task.Result.model!;
        }

        return null;
    }

    private async Task<ResponseModel?> GetResponseAsync<TResponse>(string path, string? id = null, bool cache = true)
        where TResponse : ResponseModel
    {
        var response = await InternalGetAsync<TResponse>(BuildPath(path, id!), cache);

        if (response.success)
        {
            return response.model;
        }

        return null;
    }

    private async Task<(bool success, TResponse? model)> InternalGetAsync<TResponse>(string requestUri, bool cache)
        where TResponse : ResponseModel
    {
        if (cache && _responseCache is not null)
        {
            if (_responseCache.TryGet<TResponse>(BuildCacheName(requestUri), out var cachedModel))
            {
                if (cachedModel is not null)
                {
                    return (true, cachedModel);
                }
            }
        }

        var response = await _httpClient.GetAsync(requestUri);

        if (!response.IsSuccessStatusCode)
        {
            switch (response.StatusCode)
            {
                case HttpStatusCode.Forbidden:
                    throw new EksdomException($"Invalid licence key");
                default:
                    return (false, default);
            }
        }

        var responseString = await response.Content.ReadAsStringAsync();

        var options = new JsonSerializerOptions(JsonSerializerDefaults.Web)
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            IgnoreReadOnlyProperties = true,
        };

        var model = JsonSerializer.Deserialize<TResponse>(responseString, options);

        if (_responseCache is not null && model is not null)
        {
            _responseCache.Add(BuildCacheName(requestUri), model, _cacheDuration ?? default);
        }

        return (true, model);
    }

    private string BuildPath(string path, string? id)
    {
        if (id is null && _testMode == ApiTestModes.None)
        {
            return path;
        }

        var nmc = new NameValueCollection();

        if (_testMode != ApiTestModes.None)
        {
            nmc.Add(Constants.QueryParams.Test, _testMode == ApiTestModes.Current 
                ? Constants.Testing.Current : Constants.Testing.Future);
        }
        if (id is not null)
        {
            nmc.Add(Constants.QueryParams.Id, id);
        }

        return path.ToQueryString(nmc);
    }

    private string BuildCacheName(string requestUri) => requestUri.Hash(_licenceKey);

    private bool CacheEnabled() => _responseCache is not null;

    public void Dispose()
    {
        _httpClient?.Dispose();
        _responseCache?.Dispose();
    }
}
