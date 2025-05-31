namespace ShopApp.Data.DTO;

public class StoreProductDto {
    public string? UPC { get; set; }
    public decimal SellingPrice { get; set; }
    public int ProductsNumber { get; set; }
    public string ProductName { get; set; }
    public string Characteristics { get; set; }
}

public class StoreProductPriceNumberDto
{
    public string? UPC { get; set; }
    public decimal SellingPrice { get; set; }
    public int ProductsNumber { get; set; }
}