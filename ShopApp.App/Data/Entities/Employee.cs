namespace ShopApp.Data.Entities;
public interface IEmployee
{
    string IdEmployee { get; set; }
    string EmplSurname { get; set; }
    string EmplName { get; set; }
    string EmplPatronymic { get; set; }
    string EmplRole { get; set; }
    decimal Salary { get; set; }
    DateTime DateOfBirth { get; set; }
    DateTime DateOfStart { get; set; }
    string PhoneNumber { get; set; }
    string? City { get; set; }
    string? Street { get; set; }
    string? ZipCode { get; set; }
}
public class Employee : IEmployee
{
    public string IdEmployee { get; set; } = string.Empty;
    public string EmplSurname { get; set; } = string.Empty;
    public string EmplName { get; set; } = string.Empty;
    public string EmplPatronymic { get; set; } = string.Empty;
    public string EmplRole { get; set; } = string.Empty;
    public decimal Salary { get; set; }
    public DateTime DateOfBirth { get; set; }
    public DateTime DateOfStart { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;
    public string? City { get; set; }
    public string? Street { get; set; }
    public string? ZipCode { get; set; }
}
