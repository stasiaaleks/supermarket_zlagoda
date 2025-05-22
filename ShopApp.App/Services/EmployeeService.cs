using AutoMapper;
using ShopApp.DAL.Repository;
using ShopApp.Data.DTO;
using ShopApp.Data.Entities;
using ShopApp.Data.QueriesAccess;

namespace ShopApp.Services;


public interface IEmployeeService
{
    Task<string> GetEmployeeRoleAsync(string id);
    Task<Employee> GetEmployeeByUsernameAsync(string username); // change to DTO
    Task<string> CreateEmployee(EmployeeDto dto);
}

public class EmployeeService: IEmployeeService
{
    private readonly IRepository<Employee> _employeeRepo;
    private readonly EmployeeQueryProvider _queryProvider;
    private readonly IMapper _mapper;
    
    public EmployeeService(IRepository<Employee> employeeRepo, EmployeeQueryProvider queryProvider, IMapper mapper)
    {
        _employeeRepo = employeeRepo;
        _queryProvider = queryProvider;
        _mapper = mapper;
    }
    
    public async Task<Employee> GetEmployeeByUsernameAsync(string username)
    {
        var query = _queryProvider.GetByUsername; 
        var employee = await _employeeRepo.GetSingleAsync(query, new { Username = username }); 
        
        if (employee == null)
            throw new NullReferenceException($"Employee with username '{username}' not found.");

        return employee;
    }

    public async Task<string> GetEmployeeRoleAsync(string id)
    {
        var query = _queryProvider.GetById; 
        var employee = await _employeeRepo.GetByIdAsync(query, id);
        
        if (employee == null || string.IsNullOrEmpty(employee.Role))
            throw new NullReferenceException($"Employee with ID '{id}' not found or role is missing.");

        return employee.Role;
    }
    
    public async Task<string> CreateEmployee(EmployeeDto dto)
    {
        var employeeToCreate = _mapper.Map<Employee>(dto);
        employeeToCreate.IdEmployee = await GetNextEmployeeId();
        var createdEntityId = await _employeeRepo.InsertAsync<string>(employeeToCreate, _queryProvider.CreateSingle);
        var newEmployee = await _employeeRepo.GetByIdAsync( _queryProvider.GetById, createdEntityId);

        if (newEmployee == null) throw new NullReferenceException($"Failed to create an employee {dto.Name} {dto.Surname}");
        
        return newEmployee.IdEmployee;
    }

    private async Task<string> GetNextEmployeeId()
    {
        var nextval = await _employeeRepo.ExecuteScalarAsync<string>(_queryProvider.GetSeqNextVal);
        return $"E{nextval}";
    }
} 