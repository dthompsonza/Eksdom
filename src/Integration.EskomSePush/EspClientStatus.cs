using Eksdom.Client.Models;
using Eksdom.Shared;

namespace Eksdom.Client;

public sealed partial class EspClient : IDisposable
{
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
}
