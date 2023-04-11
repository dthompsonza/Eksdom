using System.Collections.Specialized;
using System.Web;

namespace Integration.EskomSePush;

internal static class Extensions
{
    public static string ToQueryString(this string url, NameValueCollection source)
    {
        return url + string.Join("&", source.AllKeys
            .SelectMany(key => source.GetValues(key)
                .Select(value => string.Format("{0}={1}", HttpUtility.UrlEncode(key), HttpUtility.UrlEncode(value))))
            .ToArray());
    }
}
