using Microsoft.AspNetCore.Mvc;
using ShopApp.Data.DTO;
using ShopApp.Data.Enums;
using ShopApp.Services;
using ShopApp.Services.Auth;

namespace ShopApp.Controllers;

[ApiController]
[Route("api/checks")]
public class CheckController : ControllerBase
{
    private readonly ICheckService _checkService;

    public CheckController(ICheckService checkService)
    {
        _checkService = checkService;
    }

    [HttpGet]
    [VerifyRole(EmployeeRoles.Manager)]
    [ProducesResponseType(typeof(IEnumerable<CheckDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllChecks()
    {
        var checks = await _checkService.GetAll();
        return Ok(checks);
    }
    
    [HttpGet("sales")]
    [VerifyRole(EmployeeRoles.Manager)]
    [ProducesResponseType(typeof(IEnumerable<CheckWithSalesListDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetChecksWithSales([FromQuery] DateTime? start, [FromQuery] DateTime? end)
    {
        var checks = await _checkService.GetAllWithSalesByPeriod(start, end);
        return Ok(checks);
    }
    
    [HttpGet("cashiers/{cashierId}/sales")]
    [VerifyRole(EmployeeRoles.Manager)]
    [ProducesResponseType(typeof(IEnumerable<CheckWithSalesListDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetChecksWithSalesByCashier([FromQuery] DateTime? start, [FromQuery] DateTime? end, [FromRoute] string cashierId)
    {
        var checks = await _checkService.GetAllWithSalesByPeriodAndCashier(start, end, cashierId);
        return Ok(checks);
    }
    
    [HttpGet("sum")]
    [VerifyRole(EmployeeRoles.Manager)]
    [ProducesResponseType(typeof(CheckSumDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetChecksSumByPeriod([FromQuery] DateTime? start, [FromQuery] DateTime? end)
    {
        var sumDto = await _checkService.GetSumByPeriod(start, end);
        return Ok(sumDto);
    }
    
    [HttpGet("cashiers/{cashierId}/sum")]
    [VerifyRole(EmployeeRoles.Manager)]
    [ProducesResponseType(typeof(CheckSumDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetChecksSumByPeriodAndCashier([FromQuery] DateTime? start, [FromQuery] DateTime? end, [FromRoute] string cashierId)
    {
        var sumDto = await _checkService.GetSumByEmployeeAndPeriod(start, end, cashierId);
        return Ok(sumDto);
    }
    
    [HttpGet("{number}/sales")]
    [VerifyRole(EmployeeRoles.Manager)]
    [ProducesResponseType(typeof(CheckWithSalesListDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByNumberWithSales([FromRoute] string number)
    {
        var checkDto = await _checkService.GetByNumberWithSales(number);
        return Ok(checkDto);
    }
    
    [HttpPost]
    [VerifyRole(EmployeeRoles.Cashier)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateCheckWithSalesListDto dto)
    {
        var number = await _checkService.CreateCheckWithSales(dto);
        if (number == null) return BadRequest();
        return Created();
    }
    
}
