using ShopApp.DAL.Repository;
using ShopApp.Data.DTO;
using ShopApp.Data.Entities;
using ShopApp.Data.QueriesAccess;
using ShopApp.Services.Auth;

namespace ShopApp.Services;


public interface IUserService
{
    Task<User> GetById(int id);
    Task<User> GetByUsername(string username);
    Task<User> GetByEmployeeId(string employeeId);
    Task<int> CreateUser(string username, string password, string idEmployee);
    Task<int> UpdateById(User user);
    Task<User?> UpdatePasswordByUsername(string username, string password);
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

    public async Task<User> GetByEmployeeId(string employeeId)
    {
        var user = await _userRepo.GetSingleAsync(_queryProvider.GetByEmployeeId, new { EmployeeId = employeeId }); 
        if (user == null)
            throw new KeyNotFoundException($"User with employee id '{employeeId}' not found.");

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
        if (newUser == null) throw new ArgumentException($"Failed to create an employee {username}");

        return newUser.UserId;
    }

    public async Task<int> UpdateById(User user)
    {
        var query = _queryProvider.UpdateById;
        return await _userRepo.UpdateAsync<int>(user, query);
    }
    
    
    public async Task<User?> UpdatePasswordByUsername(string username, string password)
    {
        var user = await GetByUsername(username);
        if (user == null)
            throw new KeyNotFoundException($"User with username '{username}' not found.");
        var (hash, salt) = _passwordHasher.HashPassword(password);
        user.PasswordHash = hash;
        user.PasswordSalt = salt;

        var id = await UpdateById(user);
        return id != null ? user : null;
    }
}