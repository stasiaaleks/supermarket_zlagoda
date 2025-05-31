public interface IStoreProduct
{
    string UPC { get; set; }
    string? UPCProm { get; set; }
    int ProductId { get; set; }
    decimal SellingPrice { get; set; }
    int ProductsNumber { get; set; }
    bool PromotionalProduct { get; set; }
}

public class StoreProduct : IStoreProduct
{
    public string UPC { get; set; } 
    public string? UPCProm { get; set; }
    public int ProductId { get; set; }
    public decimal SellingPrice { get; set; }
    public int ProductsNumber { get; set; }
    public bool PromotionalProduct { get; set; }
}