using Npgsql;
using System.Data;
using System.Reflection;
using ShopApp.DAL.DbConnection;

namespace ShopApp.DAL.Repository;


public interface IRepository<T> where T : class
{
    Task<IEnumerable<T>> QueryListAsync(string sql, object? parameters = null);
    Task<T?> ExecuteAsync(string sql, object? parameters = null);
    Task<T?> QuerySingleAsync(string sql, object? parameters = null);
}

public abstract class Repository<T>: IRepository<T> where T : class
{
    private readonly IConnectionProvider _dbConnectionProvider;

    protected Repository(IConnectionProvider dbConnectionProvider)
    {
        _dbConnectionProvider = dbConnectionProvider;
    }

    public async Task<IEnumerable<T>> QueryListAsync(string sql, object? parameters = null)
    {
        var results = await QueryAsync(sql, parameters);
        return results;
    }

    public async Task<T?> ExecuteAsync(string sql, object? parameters = null)
    {
        using var connection = await _dbConnectionProvider.Connect();
        await using var command = new NpgsqlCommand(sql, (NpgsqlConnection)connection);

        if (parameters != null)
            command.Parameters.AddRange(ToNpgsqlParameters(parameters));

        var result = await command.ExecuteScalarAsync();
        if (result == DBNull.Value) return default(T);
        return (T?)result;
    }
    
    public async Task<T?> QuerySingleAsync(string sql, object? parameters = null)
    {
        var results = await QueryAsync(sql, parameters);
        return results[0];
    }

    protected abstract T Map(IDataRecord record);
    
    // protected abstract object Map(T entity);

    private NpgsqlParameter[] ToNpgsqlParameters(object obj)
    {
        return obj.GetType()
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Select(p => new NpgsqlParameter("@" + p.Name, p.GetValue(obj) ?? DBNull.Value))
            .ToArray();
    }

    private async Task<IList<T>> QueryAsync(string sql, object? parameters = null)
    {
        var results = new List<T>();

        using var connection = await _dbConnectionProvider.Connect();
        await using var command = new NpgsqlCommand(sql, (NpgsqlConnection)connection);

        if (parameters != null)
            command.Parameters.AddRange(ToNpgsqlParameters(parameters));

        await using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            results.Add(Map(reader));
        }

        return results;
    }
}