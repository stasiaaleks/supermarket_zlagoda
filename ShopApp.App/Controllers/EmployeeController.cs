using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using ShopApp.Data.DTO;
using ShopApp.Data.Enums;
using ShopApp.Services;
using ShopApp.Services.Auth;

namespace ShopApp.Controllers;

[ApiController]
[Route("api/employees")]
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeService _employeeService;

    public EmployeeController(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }
    
    [HttpGet]
    [VerifyRole(EmployeeRoles.Manager)]
    [SwaggerOperation(Description = "Returns a list of employees sorted by surname.")]
    [ProducesResponseType(typeof(IEnumerable<EmployeeDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var employees = await _employeeService.GetAll();
        return Ok(employees);
    }
    
    [HttpGet("cashiers")]
    [VerifyRole(EmployeeRoles.Manager)]
    [SwaggerOperation(Description = "Returns a list of cashiers sorted by surname.")]
    [ProducesResponseType(typeof(IEnumerable<EmployeeDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllCashiers()
    {
        var employees = await _employeeService.GetAllCashiers();
        return Ok(employees);
    }
    
    [HttpGet("{id}")]
    [VerifyRole(EmployeeRoles.Manager, EmployeeRoles.Cashier)]
    [OnlySelf(nameof(id), EmployeeRoles.Cashier)]
    [ProducesResponseType(typeof(EmployeeDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById([FromRoute] string id)
    {
        var employee = await _employeeService.GetById(id);
        return Ok(employee);
    }
    
    [HttpGet("contacts")]
    [VerifyRole(EmployeeRoles.Manager)]
    [ProducesResponseType(typeof(EmployeeContactsDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetEmployeeContacts([FromQuery] string surname)
    {
        var contactsDto = await _employeeService.GetContactsBySurname(surname);
        return Ok(contactsDto);
    }

    [HttpGet("{id}/personal-info/all")]
    [VerifyRole(EmployeeRoles.Cashier)]
    [OnlySelf(nameof(id), EmployeeRoles.Cashier)]
    [ProducesResponseType(typeof(PersonalEmployeeInfoDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllPersonalInfo([FromRoute] string id)
    {
        var info = await _employeeService.GetAllPersonalInfo(id);
        return Ok(info);
    }
    
    [HttpPost]
    [VerifyRole(EmployeeRoles.Manager)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateEmployeeDto dto)
    {
        var id = await _employeeService.CreateEmployee(dto);
        if (id == null) return BadRequest();
        return Created();
    }
    
    [HttpPut]
    [VerifyRole(EmployeeRoles.Manager, EmployeeRoles.Cashier)]
    [OnlySelf(nameof(dto.IdEmployee), EmployeeRoles.Cashier)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Update([FromBody] EmployeeDto dto)
    {
        var id = await _employeeService.UpdateEmployee(dto);
        if (id == null) return BadRequest();
        return NoContent();
    }
    
    [HttpDelete("{id}")]
    [VerifyRole(EmployeeRoles.Manager, EmployeeRoles.Cashier)]
    [OnlySelf(nameof(id), EmployeeRoles.Cashier)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete([FromRoute] string id)
    {
        var deleted = await _employeeService.DeleteEmployee(id);
        if (!deleted) return BadRequest();
        return NoContent();
    }
}
