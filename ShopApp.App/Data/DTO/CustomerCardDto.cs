using ShopApp.Data.Entities;

namespace ShopApp.Data.DTO;

public class CustomerCardDto: ICustomerCard
{
    public string CardNumber { get; set; }
    public string CustSurname { get; set; }
    public string CustName { get; set; }
    public string? CustPatronymic { get; set; }
    public string PhoneNumber { get; set; }
    public string? City { get; set; }
    public string? Street { get; set; }
    public string? ZipCode { get; set; }
    public int Percent { get; set; }
}