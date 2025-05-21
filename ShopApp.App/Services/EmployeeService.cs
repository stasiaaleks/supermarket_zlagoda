using AutoMapper;
using ShopApp.DAL.Queries;
using ShopApp.DAL.Repository;
using ShopApp.Data.DTO;
using ShopApp.Data.Entities;
using ShopApp.Data.QueriesAccess;
using ShopApp.Repositories;

namespace ShopApp.Services;


public interface IEmployeeService
{
    Task<string> GetEmployeeRoleAsync(string id);
    Task<Employee> GetEmployeeByUsernameAsync(string username); // change to DTO
    Task<string> CreateEmployee(EmployeeDto dto);
}

public class EmployeeService: IEmployeeService
{
    private readonly IEmployeeRepository _employeeRepo;
    private readonly IReadonlyRegistry _sqlQueryRegistry;
    private readonly EmployeeQueryProvider _queryProvider;
    private readonly IMapper _mapper;
    
    public EmployeeService(IEmployeeRepository employeeRepo, IReadonlyRegistry sqlQueryRegistry, EmployeeQueryProvider queryProvider, IMapper mapper)
    {
        _employeeRepo = employeeRepo;
        _sqlQueryRegistry = sqlQueryRegistry;
        _queryProvider = queryProvider;
        _mapper = mapper;
    }
    
    public async Task<Employee> GetEmployeeByUsernameAsync(string username)
    {
        var query = _sqlQueryRegistry.Load(_queryProvider.GetByUsername); 
        var employee = await _employeeRepo.GetSingleAsync<Employee>(query, new { Username = username }); 
        
        if (employee == null)
            throw new NullReferenceException($"Employee with username '{username}' not found.");

        return employee;
    }

    public async Task<string> GetEmployeeRoleAsync(string id)
    {
        var query = _sqlQueryRegistry.Load(_queryProvider.GetById); 
        var employee = await _employeeRepo.GetByIdAsync(query, id);
        
        if (employee == null || string.IsNullOrEmpty(employee.Role))
            throw new NullReferenceException($"Employee with ID '{id}' not found or role is missing.");

        return employee.Role;
    }
    
    public async Task<string> CreateEmployee(EmployeeDto dto)
    {
        var query = _queryProvider.CreateSingle;
        var employeeToCreate = _mapper.Map<Employee>(dto);
        var createdEntity = await _employeeRepo.CreateAsync(employeeToCreate, query);
        return createdEntity.IdEmployee;
    }
} 