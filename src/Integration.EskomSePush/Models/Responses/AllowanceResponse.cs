#pragma warning disable CS8618

namespace Integration.EskomSePush.Models.Responses;

public class AllowanceResponse : Response
{
    public int Count { get; set; }
    
    public int Limit { get; set; }

    public int Balance => Math.Min(Limit - Count, 0);
    
    public string Type { get; set; }
}
