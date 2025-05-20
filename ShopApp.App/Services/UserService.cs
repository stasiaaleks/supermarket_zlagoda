using ShopApp.DAL.Queries;
using ShopApp.DAL.Repository;
using ShopApp.Data.Entities;
using ShopApp.Data.QueriesAccess;

namespace ShopApp.Services;


public interface IUserService
{
    Task<User> GetByUsername(string username);
}

public class UserService: IUserService
{
    private readonly IReadonlyRegistry _sqlQueryRegistry;
    private readonly UserQueryProvider _queryProvider;
    private readonly IRepository<User> _userRepo;

    public UserService(IRepository<User> userRepo, IReadonlyRegistry sqlQueryRegistry, UserQueryProvider queryProvider)
    {
        _userRepo = userRepo;
        _sqlQueryRegistry = sqlQueryRegistry;
        _queryProvider = queryProvider;
    }

    public async Task<User> GetByUsername(string username)
    {
        var query = _sqlQueryRegistry.Load(_queryProvider.GetByUsername); 
        var user = await _userRepo.GetSingleAsync(query, new { Username = username }); 
        
        if (user == null)
            throw new KeyNotFoundException($"User with username '{username}' not found.");

        return user;
    }
}