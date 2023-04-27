using Eksdom.Shared;
using Eksdom.Client.Models;

namespace Eksdom.Client;

public sealed partial class EspClient : IDisposable
{
    /// <summary>
    /// Check allowance allocated for token.
    /// </summary>
    /// <returns></returns>
    public Allowance? GetAllowance()
    {
        var response = GetResponse<AllowanceResponse>(Constants.Resources.Allowance, cache: false, apiCost: 0);
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
        var response = await GetResponseAsync<AllowanceResponse>(Constants.Resources.Allowance, cache: false, apiCost: 0);
        if (response is null) return null;
        var result = ((AllowanceResponse)response).Map();
        return result;
    }

}
