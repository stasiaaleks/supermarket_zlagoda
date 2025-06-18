using Microsoft.AspNetCore.Mvc;
using ShopApp.Data.DTO;
using ShopApp.Data.Enums;
using ShopApp.Data.SearchCriteria;
using ShopApp.Services;
using ShopApp.Services.Auth;

namespace ShopApp.Controllers;

[ApiController]
[Route("api/cards")]
public class CustomerCardController : ControllerBase
{
    private readonly ICustomerCardService _cardService;

    public CustomerCardController(ICustomerCardService checkService)
    {
        _cardService = checkService;
    }

    [HttpGet]
    [VerifyRole(EmployeeRoles.Manager, EmployeeRoles.Cashier)]
    [ProducesResponseType(typeof(IEnumerable<CustomerCardDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllCustomerCards()
    {
        var cards = await _cardService.GetAll();
        return Ok(cards);
    }
    
    [HttpGet("filter")]
    [VerifyRole(EmployeeRoles.Manager, EmployeeRoles.Cashier)]
    [ProducesResponseType(typeof(IEnumerable<CustomerCardDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> FilterCategories([FromQuery] CustomerCardSearchCriteria searchCriteria)
    {
        var cards = await _cardService.Filter(searchCriteria);
        return Ok(cards);
    }

    [HttpPost]
    [VerifyRole(EmployeeRoles.Manager, EmployeeRoles.Cashier)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CustomerCardDto dto)
    {
        var number = await _cardService.CreateCustomerCard(dto);
        if (number == null) return BadRequest();
        return Created();
    }
    
    [HttpDelete("{number}")]
    [VerifyRole(EmployeeRoles.Manager)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteByNum(string number)
    {
        await _cardService.DeleteByNum(number);
        return NoContent();
    }

    [HttpPut]
    [VerifyRole(EmployeeRoles.Manager, EmployeeRoles.Cashier)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdateByNum(string number, [FromBody] CustomerCardDto dto)
    {
        await _cardService.UpdateByNum(dto);
        return NoContent();
    }
}
