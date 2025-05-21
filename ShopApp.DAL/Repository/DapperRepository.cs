using System.Linq.Expressions;
using Dapper;
using ShopApp.DAL.DbConnection;

namespace ShopApp.DAL.Repository;
public interface IDapperRepository<T> where T : class
{
    Task<T?> GetByIdAsync(string query, string id);
    Task<T?> GetSingleAsync(string query, object? parameters = null);
    Task<IEnumerable<T>> GetAllAsync(string query);
    Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> predicate);
    Task<int> AddAsync(T entity, string query, object? parameters = null);
    Task AddRangeAsync(IEnumerable<T> entities);
    Task<int> UpdateAsync(T entity, string query, object? parameters = null);
    Task<int> UpdateRangeAsync(IEnumerable<T> entities);
    Task<int> DeleteAsync(T entity, string query, object? parameters = null);
    void DeleteRangeAsync(IEnumerable<T> entities);
}

public class DapperRepository<T> : IDapperRepository<T> where T : class
{
    private readonly IConnectionProvider _dbConnectionProvider;

    public DapperRepository(IConnectionProvider dbConnectionProvider)
    {
        _dbConnectionProvider = dbConnectionProvider;
    }
    
    public async Task<T?> GetByIdAsync(string query, string id)
    {
        using var connection = await _dbConnectionProvider.Connect();
        return await connection.QuerySingleAsync<T>(query, new { Id = id });
    }

    public async Task<IEnumerable<T>> GetAllAsync(string query)
    {
        using var connection = await _dbConnectionProvider.Connect();
        return await connection.QueryAsync<T>(query);
    }

        public async Task<T?> GetSingleAsync(string query, object? parameters = null)
        {
            using var connection = await _dbConnectionProvider.Connect();
            return await connection.QuerySingleAsync<T>(query, parameters);
        }

    public Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> predicate)
    {
        throw new NotImplementedException();
    }

    public Task<int> AddAsync(T entity, string query, object? parameters = null)
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

    public async Task<int> UpdateAsync(T entity, string query, object? parameters = null)
    {
        using var connection = await _dbConnectionProvider.Connect();
        await connection.ExecuteAsync(query, parameters);
        return 0; // return id later
    }

    public Task<int> UpdateRangeAsync(IEnumerable<T> entities)
    {
        throw new NotImplementedException();
    }

    public async Task<int> DeleteAsync(T entity, string query, object? parameters = null)
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
