using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Integration.EskomSePush.Models.Responses;

internal class StatusResponse : ResponseModel
{
    public StatusDetailDto Eskom { get; set; } = new StatusDetailDto();

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
