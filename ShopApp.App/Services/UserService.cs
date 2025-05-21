using ShopApp.Data.Entities;
using ShopApp.Data.QueriesAccess;
using ShopApp.Repositories;
using ShopApp.Services.Auth;

namespace ShopApp.Services;


public interface IUserService
{
    Task<User> GetByUsername(string username);
    Task<int> CreateUser(string username, string password, string idEmployee);
}

public class UserService: IUserService
{
    private readonly UserQueryProvider _queryProvider;
    private readonly IUserRepository _userRepo;
    private readonly IPasswordHasher _passwordHasher;

    public UserService(IUserRepository userRepo, UserQueryProvider queryProvider, IPasswordHasher passwordHasher)
    {
        _userRepo = userRepo;
        _queryProvider = queryProvider;
        _passwordHasher = passwordHasher;
    }

    public async Task<User> GetByUsername(string username)
    {
        var user = await _userRepo.GetSingleAsync<User>(_queryProvider.GetByUsername, new { Username = username }); 
        if (user == null)
            throw new KeyNotFoundException($"User with username '{username}' not found.");

        return user;
    }

    public async Task<int> CreateUser(string username, string password, string idEmployee)
    {
        var (hash, salt) = _passwordHasher.HashPassword(password);
        var userToCreate = new User
        {
            PasswordHash = hash,
            PasswordSalt = salt,
            IdEmployee = idEmployee,
            Username = username
        };
        var query = _queryProvider.CreateSingle;
        var createdEntity = await _userRepo.CreateAsync(userToCreate, query);
        return createdEntity.UserId;
    }
}