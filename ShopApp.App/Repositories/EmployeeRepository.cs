using System.Data;
using ShopApp.DAL.DbConnection;
using ShopApp.DAL.Queries;
using ShopApp.DAL.Repository;
using ShopApp.Data.Entities;

namespace ShopApp.Repositories;


public interface IEmployeeRepository
{
    Task<Employee?> GetSingleAsync(string queryFilePath, object? parameters = null);
    Task<IEnumerable<Employee>> GetListAsync(string queryFilePath, object? parameters = null);
    Task<Employee?> GetByIdAsync(string queryFilePath, string id);
    Task<Employee?> CreateAsync(Employee entity, string queryFilePath);
    Task<Employee?> UpdateAsync(Employee entity, string queryFilePath);
    Task<Employee?> DeleteAsync(Employee entity, string queryFilePath);
}

public class EmployeeRepository : Repository<Employee>, IEmployeeRepository
{
    private readonly IReadonlyRegistry _sqlQueryRegistry;

    public EmployeeRepository(IConnectionProvider dbConnectionProvider, IReadonlyRegistry sqlQueryRegistry)
        : base(dbConnectionProvider)
    {
        _sqlQueryRegistry = sqlQueryRegistry;
    }

    protected override Employee Map(IDataRecord record)
    {
        return new Employee
        {
            IdEmployee = record.GetString(record.GetOrdinal("id_employee")),
            Surname = record.GetString(record.GetOrdinal("empl_surname")),
            Name = record.GetString(record.GetOrdinal("empl_name")),
            Patronymic = record.GetString(record.GetOrdinal("empl_patronymic")),
            Role = record.GetString(record.GetOrdinal("empl_role")),
            Salary = record.GetDecimal(record.GetOrdinal("salary")),
            DateOfBirth = record.GetDateTime(record.GetOrdinal("date_of_birth")),
            DateOfStart = record.GetDateTime(record.GetOrdinal("date_of_start")),
            PhoneNumber = record.GetString(record.GetOrdinal("phone_number")),
            City = record.IsDBNull(record.GetOrdinal("city")) ? null : record.GetString(record.GetOrdinal("city")),
            Street = record.IsDBNull(record.GetOrdinal("street")) ? null : record.GetString(record.GetOrdinal("street")),
            ZipCode = record.IsDBNull(record.GetOrdinal("zip_code")) ? null : record.GetString(record.GetOrdinal("zip_code"))
        };
    }

    public Task<Employee?> GetSingleAsync(string queryFilePath, object? parameters = null)
    {
        var query = _sqlQueryRegistry.Load(queryFilePath); 
        return QuerySingleAsync(query, parameters);
    }

    public Task<IEnumerable<Employee>> GetListAsync(string queryFilePath, object? parameters = null)
    {
        var query = _sqlQueryRegistry.Load(queryFilePath); 
        return QueryListAsync(query);
    }

    public Task<Employee?> GetByIdAsync(string queryFilePath, string id)
    {
        var query = _sqlQueryRegistry.Load(queryFilePath); 
        return QuerySingleAsync(query, new { Id = id });
    }

    public Task<Employee?> CreateAsync(Employee entity, string queryFilePath)
    {
        var query = _sqlQueryRegistry.Load(queryFilePath);
        var parameters = new
        {
            IdEmployee = entity.IdEmployee,
            Surname = entity.Surname,
            Name = entity.Name,
            Patronymic = entity.Patronymic,
            Role = entity.Role,
            Salary = entity.Salary,
            DateOfBirth = entity.DateOfBirth,
            DateOfStart = entity.DateOfStart,
            PhoneNumber = entity.PhoneNumber,
            City = entity.City,
            Street = entity.Street,
            ZipCode = entity.ZipCode
        };

        return ExecuteAsync(query, parameters);
    }

    public Task<Employee?> UpdateAsync(Employee entity, string queryFilePath)
    {
        var query = _sqlQueryRegistry.Load(queryFilePath);
        var parameters = new
        {
            IdEmployee = entity.IdEmployee,
            Surname = entity.Surname,
            Name = entity.Name,
            Patronymic = entity.Patronymic,
            Role = entity.Role,
            Salary = entity.Salary,
            DateOfBirth = entity.DateOfBirth,
            DateOfStart = entity.DateOfStart,
            PhoneNumber = entity.PhoneNumber,
            City = entity.City,
            Street = entity.Street,
            ZipCode = entity.ZipCode
        };

        return ExecuteAsync(query, parameters);
    }

    public Task<Employee?> DeleteAsync(Employee entity, string queryFilePath)
    {
        var query = _sqlQueryRegistry.Load(queryFilePath);
        var parameters = new { userid = entity.IdEmployee };
        return ExecuteAsync(query, parameters);
    }

    // private async Task<string> GenerateNewEmployeeIdAsync()
    // { }
}