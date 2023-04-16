using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Integration.EskomSePush;

internal static class Extensions
{
    internal static List<TOut> SelectList<TOut, TIn>(this List<TIn> list, Func<TIn, TOut> map)
    {
        return list.Select(item => map(item)).ToList();
    }

    internal static string ToQueryString(this string path, NameValueCollection parameters)
    {
        return $"{path}?{GetEncodedQueryString(parameters)}";
    }

    internal static string Hash(this string input)
    {
        var inputBytes = Encoding.UTF8.GetBytes(input);
        using MD5 md5 = MD5.Create();
        var hashBytes = md5.ComputeHash(inputBytes);
        var hash = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

        return hash;
    }

    private static string GetEncodedQueryString(NameValueCollection parameters)
    {
        var queryBuilder = new StringBuilder();

        foreach (string key in parameters.Keys)
        {
            var encodedKey = HttpUtility.UrlEncode(key);
            var values = parameters.GetValues(key);
            foreach (string value in values!)
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
