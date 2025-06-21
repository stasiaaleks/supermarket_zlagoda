namespace ShopApp.Data.DTO;

public class CashierPromProductsNumericData
{
    public string IdEmployee { get; set; }
    public string Surname { get; set; }
    public string Name { get; set; }
    public string Patronymic { get; set; }
    public int PromoProductQuantity { get; set; }
    public int TotalPromo { get; set; }
}

public class CashierCheckData
{
    public string IdEmployee { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string CheckNumber { get; set; }
}

public class ProductsSoldOnlyPromo
{
    public string UPC { get; set; }
    public string IdProduct { get; set; }
    public string ProductName { get; set; }
    public int TotalQuantity { get; set; }
}

public class CashierChecksCountData
{
    public string IdEmployee { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public int ChecksAmount { get; set; }
}
public class CashierProductsCertainCategory
{
    public string Name { get; set; }
    public string Surname { get; set; }
}
public class CustomersWithNumCategories
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public int CategoryCount { get; set; }
    public int ProductCount { get; set; }
}