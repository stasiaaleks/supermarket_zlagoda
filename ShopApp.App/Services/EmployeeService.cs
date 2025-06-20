using AutoMapper;
using ShopApp.DAL.Repository;
using ShopApp.Data.DTO;
using ShopApp.Data.Entities;
using ShopApp.Data.QueriesAccess;
using ShopApp.Data.SearchCriteria;

namespace ShopApp.Services;


public interface IEmployeeService
{
    Task<IEnumerable<EmployeeDto>> GetAll();
    Task<IEnumerable<EmployeeDto>> GetAllCashiers();
    Task<string> GetEmployeeRole(string id);
    Task<EmployeeDto> GetById(string id);
    Task<EmployeeDto> GetByUsername(string username);
    Task<IEnumerable<EmployeeContactsDto>> GetContactsBySurname(string surname);
    Task<string> CreateEmployee(CreateEmployeeDto dto);
    Task<string> UpdateEmployee(EmployeeDto dto);
    Task<bool> DeleteEmployee(string id);
    Task<PersonalEmployeeInfoDto> GetAllPersonalInfo(string id);
    Task<IEnumerable<EmployeeDto>> Filter(EmployeeSearchCriteria criteria);
}

public class EmployeeService: IEmployeeService
{
    private readonly IRepository<Employee> _employeeRepo;
    private readonly IUserService _userService;
    private readonly ICheckService _checkService;
    private readonly EmployeeQueryProvider _queryProvider;
    private readonly IMapper _mapper;
    
    public EmployeeService(IRepository<Employee> employeeRepo, EmployeeQueryProvider queryProvider, IMapper mapper, IUserService userService, ICheckService checkService)
    {
        _employeeRepo = employeeRepo;
        _queryProvider = queryProvider;
        _mapper = mapper;
        _userService = userService;
        _checkService = checkService;
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
            throw new KeyNotFoundException($"Employee with id '{id}' not found.");

        return _mapper.Map<EmployeeDto>(employee);
    }
    
    public async Task<EmployeeDto> GetByUsername(string username)
    {
        var query = _queryProvider.GetByUsername; 
        var employee = await _employeeRepo.GetSingleAsync(query, new { Username = username }); 
        
        if (employee == null)
            throw new KeyNotFoundException($"Employee with username '{username}' not found.");

        return _mapper.Map<EmployeeDto>(employee);
    }

    public async Task<IEnumerable<EmployeeContactsDto>> GetContactsBySurname(string surname) 
    {
        var query = _queryProvider.GetContactsBySurname; 
        var employeeContacts = await _employeeRepo.GetAllAsync<EmployeeContactsDto>(query, new { Surname = surname });
        
        if (employeeContacts == null)
            throw new KeyNotFoundException($"Contacts for employee with surname '{surname}' not found.");
        
        return employeeContacts;
    }

    public async Task<string> GetEmployeeRole(string id)
    {
        var query = _queryProvider.GetById; 
        var employee = await _employeeRepo.GetByIdAsync(query, id);
        
        if (employee == null || string.IsNullOrEmpty(employee.Role))
            throw new KeyNotFoundException($"Employee with ID '{id}' not found or role is missing.");

        return employee.Role;
    }
    
    public async Task<string> CreateEmployee(CreateEmployeeDto dto)
    {
        var employeeToCreate = _mapper.Map<Employee>(dto);
        employeeToCreate.IdEmployee = await _employeeRepo.GetNextPrefixedStringId("E", _queryProvider.GetSeqNextVal);
        var createdEntityId = await _employeeRepo.InsertAsync<string>(employeeToCreate, _queryProvider.CreateSingle);
        
        if (createdEntityId == null) throw new ArgumentException($"Failed to create an employee {dto.Name} {dto.Surname}");
        return createdEntityId;
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

    public async Task<PersonalEmployeeInfoDto> GetAllPersonalInfo(string id)
    {
        var employee = await _employeeRepo.GetByIdAsync(_queryProvider.GetById, id);
        var user = await _userService.GetByEmployeeId(id);
        var checks = await _checkService.GetAllWithSalesByPeriodAndCashier(employee.DateOfStart, DateTime.Today, id);
        return MapToPersonalInfoDto(employee, user.Username, checks);
    }
    
    private PersonalEmployeeInfoDto MapToPersonalInfoDto(Employee employee, string username, IEnumerable<CheckWithSalesListDto> checks)
    {
        var dto = _mapper.Map<PersonalEmployeeInfoDto>(employee);
        dto.Username = username;
        dto.Checks = checks.ToList();
        return dto;
    }
    
    public async Task<IEnumerable<EmployeeDto>> Filter(EmployeeSearchCriteria criteria)
    {
        var query = _queryProvider.GetAll;
        var employees = await _employeeRepo.FilterByPredicateAsync<EmployeeDto>(query, criteria);
        return employees;
    }
} 
