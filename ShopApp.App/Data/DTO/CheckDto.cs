namespace ShopApp.Data.DTO;

public interface ICheckDto
{
    public string IdEmployee { get; set; }
    public string? CardNumber { get; set; }
    public DateTime PrintDate { get; set; }
    public decimal SumTotal { get; set; }
    public decimal VAT { get; set; }
}

public class CheckDto: ICheckDto
{
    public string CheckNumber { get; set; }
    public string IdEmployee { get; set; }
    public string? CardNumber { get; set; }
    public DateTime PrintDate { get; set; }
    public decimal SumTotal { get; set; }
    public decimal VAT { get; set; }
}



public class CheckWithSaleDto: ICheckDto, ISaleDto
{
    public string CheckNumber { get; set; }
    public string IdEmployee { get; set; }
    public string? CardNumber { get; set; }
    public DateTime PrintDate { get; set; }
    public decimal SumTotal { get; set; }
    public decimal VAT { get; set; }
    public string UPC { get; set; }
    public string ProductName { get; set; }
    public int ProductNumber { get; set; }
    public int TotalPricePerProduct { get; set; }
    public decimal SellingPrice { get; set; }
}

public class CheckWithSalesListDto: ICheckDto
{
    public string CheckNumber { get; set; }
    public string IdEmployee { get; set; }
    public string? CardNumber { get; set; }
    public DateTime PrintDate { get; set; }
    public decimal SumTotal { get; set; }
    public decimal VAT { get; set; }
    public List<SaleDto> Sales { get; set; } = [];
}

public class CheckSumDto
{
    public decimal TotalSum { get; set; }
}

