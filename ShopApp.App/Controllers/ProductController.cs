using Microsoft.AspNetCore.Mvc;
using ShopApp.Data.DTO;
using ShopApp.Data.Enums;
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
    [ProducesResponseType(typeof(IEnumerable<ProductDto>), StatusCodes.Status200OK)]
    //[ProducesResponseType(StatusCodes.Status401Unauthorized)] - TODO: discuss if needed
    //[ProducesResponseType(StatusCodes.Status403Forbidden)] - TODO:  discuss if needed
    public async Task<IActionResult> GetAllProducts()
    {
        var products = await _productService.GetAll();
        return Ok(products);
    }
    
    [HttpGet("{id}")]
    [VerifyRole(EmployeeRoles.Manager, EmployeeRoles.Cashier)]
    [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProductById([FromRoute] string id)
    {
        var products = await _productService.GetById(id);
        return Ok(products);
    }
    
    
}
