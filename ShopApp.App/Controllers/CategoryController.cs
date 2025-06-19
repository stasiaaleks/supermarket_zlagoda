using Microsoft.AspNetCore.Mvc;
using ShopApp.Data.DTO;
using ShopApp.Data.Enums;
using ShopApp.Data.SearchCriteria;
using ShopApp.Services;
using ShopApp.Services.Auth;

namespace ShopApp.Controllers;

[ApiController]
[Route("api/categories")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    [VerifyRole(EmployeeRoles.Manager, EmployeeRoles.Cashier)]
    [ProducesResponseType(typeof(IEnumerable<CategoryDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllCategories()
    {
        var categories = await _categoryService.GetAll();
        return Ok(categories);
    }
    
    [HttpGet("{number}")]
    [VerifyRole(EmployeeRoles.Manager)]
    [ProducesResponseType(typeof(IEnumerable<CategoryDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByNum(int number)
    {
        var categories = await _categoryService.GetByNum(number);
        return Ok(categories);
    }
    
    [HttpPost]
    [VerifyRole(EmployeeRoles.Manager)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateCategoryDto dto)
    {
        var number = await _categoryService.CreateCategory(dto);
        if (number == null) return BadRequest();
        return Created();
    }
    
    [HttpDelete("{number}")]
    [VerifyRole(EmployeeRoles.Manager)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteByNum(string number)
    {
        await _categoryService.DeleteByNum(number);
        return NoContent();
    }

    [HttpPut]
    [VerifyRole(EmployeeRoles.Manager)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdateByNum(int number, [FromBody] CategoryDto dto)
    {
        await _categoryService.UpdateByNum(dto);
        return NoContent();
    }

    
    
    [HttpGet("filter")]
    [VerifyRole(EmployeeRoles.Manager)]
    [ProducesResponseType(typeof(IEnumerable<CategoryDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> FilterCategories([FromQuery] CategorySearchCriteria searchCriteria)
    {
        var categories = await _categoryService.Filter(searchCriteria);
        return Ok(categories);
    }
}
