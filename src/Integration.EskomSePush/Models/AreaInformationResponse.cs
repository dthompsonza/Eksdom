using System.ComponentModel.DataAnnotations;

namespace Eksdom.Client.Models;

internal class AreaInformationResponse : ResponseModel
{
    public List<EventDto> Events { get; set; } = new List<EventDto>();

    public InfoDto Info { get; set; } = new InfoDto();

    public ScheduleDto Schedule { get; set; } = new ScheduleDto();
}

internal class InfoDto
{
    [Required]
    public string? Name { get; set; }

    [Required]
    public string? Region { get; set; }
}

internal class EventDto
{
    [Required]
    public string? End { get; set; }

    [Required]
    public string? Note { get; set; }

    [Required]
    public string? Start { get; set; }
}

internal class ScheduleDto
{
    public List<ScheduleDayDto> Days { get; set; } = new List<ScheduleDayDto>();
}

internal class ScheduleDayDto
{
    [Required]
    public string? Date { get; set; }

    [Required]
    public string? Name { get; set; }

    [MinLength(1)]
    public List<List<string>> Stages { get; set; } = new List<List<string>>();
}