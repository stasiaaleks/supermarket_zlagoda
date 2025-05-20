namespace ShopApp.Data.Entities;

public interface ICheck
{
    string CheckNumber { get; set; }
    string IdEmployee { get; set; }
    string? CardNumber { get; set; }
    DateTime PrintDate { get; set; }
    decimal SumTotal { get; set; }
    decimal VAT { get; set; }
}
public class Check : ICheck
{
    public string CheckNumber { get; set; } = string.Empty;
    public string IdEmployee { get; set; } = string.Empty;
    public string? CardNumber { get; set; }
    public DateTime PrintDate { get; set; }
    public decimal SumTotal { get; set; }
    public decimal VAT { get; set; }
}
