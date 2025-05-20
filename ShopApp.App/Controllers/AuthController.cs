using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using ShopApp.Data.DTO;
using ShopApp.Services.Auth;

namespace ShopApp.Controllers;

[ApiController]
public class AuthController: ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        var user = await _authService.Authenticate(loginDto.Username, loginDto.Password);

        if (user == null) return Unauthorized();

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, loginDto.Username), // sets user.Identity.Name
            new Claim("EmployeeId", user.IdEmployee), // optional
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        
        return Ok();
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] LoginDto loginDto)
    {
       // TBI
        return Ok();
    }
        
}