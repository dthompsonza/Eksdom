using System.Collections.Specialized;
using System.Net;
using System.Text.Json;
using EnsureThat;
using Integration.EskomSePush.Models.Responses;
using Integration.EskomSePush.Models.Responses.Caching;

namespace Integration.EskomSePush;

public sealed partial class ApiClient : IDisposable
{
    private static ApiClient? _instance;

    private readonly HttpClient _httpClient;
    private readonly ApiTestModes _testMode;
    private readonly TimeSpan _cacheDuration;
    private readonly IResponseCache _responseCache;

    private const int DefaultCacheHours = 2;

    private ApiClient(string espLicenceKey,
        ApiTestModes testMode,
        TimeSpan? cacheDuration,
        HttpClient? httpClient,
        IResponseCache? responseCache)
    {
        Ensure.That(espLicenceKey).IsNotNullOrEmpty();

        _testMode = testMode;
        _httpClient = httpClient is null ? new HttpClient
        {
            BaseAddress = new Uri(Constants.Api20Uri),
        } : httpClient;

        _httpClient.DefaultRequestHeaders.Add(Constants.Headers.Token, espLicenceKey);
        _httpClient.DefaultRequestHeaders.Add(Constants.Headers.Client, Constants.ClientDescription);

        _cacheDuration = cacheDuration is null
            ? TimeSpan.FromHours(DefaultCacheHours)
            : cacheDuration!.Value.Seconds == 0 ? TimeSpan.FromHours(DefaultCacheHours) : cacheDuration!.Value;

        _responseCache = responseCache is null ? new MemoryResponseCache() : responseCache;
    }

    private ResponseModel? GetResponse<TResponse>(string path, string? id = null, bool cache = true)
        where TResponse : ResponseModel
    {
        if (cache)
        {
            if (_responseCache.TryGet<TResponse>(BuildCacheName(path, id), out var cachedModel))
            {
                if (cachedModel is not null)
                {
                    return cachedModel;
                }
            }
        }
        var task = InternalGetAsync<TResponse>(BuildPath(path, id!));

        Task.WaitAll(task);

        if (task.IsCompleted &&
            task.Result.success &&
            task.Result.model is not null)
        {
            if (cache)
            {
                _responseCache.Add(BuildCacheName(path, id), task.Result.model!, _cacheDuration);
            }
            return task.Result.model!;
        }

        return null;
    }

    private async Task<ResponseModel?> GetResponseAsync<TResponse>(string path, string? id = null, bool cache = true)
        where TResponse : ResponseModel
    {
        var response = await InternalGetAsync<TResponse>(BuildPath(path, id!));

        if (response.success)
        {
            return response.model;
        }

        return null;
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
            nmc.Add(Constants.QueryParams.Test, _testMode == ApiTestModes.Current ? Constants.Testing.Current : Constants.Testing.Future);
        }
        if (id is not null)
        {
            nmc.Add(Constants.QueryParams.Id, id);
        }

        return path.ToQueryString(nmc);
    }

    private string BuildCacheName(string path, string? id)
    {
        return $"{path}-{id ?? "NoID"}".Hash();
    }

    private async Task<(bool success, TModel? model)> InternalGetAsync<TModel>(string requestUri)
    {
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

        var model = JsonSerializer.Deserialize<TModel>(responseString, options);
        return (true, model);
    }

    public void Dispose()
    {
        _httpClient?.Dispose();
        _responseCache?.Dispose();
    }
}
