#pragma warning disable CS8618

using System.Text.Json.Serialization;

namespace Integration.EskomSePush.Models.Responses;

public class StatusDetailResponse
{
    public string Name { get; set; }

    public int Stage { get; set; }

    [JsonPropertyName("stage_updated")]
    public DateTimeOffset Updated { get; set; }

    [JsonPropertyName("next_stages")]
    public List<StatusNextStage> NextStages { get; set; }
}
