#pragma warning disable CS8618

using System.Text.Json.Serialization;

namespace Integration.EskomSePush.Models.Responses;

public class StatusNextStage
{
    public int Stage { get; set; }

    [JsonPropertyName("stage_start_timestamp")]
    public DateTimeOffset Starts { get;set; }
}
