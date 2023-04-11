#pragma warning disable CS8618

namespace Integration.EskomSePush.Models.Responses;

public class StatusResponse : Response
{
    public StatusDetailResponse Eskom { get; set; }

    public StatusDetailResponse CapeTown { get; set; }
}
