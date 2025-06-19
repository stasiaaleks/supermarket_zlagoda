using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopApp.Data.DTO;
using ShopApp.Data.Enums;
using ShopApp.Data.SearchCriteria;
using ShopApp.Services;
using ShopApp.Services.Auth;

namespace ShopApp.Controllers;

[ApiController]
[Route("api/products")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    [VerifyRole(EmployeeRoles.Manager, EmployeeRoles.Cashier)]
    [ProducesResponseType(typeof(IEnumerable<ProductWithCategoryDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllProducts()
    {
        var products = await _productService.GetAll();
        return Ok(products);
    }
    
    [HttpGet("{id}")]
    [Authorize]
    [ProducesResponseType(typeof(ProductWithCategoryDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProductById([FromRoute] string id)
    {
        var products = await _productService.GetById(id);
        return Ok(products);
    }
    
    [HttpGet("filter")]
    [Authorize]
    [ProducesResponseType(typeof(IEnumerable<ProductWithCategoryDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> FilterProducts([FromQuery] ProductSearchCriteria searchCriteria)
    {
        var products = await _productService.Filter(searchCriteria);
        return Ok(products);
    }
    [HttpPost]
    [VerifyRole(EmployeeRoles.Manager)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateProductDto dto)
    {
        var id = await _productService.CreateProduct(dto);
        if (string.IsNullOrEmpty(id)) return BadRequest();
        return Created();
    }

    [HttpPut]
    [VerifyRole(EmployeeRoles.Manager)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Update([FromBody] ProductDto dto)
    {
        var id = await _productService.UpdateById(dto);
        if (string.IsNullOrEmpty(id)) return BadRequest();
        return NoContent();
    }

    [HttpDelete("{id}")]
    [VerifyRole(EmployeeRoles.Manager)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(string id)
    {
        var deleted = await _productService.DeleteById(id);
        if (!deleted) return BadRequest();
        return NoContent();
    }
}
