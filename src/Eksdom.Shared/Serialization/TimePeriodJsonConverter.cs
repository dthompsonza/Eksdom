using System.Text.Json;
using System.Text.Json.Serialization;

namespace Eksdom.Shared.Serialization;

public sealed class TimePeriodJsonConverter : JsonConverter<TimePeriod>
{
    public override TimePeriod Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return new TimePeriod(reader.GetString()!);
    }

    public override void Write(Utf8JsonWriter writer, TimePeriod value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}
