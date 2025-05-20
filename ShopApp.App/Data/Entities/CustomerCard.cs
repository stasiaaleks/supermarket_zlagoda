namespace ShopApp.Data.Entities;

public interface ICustomerCard
{
    string CardNumber { get; set; }
    string CustSurname { get; set; }
    string CustName { get; set; }
    string? CustPatronymic { get; set; }
    string PhoneNumber { get; set; }
    string? City { get; set; }
    string? Street { get; set; }
    string? ZipCode { get; set; }
    int Percent { get; set; }
}
public class CustomerCard : ICustomerCard
{
    public string CardNumber { get; set; } = string.Empty;
    public string CustSurname { get; set; } = string.Empty;
    public string CustName { get; set; } = string.Empty;
    public string? CustPatronymic { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;
    public string? City { get; set; }
    public string? Street { get; set; }
    public string? ZipCode { get; set; }
    public int Percent { get; set; }
}
