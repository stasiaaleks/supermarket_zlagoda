namespace ShopApp.Data.DTO;

public class CashierPromProductsNumericData
{
    string IdEmployee { get; set; }
    string Surname { get; set; }
    string Name { get; set; }
    string Patronymic { get; set; }
    int PromoProductQuantity { get; set; }
    int TotalPromo { get; set; }
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
    public string TotalQuantity { get; set; }
}

public class CashierChecksCountData
{
    public string IdEmployee { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public int ChecksAmount { get; set; }
}