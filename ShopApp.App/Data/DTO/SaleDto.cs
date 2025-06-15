namespace ShopApp.Data.DTO;

public interface ISaleDto {
    public string UPC { get; set; }
    public int ProductNumber { get; set; }
    public string? ProductName { get; set; }
    public decimal SellingPrice { get; set; }
}

public class SaleDto: ISaleDto
{
    public string UPC { get; set; }
    public string CheckNumber { get; set; }
    public int ProductNumber { get; set; }
    public string? ProductName { get; set; }
    public decimal SellingPrice { get; set; }
    public int? TotalPricePerProduct { get; set; }
}

public class CreateSaleDto : ISaleDto
{
    public string UPC { get; set; }
    public int ProductNumber { get; set; }
    public string? ProductName { get; set; }
    public decimal SellingPrice { get; set; }
}

public class ProductsSoldDto
{
    public int TotalSold { get; init; }
}
