#pragma warning disable CS8618

namespace Integration.EskomSePush.Models.Responses;

public class AreaScheduleResponse
{
    public List<AreaScheduleDayResponse> Days { get; set; }

    public string Source { get; set; }
}
