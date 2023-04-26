using Eksdom.EskomSePush.Client;
using Eksdom.Shared;
using EnsureThat;
using Integration.EskomSePush.Models.Responses;

namespace Integration.EskomSePush;

/// <summary>
/// Client for <see href="https://eskomsepush.gumroad.com/l/api">EskomSePush API</see> 2.0. 
/// <para>
///     Documentation on <see href="https://documenter.getpostman.com/view/1296288/UzQuNk3E">Postman Documenter</see>.
/// </para>
/// </summary>
public sealed partial class EspClient : IDisposable
{
    #region Static initializers

    /// <summary>
    /// Returns a singleton instance of the Eskom Se Push client
    /// </summary>
    /// <param name="options">Options to configure client</param>
    /// <returns></returns>
    public static EspClient Create(EspClientOptions options)
    {
        if (_instance is null)
        {
            _instance = new EspClient(options);
        }

        return _instance;
    }

    /// <summary>
    /// Creates and returns a new instance of the Eskom Se Push client
    /// </summary>
    /// <param name="options">Options to configure client</param>
    /// <returns></returns>
    public static EspClient CreateNonStatic(EspClientOptions options)
    {
        return new EspClient(options); 
    }

    #endregion

    #region Status

    /// <summary>
    /// The current and next loadshedding statuses for South Africa and (Optional) municipal overrides.
    /// </summary>
    /// <returns></returns>
    public Status? GetStatus()
    {
        var response = GetResponse<StatusResponse>(Constants.Resources.Status);
        if (response is null) return null;
        var result = ((StatusResponse)response).Map();
        return result;
    }

    /// <summary>
    /// The current and next loadshedding statuses for South Africa and (Optional) municipal overrides.
    /// </summary>
    /// <returns></returns>
    public async Task<Status?> GetStatusAsync()
    {
        var response = await GetResponseAsync<StatusResponse>(Constants.Resources.Status);
        if (response is null) return null;
        var result = ((StatusResponse)response).Map();
        return result;
    }

    #endregion

    #region Check allowance

    /// <summary>
    /// Check allowance allocated for token.
    /// </summary>
    /// <returns></returns>
    public Allowance? GetAllowance()
    {
        var response = GetResponse<AllowanceResponse>(Constants.Resources.Allowance, cache: false);
        if (response is null) return null;
        var result = ((AllowanceResponse)response).Map();
        return result;
    }

    /// <summary>
    /// Check allowance allocated for token.
    /// </summary>
    /// <returns></returns>
    public async Task<Allowance?> GetAllowanceAsync()
    {
        var response = await GetResponseAsync<AllowanceResponse>(Constants.Resources.Allowance, cache: false);
        if (response is null) return null;
        var result = ((AllowanceResponse)response).Map();
        return result;
    }

    #endregion

    #region Area information

    /// <summary>
    /// This single request has everything you need to monitor upcoming loadshedding events for the chosen suburb.
    /// </summary>
    /// <param name="id"></param>
    public AreaInformation? GetAreaInformation(string id)
    {
        Ensure.That(id).IsNotNullOrEmpty();

        var response = GetResponse<AreaInformationResponse>(Constants.Resources.AreaInformation, id);
        if (response is null) return null;
        var result = ((AreaInformationResponse)response).Map();
        return result;
    }

    /// <summary>
    /// This single request has everything you need to monitor upcoming loadshedding events for the chosen suburb.
    /// </summary>
    public async Task<AreaInformation?> GetAreaInformationAsync(string id)
    {
        Ensure.That(id).IsNotNullOrEmpty();

        var response = await GetResponseAsync<AreaInformationResponse>(Constants.Resources.AreaInformation, id);
        if (response is null) return null;
        var result = ((AreaInformationResponse)response).Map();
        return result;
    }

    #endregion
}
