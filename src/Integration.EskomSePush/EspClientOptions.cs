using Integration.EskomSePush;
using Integration.EskomSePush.Models.Responses.Caching;

namespace Eksdom.EskomSePush.Client;

public class EspClientOptions
{
    /// <summary>
    /// Obtain licence key from EskomSePush <see href="https://eskomsepush.gumroad.com/l/api"/>
    /// </summary>
    public string LicenceKey { get; }

    /// <summary>
    /// Developer testing option
    /// </summary>
    public ApiTestModes TestMode { get; init; } = ApiTestModes.None;

    /// <summary>
    /// Override default http response caching of 2 hours
    /// </summary>
    public TimeSpan CacheDuration { get; }

    /// <summary>
    /// Supplied <see cref="HttpClient"/>
    /// </summary>
    public HttpClient? httpClient { get; init; }

    /// <summary>
    /// Override response caching. Caching is used in conjunction with <see cref="cacheDuration"/></param> to avoid making too many API calls.
    /// </summary>
    public IResponseCache? ResponseCache { get; init; } 

    public EspClientOptions(string licenceKey, TimeSpan? cacheDuration = null)
    {
        LicenceKey = licenceKey;
        CacheDuration = cacheDuration ?? TimeSpan.FromHours(2);
        if (CacheDuration.TotalMinutes < 5)
        {
            CacheDuration = TimeSpan.FromMinutes(5);
        }
    }
}
