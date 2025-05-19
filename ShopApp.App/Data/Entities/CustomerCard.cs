namespace ShopApp.Data.Entities;

public interface ICustomerCard
{
    string CardNumber { get; set; }
    string Surname { get; set; }
    string Name { get; set; }
    string? Patronymic { get; set; }
    string PhoneNumber { get; set; }
    string? City { get; set; }
    string? Street { get; set; }
    string? ZipCode { get; set; }
    int Percent { get; set; }
}
public class CustomerCard : ICustomerCard
{
    public string CardNumber { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Patronymic { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;
    public string? City { get; set; }
    public string? Street { get; set; }
    public string? ZipCode { get; set; }
    public int Percent { get; set; }
}
