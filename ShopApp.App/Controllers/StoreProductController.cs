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
    [ProducesResponseType(typeof(IEnumerable<StoreProductInfoDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var products = await _productService.GetAll();
        return Ok(products);
    }
    
    [HttpGet("{upc}")]
    [VerifyRole(EmployeeRoles.Manager, EmployeeRoles.Cashier)]
    [ProducesResponseType(typeof(StoreProductInfoDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByUpc([FromRoute] string upc)
    {
        var dto = await _productService.GetProductInfoByUpc(upc);
        return Ok(dto);
    }
    
    [HttpGet("filter")]
    [Authorize]
    [ProducesResponseType(typeof(IEnumerable<StoreProductInfoDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> FilterProducts([FromQuery] StoreProductSearchCriteria searchCriteria)
    {
        var products = await _productService.Filter(searchCriteria);
        return Ok(products);
    }
    
    [HttpGet("promotional")]
    [Authorize]
    [ProducesResponseType(typeof(IEnumerable<StoreProductInfoDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> FilterPromotionalProducts([FromQuery] StoreProductSearchCriteria searchCriteria)
    {
        var products = await _productService.GetFilteredPromotional(searchCriteria);
        return Ok(products);
    }
    
    [HttpGet("regular")]
    [Authorize]
    [ProducesResponseType(typeof(IEnumerable<StoreProductInfoDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> FilterRegularProducts([FromQuery] StoreProductSearchCriteria searchCriteria)
    {
        var products = await _productService.GetFilteredRegular(searchCriteria);
        return Ok(products);
    }
    
    [HttpPost]
    [VerifyRole(EmployeeRoles.Manager)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] SaveStoreProductDto dto)
    {
        var id = await _productService.CreateStoreProduct(dto);
        if (string.IsNullOrEmpty(id)) return BadRequest();
        return Created();
    }

    [HttpPut]
    [VerifyRole(EmployeeRoles.Manager)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Update([FromBody] SaveStoreProductDto dto)
    {
        var id = await _productService.UpdateStoreProduct(dto);
        if (string.IsNullOrEmpty(id)) return BadRequest();
        return NoContent();
    }

    [HttpDelete("{upc}")]
    [VerifyRole(EmployeeRoles.Manager)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(string upc)
    {
        var deleted = await _productService.DeleteStoreProduct(upc);
        if (!deleted) return BadRequest();
        return NoContent();
    }
}
