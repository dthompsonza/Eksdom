using System.ComponentModel.DataAnnotations;

namespace Integration.EskomSePush.Models.Responses;

internal class AllowanceResponse : ResponseModel
{
    public AllowanceDetail Allowance { get; set; } = new AllowanceDetail();
}

internal class AllowanceDetail
{
    [Required]
    public int Count { get; set; }

    [Required]
    public int Limit { get; set; }
    
    public string? Type { get; set; }
}
