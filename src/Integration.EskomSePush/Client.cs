using System.Collections.Specialized;
using System.Text.Json;
using EnsureThat;
using Integration.EskomSePush.Models.Responses;

namespace Integration.EskomSePush;

/// <summary>
/// Client for <see href="https://eskomsepush.gumroad.com/l/api">EskomSePush API</see> 2.0. 
/// Documentation on <see href="https://documenter.getpostman.com/view/1296288/UzQuNk3E">Postman Documenter</see>
/// </summary>
public class Client : IDisposable
{
    private HttpClient _httpClient;
    private readonly ApiTestModes _testMode;
    private static Client? _instance;

    private Client(string espLicenceKey, ApiTestModes testMode = default) 
    {
        Ensure.That(espLicenceKey).IsNotNullOrEmpty();

        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri("https://developer.sepush.co.za/business/2.0/");
        _httpClient.DefaultRequestHeaders.Add("Token", espLicenceKey);
        _testMode = testMode;
    }

    public static Client Create(string espLicenceKey, ApiTestModes testMode = default)
    {
        if (_instance is null)
        {
            _instance = new Client(espLicenceKey, testMode);
        }

        return _instance;
    }

    #region API Calls

    public Status? GetStatus()
    {
        return (Status?)GetResponse<Status>(Constants.Endpoints.Status);
    }

    public Allowance? GetAllowance()
    {
        return (Allowance?)GetResponse<Allowance>(Constants.Endpoints.Allowance);
    }

    public async Task<Allowance?> GetAllowanceAsync()
    {
        var response = await GetResponseAsync<Allowance>(Constants.Endpoints.Allowance);
        return (Allowance?)response;
    }

    public AreaInformation? GetArea(string id)
    {
        Ensure.That(id).IsNotNullOrEmpty();
        return (AreaInformation?)GetResponse<AreaInformation>(Constants.Endpoints.Area, id);
    }

    #endregion

    #region Internal

    private Response? GetResponse<TResponse>(string path, string? id = null) 
        where TResponse : Response
    {
        var task = InternalGetAsync<TResponse>(BuildPath(path, id!));

        Task.WaitAll(task);

        if (task.IsCompleted && task.Result.success)
        {
            return task.Result.model!;
        }

        return null;
    }

    private async Task<Response?> GetResponseAsync<TResponse>(string path, string? id = null)
        where TResponse : Response
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
            nmc.Add("test", _testMode == ApiTestModes.Current ? "current" : "future");
        }
        if (id is not null)
        {
            nmc.Add("id", id);
        }

        return path.ToQueryString(nmc);
    }

    private async Task<(bool success, TModel? model)> InternalGetAsync<TModel>(string requestUri)
    {
        var response = await _httpClient.GetAsync(requestUri);

        if (!response.IsSuccessStatusCode)
        {
            return (false, default);
        }

        var responseString = await response.Content.ReadAsStringAsync();

        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        var model = JsonSerializer.Deserialize<TModel>(responseString, options);
        return (true, model);
    }

    public void Dispose()
    {
        _httpClient?.Dispose();
    }

    #endregion
}
