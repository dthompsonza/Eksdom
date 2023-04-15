#pragma warning disable CS8618

using System.Text.Json.Serialization;

namespace Integration.EskomSePush.Models.Responses;

public class Status : Response
{
    public StatusDetail Eskom { get; set; }

    public StatusDetail CapeTown { get; set; }
}

public class StatusDetail
{
    public string Name { get; set; }

    public int Stage { get; set; }

    [JsonPropertyName("stage_updated")]
    public DateTimeOffset Updated { get; set; }

    [JsonPropertyName("next_stages")]
    public List<StatusNextStage> NextStages { get; set; }
}

public class StatusNextStage
{
    public int Stage { get; set; }

    [JsonPropertyName("stage_start_timestamp")]
    public DateTimeOffset Starts { get; set; }
}
