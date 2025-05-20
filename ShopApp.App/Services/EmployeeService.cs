using ShopApp.DAL.Queries;
using ShopApp.DAL.Repository;
using ShopApp.Data.Entities;
using ShopApp.Data.QueriesAccess;

namespace ShopApp.Services;


public interface IEmployeeService
{
    Task<string> GetEmployeeRoleAsync(string id);
    Task<Employee> GetEmployeeByUsernameAsync(string username); // change to DTO
}

public class EmployeeService: IEmployeeService
{
    private readonly IRepository<Employee> _employeeRepo;
    private readonly IReadonlyRegistry _sqlQueryRegistry;
    private readonly EmployeeQueryProvider _queryProvider;
    
    public EmployeeService(IRepository<Employee> employeeRepo, IReadonlyRegistry sqlQueryRegistry, EmployeeQueryProvider queryProvider)
    {
        _employeeRepo = employeeRepo;
        _sqlQueryRegistry = sqlQueryRegistry;
        _queryProvider = queryProvider;
    }
    
    public async Task<Employee> GetEmployeeByUsernameAsync(string username)
    {
        var query = _sqlQueryRegistry.Load(_queryProvider.GetByUsername); 
        var employee = await _employeeRepo.GetSingleAsync(query, new { Username = username }); 
        
        if (employee == null)
            throw new KeyNotFoundException($"Employee with username '{username}' not found.");

        return employee;
    }

    public async Task<string> GetEmployeeRoleAsync(string id)
    {
        var query = _sqlQueryRegistry.Load(_queryProvider.GetById); 
        var employee = await _employeeRepo.GetByIdAsync(query, id);
        
        if (employee == null || string.IsNullOrEmpty(employee.Role))
            throw new KeyNotFoundException($"Employee with ID '{id}' not found or role is missing.");

        return employee.Role;
    }
}