using System.Data;
using ShopApp.DAL.DbConnection;
using ShopApp.DAL.Queries;
using ShopApp.DAL.Repository;
using ShopApp.Data.Entities;

namespace ShopApp.Repositories;

public interface IUserRepository
{
    Task<User?> GetSingleAsync(string queryFilePath, object? parameters = null);
    Task<IEnumerable<User>> GetListAsync(string queryFilePath, object? parameters = null);
    Task<User?> CreateAsync(User entity, string queryFilePath);
    Task<User?> UpdateAsync(User entity, string queryFilePath);
    Task<User?> DeleteAsync(User entity, string queryFilePath);
}


public class UserRepository : Repository<User>, IUserRepository
{
    private readonly IReadonlyRegistry _sqlQueryRegistry;
    
    public UserRepository(IConnectionProvider dbConnectionProvider, IReadonlyRegistry sqlQueryRegistry)
        : base(dbConnectionProvider)
    {
        _sqlQueryRegistry = sqlQueryRegistry;
    }

    protected override User Map(IDataRecord record)
    {
        return new User
        {
            UserId = record.GetInt32(record.GetOrdinal("user_id")),
            Username = record.GetString(record.GetOrdinal("username")),
            PasswordHash = record.GetString(record.GetOrdinal("password_hash")),
            PasswordSalt = record.GetString(record.GetOrdinal("password_salt")),
            IdEmployee = record.GetString(record.GetOrdinal("id_employee"))
        };
    }

    public Task<User?> GetSingleAsync(string queryFilePath, object? parameters = null)
    {
        var query = _sqlQueryRegistry.Load(queryFilePath); 
        return QuerySingleAsync(query, parameters);
    }

    public Task<IEnumerable<User>> GetListAsync(string queryFilePath, object? parameters = null)
    {
        var query = _sqlQueryRegistry.Load(queryFilePath); 
        return QueryListAsync(query);
    }

    public Task<User?> CreateAsync(User entity, string queryFilePath)
    {
        var query = _sqlQueryRegistry.Load(queryFilePath);
        var parameters = new
        {
            Username = entity.Username,
            PasswordHash = entity.PasswordHash,
            PasswordSalt = entity.PasswordSalt,
            IdEmployee = entity.IdEmployee
        };

        return ExecuteAsync(query, parameters);
    }

    public Task<User?> UpdateAsync(User entity, string queryFilePath)
    {
        var query = _sqlQueryRegistry.Load(queryFilePath);
        var parameters = new
        {
            UserId = entity.UserId,
            Username = entity.Username,
            PasswordHash = entity.PasswordHash,
            PasswordSalt = entity.PasswordSalt,
            IdEmployee = entity.IdEmployee
        };

        return ExecuteAsync(query, parameters);
    }

    public Task<User?> DeleteAsync(User entity, string queryFilePath)
    {
        var query = _sqlQueryRegistry.Load(queryFilePath);
        var parameters = new { userid = entity.UserId };
        return ExecuteAsync(query, parameters);
    }
    
    
}