using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopApp.Data.DTO;
using ShopApp.Data.Entities;
using ShopApp.Services;
using ShopApp.Services.Auth;

namespace ShopApp.Controllers;

[ApiController]
public class AuthController: ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IEmployeeService _employeeService;

    public AuthController(IAuthService authService, IEmployeeService employeeService)
    {
        _authService = authService;
        _employeeService = employeeService;
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        var user = await _authService.Authenticate(loginDto.Username, loginDto.Password);
        if (user == null) return Unauthorized();

        await LoginUser(user);
        return Ok();
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> RegisterUserWithEmployee([FromBody] RegisterDto dto)
    {
        var createdUser = await _authService.CreateAccount(dto);
        if (createdUser == null) return BadRequest();
        
        await LoginUser(createdUser); 
        return Ok();
    }
    
    [HttpPost("register/existing-employee")]
    public async Task<IActionResult> RegisterUserForExistingEmployee([FromBody] CreateUserDto dto)
    {
        var createdUser = await _authService.RegisterForExistingEmployee(dto);
        if (createdUser == null) return BadRequest();
        return Ok();
    }
    
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return Ok(new { message = "User logged out successfully." });
    }
    
    [HttpPost("change-password")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> ChangePassword([FromBody] LoginDto dto)
    {
        var currentUsername = User.Identity?.Name;
    
        if (!VerifyPasswordOwnership(currentUsername, dto.Username))
            return StatusCode(StatusCodes.Status403Forbidden, new { error = "You can only change your own password" });
        
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        var updatedUser = await _authService.ChangePassword(dto);
        if (updatedUser == null) return BadRequest(new { message = "Failed changing password" });
    
        await LoginUser(updatedUser);
        return Ok(new { message = "Password changed successfully. User logged in" });
    }

    private bool VerifyPasswordOwnership(string? currentUsername, string incomingUsername)
    {
        return currentUsername != null && string.Equals(currentUsername, incomingUsername, StringComparison.OrdinalIgnoreCase);
    }


    private async Task LoginUser(IUser user)
    {
        var relatedEmployee = await _employeeService.GetById(user.IdEmployee);
        
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Username), // sets user.Identity.Name
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),  
            new Claim(ClaimTypes.Role, relatedEmployee.Role),
            new Claim("EmployeeId", relatedEmployee.IdEmployee),      
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
    }
        
}