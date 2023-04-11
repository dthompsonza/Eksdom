#pragma warning disable CS8618

namespace Integration.EskomSePush.Models.Responses;

public class AreaResponse : Response
{
    public List<AreaEventResponse> Events { get; set; }

    public AreaInfoResponse Info { get; set; }

    public AreaScheduleResponse Schedule { get; set; }
}
