using AutoMapper;
using ShopApp.Data.DTO;
using ShopApp.Data.Entities;
    
namespace ShopApp.Services.Auth;


public interface IAuthService
{
    Task<User?> Authenticate(string username, string password);
    Task<User> Register(RegisterDto dto);
}


public class AuthService: IAuthService
{
    private readonly IUserService _userService;
    private readonly IEmployeeService _employeeService;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IMapper _mapper;
    
    public AuthService(IUserService userService, IPasswordHasher passwordHasher, IMapper mapper, IEmployeeService employeeService)
    {
        _userService = userService;
        _passwordHasher = passwordHasher;
        _mapper = mapper;
        _employeeService = employeeService;
    }

    public async Task<User?> Authenticate(string username, string password)
    {
        var user = await _userService.GetByUsername(username);
        if (user == null)
            return null;

        var isValid = _passwordHasher.VerifyPassword(password, user.PasswordHash, user.PasswordSalt);
        return isValid ? user : null;
    }

    public async Task<User> Register(RegisterDto dto)
    {
        var employeeDto = _mapper.Map<CreateEmployeeDto>(dto);
        var newEmployeeId = await _employeeService.CreateEmployee(employeeDto);
        var newUserId = await _userService.CreateUser(dto.Username, dto.Password, newEmployeeId);
        return await _userService.GetById(newUserId);
    }
}