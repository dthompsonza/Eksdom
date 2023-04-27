using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Eksdom.Client.Models;

internal class StatusResponseRequiredPropertyConverter : JsonConverter<StatusResponse>
{
    public override StatusResponse Read(
        ref Utf8JsonReader reader,
        Type type,
        JsonSerializerOptions options)
    {
        // Don't pass in options when recursively calling Deserialize.
        StatusResponse x = JsonSerializer.Deserialize<StatusResponse>(ref reader)!;

        // Check for required fields set by values in JSON
        //return forecast!.Date == default
        //    ? throw new JsonException("Required property not received in the JSON")
        //    : forecast;
        return x;
    }

    public override void Write(
        Utf8JsonWriter writer,
        StatusResponse forecast, JsonSerializerOptions options)
    {
        // Don't pass in options when recursively calling Serialize.
        JsonSerializer.Serialize(writer, forecast);
    }
}

internal class StatusResponse : ResponseModel
{
    public StatusPayloadResponseDto Status { get; set; } = new StatusPayloadResponseDto();
}

internal class StatusPayloadResponseDto
{
    [JsonPropertyName("eskom")]
    public StatusDetailDto Eskom { get; set; } = new StatusDetailDto();

    [JsonPropertyName("capetown")]
    public StatusDetailDto CapeTown { get; set; } = new StatusDetailDto();
}

internal class StatusDetailDto
{
    [Required]
    public string? Name { get; set; }

    [Required]
    public int? Stage { get; set; }

    [Required]
    [JsonPropertyName("stage_updated")]
    public DateTimeOffset? Updated { get; set; }

    [MinLength(1)]
    [JsonPropertyName("next_stages")]

    public List<StatusNextStageDto> NextStages { get; set; } = new List<StatusNextStageDto>();
}

internal class StatusNextStageDto
{
    [Required]
    public int? Stage { get; set; }

    [Required]
    [JsonPropertyName("stage_start_timestamp")]
    public DateTimeOffset? Starts { get; set; }
}
