using System.Dynamic;
using Dapper;
using ShopApp.DAL.DbConnection;
using ShopApp.DAL.Queries;

namespace ShopApp.DAL.Repository;
public interface IRepository<T> where T : class
{
    Task<T?> GetByIdAsync(string queryPath, string id);
    Task<TResult> GetByIdAsync<TResult>(string queryPath, string id);
    Task<T?> GetSingleAsync(string queryPath, object? parameters = null);
    Task<TResult> GetSingleAsync<TResult>(string queryPath, object? parameters = null);
    Task<IEnumerable<T>> GetAllAsync(string queryPath, object? parameters = null);
    Task<IEnumerable<T>> FilterByPredicateAsync(string queryPath, ISearchCriteria criteria);
    Task<IEnumerable<TResult>> FilterByPredicateAsync<TResult>(string queryPath, ISearchCriteria criteria);
    Task<IEnumerable<TResult>> GetAllAsync<TResult>(string queryPath, object? parameters = null);
    Task<TResult> InsertAsync<TResult>(T entity, string queryPath, object? parameters = null);
    Task<TResult> UpdateAsync<TResult>(T entity, string queryPath, object? parameters = null);
    Task<int> DeleteAsync(T entity, string queryPath, object? parameters = null);
    Task<int> DeleteByIdAsync(string queryPath, string id);
    Task<TResult> ExecuteScalarAsync<TResult>(string queryPath, object? parameters = null);
    Task<string> GetNextPrefixedStringId(string prefix, string sequenceQueryPath);
}

public class Repository<T> : IRepository<T> where T : class
{
    private readonly IConnectionProvider _dbConnectionProvider;
    private readonly IReadonlyRegistry _sqlQueryRegistry;
    private readonly IQueryBuilder _queryBuilder;

    public Repository(IConnectionProvider dbConnectionProvider, IReadonlyRegistry sqlQueryRegistry, IQueryBuilder queryBuilder)
    {
        _dbConnectionProvider = dbConnectionProvider;
        _sqlQueryRegistry = sqlQueryRegistry;
        _queryBuilder = queryBuilder;
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

    public async Task<TResult> GetByIdAsync<TResult>(string queryPath, string id)
    {
        using var connection = await _dbConnectionProvider.Connect();
        var query = _sqlQueryRegistry.Load(queryPath);
        
        object param;
        if (int.TryParse(id, out var intId))
            param = intId;
        else
            param = id;
        
        return await connection.QuerySingleOrDefaultAsync<TResult>(query, new { Id = param });
    }

    public async Task<IEnumerable<T>> GetAllAsync(string queryPath, object? parameters = null)
    {
        using var connection = await _dbConnectionProvider.Connect();
        var query = _sqlQueryRegistry.Load(queryPath); 
        return await connection.QueryAsync<T>(query, parameters);
    }

    public async Task<IEnumerable<T>> FilterByPredicateAsync(string queryPath, ISearchCriteria criteria)
    {
        var baseQuery = _sqlQueryRegistry.Load(queryPath);
        var predicate = criteria.ToPredicate();
        
        var finalQuery = _queryBuilder.Build(baseQuery, criteria, predicate);
        
        using var connection = await _dbConnectionProvider.Connect();
        return await connection.QueryAsync<T>(finalQuery, predicate.Parameters);
    }
    
    public async Task<IEnumerable<TResult>> FilterByPredicateAsync<TResult>(string queryPath, ISearchCriteria criteria)
    {
        var baseQuery = _sqlQueryRegistry.Load(queryPath);
        var predicate = criteria.ToPredicate();
        
        var finalQuery = _queryBuilder.Build(baseQuery, criteria, predicate);
        
        using var connection = await _dbConnectionProvider.Connect();
        return await connection.QueryAsync<TResult>(finalQuery, predicate.Parameters);
    }

    public async Task<IEnumerable<TResult>> GetAllAsync<TResult>(string queryPath, object? parameters = null)
    {
        using var connection = await _dbConnectionProvider.Connect();
        var query = _sqlQueryRegistry.Load(queryPath); 
        return await connection.QueryAsync<TResult>(query, parameters);
    }

    public async Task<T?> GetSingleAsync(string queryPath, object? parameters = null)
    {
        using var connection = await _dbConnectionProvider.Connect();
        var query = _sqlQueryRegistry.Load(queryPath); 
        return await connection.QuerySingleOrDefaultAsync<T>(query, parameters);
    }
    
    public async Task<TResult> GetSingleAsync<TResult>(string queryPath, object? parameters = null)
    {
        using var connection = await _dbConnectionProvider.Connect();
        var query = _sqlQueryRegistry.Load(queryPath); 
        return await connection.QuerySingleOrDefaultAsync<TResult>(query, parameters);
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

    public async Task<int> DeleteByIdAsync(string queryPath, string id)
    {
        using var connection = await _dbConnectionProvider.Connect();
        var query = _sqlQueryRegistry.Load(queryPath);
        
        return await connection.ExecuteAsync(query, new { Id = id });
    }

    public async Task<TResult> ExecuteScalarAsync<TResult>(string queryPath, object? parameters = null)
    {
        using var connection = await _dbConnectionProvider.Connect();
        var query = _sqlQueryRegistry.Load(queryPath);
        return await connection.ExecuteScalarAsync<TResult>(query, parameters);
    }
    
    public async Task<string> GetNextPrefixedStringId(string prefix, string sequenceQueryPath)
    {
        var nextval = await ExecuteScalarAsync<string>(sequenceQueryPath);
        return $"{prefix}{nextval}";
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
