#pragma warning disable CS8618

namespace Integration.EskomSePush.Models.Responses;

public class StatusResponse
{
    public StatusDetailResponse Eskom { get; set; }

    public StatusDetailResponse CapeTown { get; set; }
}
