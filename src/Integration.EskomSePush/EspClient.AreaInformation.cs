using Eksdom.Client.Models;
using Eksdom.Shared;
using EnsureThat;

namespace Eksdom.Client;

public sealed partial class EspClient : IDisposable
{
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
}
