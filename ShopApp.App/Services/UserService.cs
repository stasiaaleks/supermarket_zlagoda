using ShopApp.Data.Entities;
using ShopApp.Data.QueriesAccess;
using ShopApp.Repositories;

namespace ShopApp.Services;


public interface IUserService
{
    Task<User> GetByUsername(string username);
}

public class UserService: IUserService
{
    private readonly UserQueryProvider _queryProvider;
    private readonly IUserRepository _userRepo;

    public UserService(IUserRepository userRepo, UserQueryProvider queryProvider)
    {
        _userRepo = userRepo;
        _queryProvider = queryProvider;
    }

    public async Task<User> GetByUsername(string username)
    {
        var user = await _userRepo.GetSingleAsync<User>(_queryProvider.GetByUsername, new { Username = username }); 
        if (user == null)
            throw new KeyNotFoundException($"User with username '{username}' not found.");

        return user;
    }
}