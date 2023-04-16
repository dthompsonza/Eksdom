using Integration.EskomSePush;
using Integration.EskomSePush.Models.Responses.Caching;

namespace Eksdom.EskomSePush.Client
{
    public class ApiClientOptions
    {
        public string LicenceKey { get; }

        public ApiTestModes TestMode { get; set; } = ApiTestModes.None;

        public TimeSpan? CacheDuration { get; set; } 

        public HttpClient? httpClient { get; set; }

        public IResponseCache? ResponseCache { get; set; } 

        public ApiClientOptions(string licenceKey)
        {
            LicenceKey = licenceKey;
        }
    }
}
