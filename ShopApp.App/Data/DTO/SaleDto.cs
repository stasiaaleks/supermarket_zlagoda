namespace ShopApp.Data.DTO;

public interface ISaleDto {
    public string UPC { get; set; }
    public string CheckNumber { get; set; }
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
}

public class CreateSaleDto : SaleDto
{
}

public class ProductsSoldDto
{
    public int TotalSold { get; set; }
}
