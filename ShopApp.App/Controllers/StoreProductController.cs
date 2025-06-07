using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopApp.Data.DTO;
using ShopApp.Data.Enums;
using ShopApp.Data.SearchCriteria;
using ShopApp.Services;
using ShopApp.Services.Auth;

namespace ShopApp.Controllers;

[ApiController]
[Route("api/store-products")]
public class StoreProductController : ControllerBase
{
    private readonly IStoreProductService _productService;

    public StoreProductController(IStoreProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    [VerifyRole(EmployeeRoles.Manager, EmployeeRoles.Cashier)]
    [ProducesResponseType(typeof(IEnumerable<StoreProductDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var products = await _productService.GetAll();
        return Ok(products);
    }
    
    [HttpGet("availability/restricted/{upc}")]
    [VerifyRole(EmployeeRoles.Cashier)]
    [ProducesResponseType(typeof(StoreProductPriceNumberDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRestrictedAvailabilityByUpc([FromRoute] string upc)
    {
        var dto = await _productService.GetPriceAndNumberByUpc(upc);
        return Ok(dto);
    }
    
    [HttpGet("availability/{upc}")]
    [VerifyRole(EmployeeRoles.Manager)]
    [ProducesResponseType(typeof(StoreProductDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAvailabilityByUpc([FromRoute] string upc)
    {
        // TODO: rename query from GetProductInfoByUpc (ambiguous)
        var dto = await _productService.GetProductInfoByUpc(upc);
        return Ok(dto);
    }
    
    [HttpGet("filter")]
    [Authorize]
    [ProducesResponseType(typeof(IEnumerable<StoreProductDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> FilterProducts([FromQuery] StoreProductSearchCriteria searchCriteria)
    {
        var products = await _productService.Filter(searchCriteria);
        return Ok(products);
    }
    
    [HttpGet("promotional")]
    [Authorize]
    [ProducesResponseType(typeof(IEnumerable<StoreProductDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> FilterPromotionalProducts([FromQuery] StoreProductSearchCriteria searchCriteria)
    {
        var products = await _productService.GetFilteredPromotional(searchCriteria);
        return Ok(products);
    }
    
    [HttpGet("regular")]
    [Authorize]
    [ProducesResponseType(typeof(IEnumerable<StoreProductDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> FilterRegularProducts([FromQuery] StoreProductSearchCriteria searchCriteria)
    {
        var products = await _productService.GetFilteredRegular(searchCriteria);
        return Ok(products);
    }
}
