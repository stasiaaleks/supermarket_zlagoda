using System.Linq.Expressions;
using Dapper;
using ShopApp.DAL.DbConnection;

namespace ShopApp.DAL.Repository;
public interface IRepository<T> where T : class
{
    Task<T?> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync(string query);
    Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> predicate);
    Task<int> AddAsync(T entity, object parameters, string query);
    Task AddRangeAsync(IEnumerable<T> entities);
    Task<int> UpdateAsync(T entity, object parameters, string query);
    Task<int> UpdateRangeAsync(IEnumerable<T> entities);
    Task<int> DeleteAsync(T entity, object parameters, string query);
    void DeleteRangeAsync(IEnumerable<T> entities);
}

public class Repository<T> : IRepository<T> where T : class
{
    private readonly IConnectionProvider _dbConnectionProvider;

    public Repository(IConnectionProvider dbConnectionProvider)
    {
        _dbConnectionProvider = dbConnectionProvider;
    }
    
    public Task<T?> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<T>> GetAllAsync(string query)
    {
        using var connection = await _dbConnectionProvider.Connect();
        return await connection.QueryAsync<T>(query);
    }

    public Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> predicate)
    {
        throw new NotImplementedException();
    }

    public async Task<int> AddAsync(T entity, object parameters, string query)
    {
        using var connection = await _dbConnectionProvider.Connect();
        return await connection.ExecuteScalarAsync<int>(query, parameters);
    }

    public Task AddRangeAsync(IEnumerable<T> entities)
    {
        throw new NotImplementedException();
    }

    public async Task<int> UpdateAsync(T entity, object parameters, string query)
    {
        using var connection = await _dbConnectionProvider.Connect();
        await connection.ExecuteAsync(query, parameters);
        return 0; // return id later
    }

    public Task<int> UpdateRangeAsync(IEnumerable<T> entities)
    {
        throw new NotImplementedException();
    }

    public async Task<int> DeleteAsync(T entity, object parameters, string query)
    {
        using var connection = await _dbConnectionProvider.Connect();
        await connection.ExecuteAsync(query, parameters);
        return 0; // return id later
    }

    public void DeleteRangeAsync(IEnumerable<T> entities)
    {
        throw new NotImplementedException();
    }
}
