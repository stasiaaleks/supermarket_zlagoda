using AutoMapper;
using ShopApp.DAL.Repository;
using ShopApp.Data.DTO;
using ShopApp.Data.Entities;
using ShopApp.Data.QueriesAccess;

namespace ShopApp.Services;


public interface IEmployeeService
{
    Task<IEnumerable<EmployeeDto>> GetAll();
    Task<IEnumerable<EmployeeDto>> GetAllCashiers();
    Task<string> GetEmployeeRole(string id);
    Task<EmployeeDto> GetById(string id);
    Task<EmployeeDto> GetByUsername(string username);
    Task<EmployeeContactsDto> GetContactsBySurname(string surname);
    Task<string> CreateEmployee(CreateEmployeeDto dto);
    Task<string> UpdateEmployee(EmployeeDto dto);
    Task<bool> DeleteEmployee(string id);
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
    
    public async Task<IEnumerable<EmployeeDto>> GetAll()
    {
        var query = _queryProvider.GetAllSortedBySurname; 
        var employees = await _employeeRepo.GetAllAsync(query);

        return _mapper.Map<IEnumerable<EmployeeDto>>(employees);
    }

    public async Task<IEnumerable<EmployeeDto>> GetAllCashiers()
    {
        var query = _queryProvider.GetAllCashiersSortedBySurname; 
        var employees = await _employeeRepo.GetAllAsync(query);

        return _mapper.Map<IEnumerable<EmployeeDto>>(employees);
    }

    public async Task<EmployeeDto> GetById(string id)
    {
        var query = _queryProvider.GetById; 
        var employee = await _employeeRepo.GetByIdAsync(query,id); 
        
        if (employee == null)
            throw new NullReferenceException($"Employee with id '{id}' not found.");

        return _mapper.Map<EmployeeDto>(employee);
    }
    
    public async Task<EmployeeDto> GetByUsername(string username)
    {
        var query = _queryProvider.GetByUsername; 
        var employee = await _employeeRepo.GetSingleAsync(query, new { Username = username }); 
        
        if (employee == null)
            throw new NullReferenceException($"Employee with username '{username}' not found.");

        return _mapper.Map<EmployeeDto>(employee);
    }

    public async Task<EmployeeContactsDto> GetContactsBySurname(string surname) 
    {
        var query = _queryProvider.GetContactsBySurname; 
        var employeeContacts = await _employeeRepo.GetSingleAsync<EmployeeContactsDto>(query, new { Surname = surname });
        
        if (employeeContacts == null)
            throw new NullReferenceException($"Contacts for employee with surname '{surname}' not found.");
        
        return employeeContacts; // fix check for null
    }

    public async Task<string> GetEmployeeRole(string id)
    {
        var query = _queryProvider.GetById; 
        var employee = await _employeeRepo.GetByIdAsync(query, id);
        
        if (employee == null || string.IsNullOrEmpty(employee.Role))
            throw new NullReferenceException($"Employee with ID '{id}' not found or role is missing.");

        return employee.Role;
    }
    
    public async Task<string> CreateEmployee(CreateEmployeeDto dto)
    {
        var employeeToCreate = _mapper.Map<Employee>(dto);
        employeeToCreate.IdEmployee = await _employeeRepo.GetNextPrefixedStringId("E", _queryProvider.GetSeqNextVal);
        var createdEntityId = await _employeeRepo.InsertAsync<string>(employeeToCreate, _queryProvider.CreateSingle);
        var newEmployee = await _employeeRepo.GetByIdAsync( _queryProvider.GetById, createdEntityId);

        if (newEmployee == null) throw new NullReferenceException($"Failed to create an employee {dto.Name} {dto.Surname}");
        
        return newEmployee.IdEmployee;
    }

    public async Task<string> UpdateEmployee(EmployeeDto dto)
    {
        var employeeToUpdate = _mapper.Map<Employee>(dto);
        var updatedEntityId = await _employeeRepo.UpdateAsync<string>(employeeToUpdate, _queryProvider.UpdateById);
        return updatedEntityId;
    }

    public async Task<bool> DeleteEmployee(string id)
    {
        var affectedRows = await _employeeRepo.DeleteByIdAsync(_queryProvider.DeleteById, id);
        return affectedRows > 0;
    }
} 