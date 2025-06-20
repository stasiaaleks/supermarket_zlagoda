using Microsoft.AspNetCore.Mvc;
using ShopApp.Data.DTO;
using ShopApp.Services;

namespace ShopApp.Controllers;

[ApiController]
[Route("api/statistics")]
public class StatisticsController: ControllerBase
{
    private readonly IStatisticsService _statisticsService;

    public StatisticsController(IStatisticsService statisticsService)
    {
        _statisticsService = statisticsService;
    }
    
    [HttpGet("cashier-prom-products/")]
    [ProducesResponseType(typeof(IEnumerable<CashierPromProductsNumericData>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCashierPromProductsData([FromQuery] string idEmployee)
    {
        var data = await _statisticsService.GetPromProductsSumQuantityByCashier(idEmployee);
        return Ok(data);
    }
    
    [HttpGet("same-checks-as/")]
    [ProducesResponseType(typeof(IEnumerable<CashierCheckData>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCashiersWithSameChecks([FromQuery] string surname)
    {
        var data = await _statisticsService.GetCashiersWithSameChecks(surname);
        return Ok(data);
    }
    
    [HttpGet("cashiers-min-checks-products/")]
    [ProducesResponseType(typeof(IEnumerable<ProductsSoldOnlyPromo>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCashiersMinProductsMinChecks([FromQuery] int minProducts, int minChecks)
    {
        var data = await _statisticsService.GetCashiersMinProductsMinChecks(minProducts, minChecks);
        return Ok(data);
    }
    
    [HttpGet("sold-only-promo-products")]
    [ProducesResponseType(typeof(IEnumerable<ProductsSoldOnlyPromo>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProductsSoldOnlyPromo()
    {
        var data = await _statisticsService.GetProductsSoldOnlyPromo();
        return Ok(data);
    }
      
    [HttpGet("cashiers-sold-products-certain-category/")]
    [ProducesResponseType(typeof(IEnumerable<CashierProductsCertainCategory>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCashiersProductsCertainCategory([FromQuery] string categoryName)
    {
        var data = await _statisticsService.GetCashierProductsCertainCategory(categoryName);
        return Ok(data);
    }
    [HttpGet("customers-with-more-category-num")]
    [ProducesResponseType(typeof(IEnumerable<CustomersWithNumCategories>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCustomersWithNumCategories([FromQuery] int categoryNum)
    {
        var data = await _statisticsService.GetCustomersWithNumCategories(categoryNum);
        return Ok(data);
    }
}