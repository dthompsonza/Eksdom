using System.Text.Json;
using Eksdom.Shared.Serialization;

namespace Eksdom.Shared;

public static class EksdomJsonSerializer
{
    public static string ToJson(object obj, bool indented = false)
    {
        var json = JsonSerializer.Serialize(obj, CreateOptions(indented));
        return json;
    }

    public static T FromJson<T>(string json)
    {
        T? obj = JsonSerializer.Deserialize<T>(json, CreateOptions());
        return obj!;
    }

    private static JsonSerializerOptions CreateOptions(bool indented = false)
    {
        return new JsonSerializerOptions()
        {
            Converters =
            {
                new DateOnlyJsonConverter(),
            },
            WriteIndented = indented,
        };
    }
}
