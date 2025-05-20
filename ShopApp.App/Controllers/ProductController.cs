using Microsoft.AspNetCore.Mvc;
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
    [VerifyRole(EmployeeRoles.Manager)]
    public async Task<IActionResult> GetAllProducts()
    {
        var products = await _productService.GetAllProductsAsync();
        return Ok(products);
    }
}
