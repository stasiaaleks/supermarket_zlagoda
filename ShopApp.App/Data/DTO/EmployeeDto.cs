namespace ShopApp.Data.DTO;

public interface IEmployeeDto
{
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

public abstract class BaseEmployeeDto: IEmployeeDto
{
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

public class EmployeeDto : BaseEmployeeDto
{
    public string? IdEmployee { get; init; }
}

public class CreateEmployeeDto: BaseEmployeeDto { }

public class EmployeeContactsDto
{
    public string Surname { get; set; }
    public string Name { get; set; }
    public string PhoneNumber { get; set; }
    public string? City { get; set; }
    public string? Street { get; set; }
    public string? ZipCode { get; set; }
}

public class PersonalEmployeeInfoDto: EmployeeDto
{
    public string Username { get; set; }
    public List<CheckWithSalesListDto> Checks { get; set; } = [];
}