using ShopApp.Data.Entities;
    
namespace ShopApp.Services.Auth;


public interface IAuthService
{
    Task<User?> Authenticate(string username, string password);
    Task<bool> Register(string username, string password);
}


public class AuthService: IAuthService
{
    private readonly IUserService _userService;
    private readonly IPasswordHasher _passwordHasher;
    
    public AuthService(IUserService userService, IPasswordHasher passwordHasher)
    {
        _userService = userService;
        _passwordHasher = passwordHasher;
    }

    public async Task<User?> Authenticate(string username, string password)
    {
        var user = await _userService.GetByUsername(username);
        if (user == null)
            return null;

        var isValid = _passwordHasher.VerifyPassword(password, user.PasswordHash, user.PasswordSalt);
        return isValid ? user : null;
    }

    public async Task<bool> Register(string username, string password)
    {
        // TBI
        return true;
    }
}