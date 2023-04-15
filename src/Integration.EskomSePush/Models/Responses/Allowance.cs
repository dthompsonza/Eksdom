#pragma warning disable CS8618

namespace Integration.EskomSePush.Models.Responses;

/// <summary>
/// Response model for 'allowance' 
/// </summary>
public class Allowance : Response
{
    /// <summary>
    /// Credit used
    /// </summary>
    public int Count { get; set; }
    
    /// <summary>
    /// Credit limit
    /// </summary>
    public int Limit { get; set; }

    /// <summary>
    /// Credit balance
    /// </summary>
    public int Balance => Math.Min(Limit - Count, 0);
    
    /// <summary>
    /// Credit type
    /// </summary>
    public string Type { get; set; }
}
