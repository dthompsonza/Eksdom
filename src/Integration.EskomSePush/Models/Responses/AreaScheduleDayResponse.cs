#pragma warning disable CS8618

namespace Integration.EskomSePush.Models.Responses;

public class AreaScheduleDayResponse
{
    public string Name { get; set; }

    public DateOnly Date { get; set; }

    public List<AreaScheduleDayStageResponse> Stages { get; set; }

}
