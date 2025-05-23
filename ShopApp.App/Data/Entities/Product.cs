public interface IProduct
{
    int IdProduct { get; set; }
    int CategoryNumber { get; set; }
    string ProductName { get; set; }
    string Characteristics { get; set; }
    string Manufacturer { get; set; }
}
public class Product : IProduct
{
    public int IdProduct { get; set; }
    public int CategoryNumber { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string Characteristics { get; set; } = string.Empty;
    public string Manufacturer { get; set; } = string.Empty;
}
