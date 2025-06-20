using Microsoft.AspNetCore.Mvc;
using ShopApp.Data.DTO;
using ShopApp.Services;

namespace ShopApp.Controllers;

[ApiController]
public class StatisticsController: ControllerBase
{
    private readonly IStatisticsService _statisticsService;

    public StatisticsController(IStatisticsService statisticsService)
    {
        _statisticsService = statisticsService;
    }
    
    [HttpGet("cashier-prom-products/{idEmployee}")]
    [ProducesResponseType(typeof(CashierPromProductsNumericData), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCashierPromProductsData([FromRoute] string idEmployee)
    {
        var data = await _statisticsService.GetPromProductsSumQuantityByCashier(idEmployee);
        return Ok(data);
    }
    
    [HttpGet("same-checks-as/{surname}")]
    [ProducesResponseType(typeof(IEnumerable<CashierCheckData>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCashiersWithSameChecks([FromRoute] string surname)
    {
        var data = await _statisticsService.GetCashiersWithSameChecks(surname);
        return Ok(data);
    }
    
    [HttpGet("cashiers-min-checks-products/")]
    [ProducesResponseType(typeof(IEnumerable<CashierChecksCountData>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCashiersMinProductsMinChecks([FromQuery] int minProducts, int minChecks)
    {
        var data = await _statisticsService.GetCashiersMinProductsMinChecks(minProducts, minChecks);
        return Ok(data);
    }
    
    [HttpGet("sold-only-promo-products")]
    [ProducesResponseType(typeof(CashierPromProductsNumericData), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProductsSoldOnlyPromo()
    {
        var data = await _statisticsService.GetProductsSoldOnlyPromo();
        return Ok(data);
    }
        
}