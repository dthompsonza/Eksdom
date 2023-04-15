using System.Collections.Specialized;
using System.Text;
using System.Web;

namespace Integration.EskomSePush;

internal static class Extensions
{
    public static string ToQueryString(this string path, NameValueCollection parameters)
    {
        return $"{path}?{GetEncodedQueryString(parameters)}";
    }

    private static string GetEncodedQueryString(NameValueCollection parameters)
    {
        var queryBuilder = new StringBuilder();

        foreach (string key in parameters.Keys)
        {
            var encodedKey = HttpUtility.UrlEncode(key);
            var values = parameters.GetValues(key);
            foreach (string value in values)
            {
                string encodedValue = HttpUtility.UrlEncode(value);
                queryBuilder.Append($"{encodedKey}={encodedValue}&");
            }
        }

        // Remove the last "&" character from the string.
        if (queryBuilder.Length > 0)
        {
            queryBuilder.Length--;
        }

        return queryBuilder.ToString();
    }
}
