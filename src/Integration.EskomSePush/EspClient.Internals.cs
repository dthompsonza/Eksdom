using System.Collections.Specialized;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using Eksdom.Client.Caching;
using Eksdom.Client.Models;

namespace Eksdom.Client;

public sealed partial class EspClient : IDisposable
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private static EspClient _instance;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    private readonly HttpClient _httpClient;
    private readonly ApiTestModes _testMode;
    private readonly TimeSpan _cacheDuration;
    private readonly Dictionary<bool, IResponseCache?> _responseCaches = new Dictionary<bool, IResponseCache?>
    {
        { ShortTermCacheKey, null },
        { LongTermCacheKey, null }
    };
    private string? _licenceKey;

    private EspClient(EspClientOptions clientOptions)
    {
        _testMode = clientOptions.TestMode;
        _httpClient = clientOptions.httpClient ?? new HttpClient(new SocketsHttpHandler
        {
            PooledConnectionLifetime = TimeSpan.FromMinutes(15),
        })
        {
            BaseAddress = new Uri(Constants.Api20Uri),
        };

        CheckAndSetLicenceKeyWithHeaders(clientOptions.LicenceKey);

        _cacheDuration = clientOptions.CacheDuration;
        _responseCaches[ShortTermCacheKey] = new MemoryResponseCache(_cacheDuration);
        _responseCaches[LongTermCacheKey] ??= clientOptions.ResponseCache ?? new MemoryResponseCache(_cacheDuration);
    }

    /// <summary>
    /// Sync HTTP GET
    /// </summary>
    private ResponseModel? GetResponse<TResponse>(string path, string? id = null, bool cache = true, int apiCost = 1)
        where TResponse : ResponseModel
    {
        var task = InternalGetAsync<TResponse>(BuildPath(path, id!), cache, apiCost);

        try
        {
            task.Wait();
        }
        catch (AggregateException aex)
        {
            if (aex.InnerExceptions.Count == 1)
            {
                throw aex.InnerExceptions[0];
            }
            throw;
        }

        if (task.IsCompleted &&
            task.Result.success &&
            task.Result.model is not null)
        {
            return task.Result.model!;
        }

        return null;
    }

    /// <summary>
    /// Async HTTP GET
    /// </summary>
    private async Task<ResponseModel?> GetResponseAsync<TResponse>(string path, string? id = null, bool cache = true, int apiCost = 1)
        where TResponse : ResponseModel
    {
        var response = await InternalGetAsync<TResponse>(BuildPath(path, id!), cache, apiCost).ConfigureAwait(false);

        if (response.success)
        {
            return response.model;
        }

        return null;
    }

    /// <summary>
    /// Internal async GET method that handles caching and invalid Licence Keys
    /// </summary>
    /// <typeparam name="TResponse"></typeparam>
    /// <param name="requestUri"></param>
    /// <param name="cache"></param>
    /// <returns></returns>
    /// <exception cref="EksdomException"></exception>
    /// 

    private async Task<(bool success, TResponse? model)> InternalGetAsync<TResponse>(string requestUri, bool cache, int apiCost)
        where TResponse : ResponseModel
    {
        if (!HasLicenceKey())
        {
            throw new EksdomException($"No licence key", ExceptionTypes.InvalidApiKey);
        }

        if (_responseCaches[cache] is not null)
        {
            if (_responseCaches[cache]!.TryGet<TResponse>(BuildCacheName(requestUri), out var cachedModel))
            {
                if (cachedModel is not null)
                {
                    return (true, cachedModel);
                }
            }
        }

        HttpResponseMessage? response;

        try
        {
            response = await _httpClient.GetAsync(requestUri).ConfigureAwait(false);
            if (cache == LongTermCacheKey && apiCost > 0)
            {
                _responseCaches[ShortTermCacheKey]!.Clear();
            }
        }
        catch (Exception ex)
        {
            throw new EksdomException($"Internal http client threw an exception - {ex.Message}", ex);
        }

        if (!response.IsSuccessStatusCode)
        {
            switch (response.StatusCode)
            {
                case HttpStatusCode.Forbidden:
                    throw new EksdomException($"Server rejects licence key", ExceptionTypes.InvalidApiKey);
                default:
                    return (false, default);
            }
        }

        var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

        var options = new JsonSerializerOptions(JsonSerializerDefaults.Web)
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            IgnoreReadOnlyProperties = true,
            PropertyNameCaseInsensitive = false,
            NumberHandling = JsonNumberHandling.AllowReadingFromString,
        };

        var model = JsonSerializer.Deserialize<TResponse>(responseString, options);

        if (_responseCaches[cache] is not null && model is not null)
        {
            var cacheDuration = cache ? _cacheDuration : TimeSpan.FromMinutes(Constants.DefaultShortCacheMinutes);
            _responseCaches[cache]!.Add(BuildCacheName(requestUri), model, cacheDuration);
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

    private string BuildCacheName(string requestUri)
    {
        if (!HasLicenceKey())
        {
            throw new InvalidOperationException("Cannot build cache name if no licence key configured");
        }

        return requestUri.Hash(_licenceKey!);
    }

    private const bool ShortTermCacheKey = false;
    private const bool LongTermCacheKey = true;


    public void Dispose()
    {
        _httpClient?.Dispose();
        _responseCaches[false]?.Dispose();
        _responseCaches[true]?.Dispose();
    }
}
