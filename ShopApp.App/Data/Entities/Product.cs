namespace ShopApp.Data.Entities;

public interface IProduct
{
    int Barcode { get; set; }
    string Name { get; set; }
    decimal Price { get; set; }
    
}

public class Product : IProduct
{
    public int Barcode { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
}