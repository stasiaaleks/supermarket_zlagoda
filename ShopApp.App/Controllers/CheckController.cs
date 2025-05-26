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
    
    [HttpGet("with-sales")]
    [VerifyRole(EmployeeRoles.Manager, EmployeeRoles.Cashier)]
    [ProducesResponseType(typeof(IEnumerable<CheckWithSalesListDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCheckById([FromQuery] DateTime start, [FromQuery] DateTime end)
    {
        var checks = await _checkService.GetAllWithSalesByPeriod(start, end);
        return Ok(checks);
    }
    
    
}
