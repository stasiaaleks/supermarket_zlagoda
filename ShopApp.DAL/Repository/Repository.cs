using System.Dynamic;
using System.Reflection;
using Dapper;
using ShopApp.DAL.DbConnection;
using ShopApp.DAL.Queries;

namespace ShopApp.DAL.Repository;
public interface IRepository<T> where T : class
{
    Task<T?> GetByIdAsync(string queryPath, string id);
    Task<T?> GetSingleAsync(string queryPath, object? parameters = null);
    Task<IEnumerable<T>> GetAllAsync(string queryPath);
    Task<TResult> InsertAsync<TResult>(T entity, string queryPath, object? parameters = null);
    Task<TResult> UpdateAsync<TResult>(T entity, string queryPath, object? parameters = null);
    Task<int> DeleteAsync(T entity, string queryPath, object? parameters = null);
    Task<TResult> ExecuteScalarAsync<TResult>(string queryPath, object? parameters = null);
}

public class Repository<T> : IRepository<T> where T : class
{
    private readonly IConnectionProvider _dbConnectionProvider;
    private readonly IReadonlyRegistry _sqlQueryRegistry;

    public Repository(IConnectionProvider dbConnectionProvider, IReadonlyRegistry sqlQueryRegistry)
    {
        _dbConnectionProvider = dbConnectionProvider;
        _sqlQueryRegistry = sqlQueryRegistry;
    }
    
    public async Task<T?> GetByIdAsync(string queryPath, string id)
    {
        using var connection = await _dbConnectionProvider.Connect();
        var query = _sqlQueryRegistry.Load(queryPath);
        
        object param;
        if (int.TryParse(id, out var intId))
            param = intId;
        else
            param = id;
        
        return await connection.QuerySingleOrDefaultAsync<T>(query, new { Id = param });
    }

    public async Task<IEnumerable<T>> GetAllAsync(string queryPath)
    {
        using var connection = await _dbConnectionProvider.Connect();
        var query = _sqlQueryRegistry.Load(queryPath); 
        return await connection.QueryAsync<T>(query);
    }

    public async Task<T?> GetSingleAsync(string queryPath, object? parameters = null)
    {
        using var connection = await _dbConnectionProvider.Connect();
        var query = _sqlQueryRegistry.Load(queryPath); 
        return await connection.QuerySingleOrDefaultAsync<T>(query, parameters);
    }
    
    public async Task<TResult> InsertAsync<TResult>(T entity, string queryPath, object? parameters = null)
    {
        using var connection = await _dbConnectionProvider.Connect();
        var query = _sqlQueryRegistry.Load(queryPath);
        
        var totalQueryParameters = MergeEntityParameters(parameters, entity);
        return await connection.ExecuteScalarAsync<TResult>(query, totalQueryParameters);
    }

    public async Task<TResult> UpdateAsync<TResult>(T entity, string queryPath, object? parameters = null)
    {
        using var connection = await _dbConnectionProvider.Connect();
        var query = _sqlQueryRegistry.Load(queryPath);

        var totalQueryParameters = MergeEntityParameters(parameters, entity);
        return await connection.ExecuteScalarAsync<TResult>(query, totalQueryParameters);
    }

    public async Task<int> DeleteAsync(T entity, string queryPath, object? parameters = null)
    {
        using var connection = await _dbConnectionProvider.Connect();
        var query = _sqlQueryRegistry.Load(queryPath);

        var totalQueryParameters = MergeEntityParameters(parameters, entity);
        return await connection.ExecuteAsync(query, totalQueryParameters);
    }

    public async Task<TResult> ExecuteScalarAsync<TResult>(string queryPath, object? parameters = null)
    {
        using var connection = await _dbConnectionProvider.Connect();
        var query = _sqlQueryRegistry.Load(queryPath);
        return await connection.ExecuteScalarAsync<TResult>(query, parameters);
    }
    
    private object MergeEntityParameters(object? parameters, T entity)
    {
        var resultObj = new ExpandoObject() as IDictionary<string, object?>;
        if (parameters != null)
        {
            foreach (var prop in parameters.GetType().GetProperties())
            {
                resultObj[prop.Name] = prop.GetValue(parameters);
            }
        }
        
        foreach (var prop in typeof(T).GetProperties())
        {
            var value = prop.GetValue(entity);
            resultObj.TryAdd(prop.Name, value);
        }

        return resultObj;
    }
}
