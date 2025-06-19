namespace ShopApp.Data.DTO;

public class ProductDto
{
    public int? IdProduct { get; set; }
    public int CategoryNumber { get; set; }
    public string Manufacturer { get; set; }
    public string ProductName { get; set; }
    public string Characteristics { get; set; }
}

public class CreateProductDto
{
    public int CategoryNumber { get; set; }
    public string ProductName { get; set; }
    public string Manufacturer { get; set; }
    public string Characteristics { get; set; }
}

public class UpdateProductDto
{
    public int IdProduct { get; set; }
    public int CategoryNumber { get; set; }
    public string ProductName { get; set; }
    public string Manufacturer { get; set; }
    public string Characteristics { get; set; }
}

public class ProductWithCategoryDto
{
    public int? IdProduct { get; set; }
    public int CategoryNumber { get; set; }
    public string CategoryName { get; set; }
    public string Manufacturer { get; set; }
    public string ProductName { get; set; }
    public string Characteristics { get; set; }
}


