using System.Transactions;
using AutoMapper;
using ShopApp.Data.DTO;
using ShopApp.Data.Entities;
    
namespace ShopApp.Services.Auth;


public interface IAuthService
{
    Task<User?> Authenticate(string username, string password);
    Task<User> RegisterUserWithEmployee(RegisterDto dto);
    Task<User> RegisterForExistingEmployee(CreateUserDto dto);
    Task<bool> ChangePassword(string username);
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

    // TODO: consider moving to UserService
    public async Task<User> RegisterUserWithEmployee(RegisterDto dto)
    {
        var employeeDto = _mapper.Map<CreateEmployeeDto>(dto);
        int? newUserId;
        
        using (var transaction = new TransactionScope())
        {
            var newEmployeeId = await _employeeService.CreateEmployee(employeeDto);
            newUserId = await _userService.CreateUser(dto.Username, dto.Password, newEmployeeId);
            transaction.Complete();
        }
        
        return await _userService.GetById(newUserId.Value);
    }
    
    public async Task<User> RegisterForExistingEmployee(CreateUserDto dto)
    {
        var exists = (await _employeeService.GetById(dto.IdEmployee)) != null;
        if (!exists) throw new KeyNotFoundException($"Employee with id {dto.IdEmployee} not found");
        
        var newUserId = await _userService.CreateUser(dto.Username, dto.Password, dto.IdEmployee);
        return await _userService.GetById(newUserId);
    }

    public Task<bool> ChangePassword(string username)
    {
        throw new NotImplementedException();
    }
}