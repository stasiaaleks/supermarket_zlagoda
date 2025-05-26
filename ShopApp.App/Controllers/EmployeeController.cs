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
    
    [HttpPost]
    [VerifyRole(EmployeeRoles.Manager)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] EmployeeDto dto)
    {
        var createdEntityId = await _employeeService.CreateEmployee(dto);
        if (createdEntityId == null) return BadRequest();
        return Created();
    }
}
