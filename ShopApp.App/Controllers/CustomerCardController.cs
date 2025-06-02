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
    [VerifyRole(EmployeeRoles.Manager)]
    [ProducesResponseType(typeof(IEnumerable<CustomerCardDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllCustomerCards()
    {
        var cards = await _cardService.GetAll();
        return Ok(cards);
    }
    
    [HttpGet("filter")]
    [VerifyRole(EmployeeRoles.Manager)]
    [ProducesResponseType(typeof(IEnumerable<CustomerCardDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> FilterCategories([FromQuery] CustomerCardSearchCriteria searchCriteria)
    {
        var cards = await _cardService.Filter(searchCriteria);
        return Ok(cards);
    }
}
