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