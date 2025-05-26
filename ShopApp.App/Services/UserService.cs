using ShopApp.DAL.Repository;
using ShopApp.Data.Entities;
using ShopApp.Data.QueriesAccess;
using ShopApp.Services.Auth;

namespace ShopApp.Services;


public interface IUserService
{
    Task<User> GetById(int id);
    Task<User> GetByUsername(string username);
    Task<int> CreateUser(string username, string password, string idEmployee);
}

public class UserService: IUserService
{
    private readonly UserQueryProvider _queryProvider;
    private readonly IRepository<User> _userRepo;
    private readonly IPasswordHasher _passwordHasher;

    public UserService(IRepository<User> userRepo, UserQueryProvider queryProvider, IPasswordHasher passwordHasher)
    {
        _userRepo = userRepo;
        _queryProvider = queryProvider;
        _passwordHasher = passwordHasher;
    }

    public async Task<User> GetById(int id)
    {
        var user = await _userRepo.GetByIdAsync(_queryProvider.GetById, id.ToString()); 
        if (user == null)
            throw new KeyNotFoundException($"User with id '{id}' not found.");

        return user;
    }

    public async Task<User> GetByUsername(string username)
    {
        var user = await _userRepo.GetSingleAsync(_queryProvider.GetByUsername, new { Username = username }); 
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
   
        var createdEntityId = await _userRepo.InsertAsync<string>(userToCreate,  _queryProvider.CreateSingle);
        var newUser = await _userRepo.GetByIdAsync( _queryProvider.GetById, createdEntityId);
        if (newUser == null) throw new NullReferenceException($"Failed to create an employee {username}");

        return newUser.UserId;
    }
}