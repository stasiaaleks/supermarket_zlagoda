using System.Data;
using ShopApp.DAL.DbConnection;
using ShopApp.DAL.Queries;
using ShopApp.DAL.Repository;
using ShopApp.Data.Entities;

namespace ShopApp.Repositories;

public interface IUserRepository
{
    Task<T?> GetSingleAsync<T>(string queryFilePath, object? parameters = null);
    Task<IEnumerable<User>> GetListAsync(string queryFilePath, object? parameters = null);
    Task<int> CreateAsync(User entity, string queryFilePath);
    Task<int> UpdateAsync(User entity, string queryFilePath);
    Task<int> DeleteAsync(User entity, string queryFilePath);
}


public class UserRepository : RawRepository<User>, IUserRepository
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
            UserId = record.GetInt32(record.GetOrdinal("id")),
            Username = record.GetString(record.GetOrdinal("username")),
            PasswordHash = record.GetString(record.GetOrdinal("password_hash")),
            PasswordSalt = record.GetString(record.GetOrdinal("password_salt")),
            IdEmployee = record.GetString(record.GetOrdinal("id_employee"))
        };
    }

    public Task<T?> GetSingleAsync<T>(string queryFilePath, object? parameters = null)
    {
        var query = _sqlQueryRegistry.Load(queryFilePath); 
        return QuerySingleValueAsync<T>(query, parameters);
    }

    public Task<IEnumerable<User>> GetListAsync(string queryFilePath, object? parameters = null)
    {
        var query = _sqlQueryRegistry.Load(queryFilePath); 
        return QueryAsync(query);
    }

    public Task<int> CreateAsync(User entity, string queryFilePath)
    {
        var query = _sqlQueryRegistry.Load(queryFilePath);
        var parameters = new
        {
            username = entity.Username,
            passwordhash = entity.PasswordHash,
            passwordsalt = entity.PasswordSalt,
            idemployee = entity.IdEmployee
        };

        return ExecuteAsync(query, parameters);
    }

    public Task<int> UpdateAsync(User entity, string queryFilePath)
    {
        var query = _sqlQueryRegistry.Load(queryFilePath);
        var parameters = new
        {
            userid = entity.UserId,
            username = entity.Username,
            passwordhash = entity.PasswordHash,
            passwordsalt = entity.PasswordSalt,
            idemployee = entity.IdEmployee
        };

        return ExecuteAsync(query, parameters);
    }

    public Task<int> DeleteAsync(User entity, string queryFilePath)
    {
        var query = _sqlQueryRegistry.Load(queryFilePath);
        var parameters = new { userid = entity.UserId };
        return ExecuteAsync(query, parameters);
    }
    
    
}