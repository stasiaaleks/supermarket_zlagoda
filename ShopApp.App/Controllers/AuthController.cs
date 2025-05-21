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

        await LoginUser(loginDto);
        return Ok();
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        var createdEntities = await _authService.Register(dto);
        if (!createdEntities) return BadRequest();
        
        await LoginUser(dto); 
        return Ok();
    }
    
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return Ok(new { message = "User logged out successfully." });
    }


    private async Task LoginUser(ILoginDto dto)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, dto.Username), // sets user.Identity.Name
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
    }
        
}