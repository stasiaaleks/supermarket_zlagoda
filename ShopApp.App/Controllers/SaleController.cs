using Microsoft.AspNetCore.Mvc;
using ShopApp.Data.DTO;
using ShopApp.Data.Enums;
using ShopApp.Services;
using ShopApp.Services.Auth;

namespace ShopApp.Controllers;

[ApiController]
[Route("api/sales")]
public class SaleController : ControllerBase
{
    private readonly ISaleService _saleService;

    public SaleController(ISaleService saleService)
    {
        _saleService = saleService;
    }

    [HttpGet]
    [VerifyRole(EmployeeRoles.Manager)]
    [ProducesResponseType(typeof(IEnumerable<SaleDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var sales = await _saleService.GetAll();
        return Ok(sales);
    }
    
    [HttpGet("{productUPC}/total")]
    [VerifyRole(EmployeeRoles.Manager)]
    [ProducesResponseType(typeof(ProductsSoldDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProductTotalSoldByPeriod([FromQuery] DateTime start, [FromQuery] DateTime end, [FromRoute] string productUPC)
    {
        var totalSold = await _saleService.GetProductTotalSoldByPeriod(start, end, productUPC);
        return Ok(totalSold);
    }
    
    [HttpPost]
    [VerifyRole(EmployeeRoles.Cashier)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] SaleDto dto)
    {
        var upc = await _saleService.CreateSale(dto);
        if (string.IsNullOrEmpty(upc)) return BadRequest();
        return Created();
    }
}
