namespace ShopApp.Data.Entities;

public interface ISale
{
    string UPC { get; set; }
    string CheckNumber { get; set; }
    int ProductNumber { get; set; }
    decimal SellingPrice { get; set; }
}
public class Sale : ISale
{
    public string UPC { get; set; } = string.Empty;
    public string CheckNumber { get; set; } = string.Empty;
    public int ProductNumber { get; set; }
    public decimal SellingPrice { get; set; }
}
