namespace ShopApp.Data.DTO;

public interface ILoginDto
{
    string Username { get; set; }
    string Password { get; set; }
}

public class LoginDto : ILoginDto
{
    public string Username { get; set; }
    public string Password { get; set; }
}

public class RegisterDto: IEmployeeDto, ILoginDto
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string? IdEmployee { get; set; }
    public string Surname { get; set; }
    public string Name { get; set; }
    public string Patronymic { get; set; }
    public string Role { get; set; }
    public decimal? Salary { get; set; }
    public DateTime DateOfBirth { get; set; }
    public DateTime DateOfStart { get; set; }
    public string PhoneNumber { get; set; }
    public string? City { get; set; }
    public string? Street { get; set; }
    public string? ZipCode { get; set; }
}