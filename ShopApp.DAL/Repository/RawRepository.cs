using Npgsql;
using System.Data;
using System.Reflection;
using ShopApp.DAL.DbConnection;

namespace ShopApp.DAL.Repository;


public interface IRawRepository<T> where T : class
{
    Task<IEnumerable<T>> QueryListAsync(string sql, object? parameters = null);
    Task<int> ExecuteAsync(string sql, object? parameters = null);
    Task<TK?> QuerySingleAsync<TK>(string sql, object? parameters = null);
}

public abstract class RawRepository<T> where T : class, new()
{
    private readonly IConnectionProvider _dbConnectionProvider;

    protected RawRepository(IConnectionProvider dbConnectionProvider)
    {
        _dbConnectionProvider = dbConnectionProvider;
    }

    public async Task<IEnumerable<T>> QueryAsync(string sql, object? parameters = null)
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

    public async Task<int> ExecuteAsync(string sql, object? parameters = null)
    {
        using var connection = await _dbConnectionProvider.Connect();
        await using var command = new NpgsqlCommand(sql, (NpgsqlConnection)connection);

        if (parameters != null)
            command.Parameters.AddRange(ToNpgsqlParameters(parameters));

        return await command.ExecuteNonQueryAsync();
    }
    
    public async Task<TK?> QuerySingleValueAsync<TK>(string sql, object? parameters = null)
    {
        using var connection = await _dbConnectionProvider.Connect();
        await using var command = new NpgsqlCommand(sql, (NpgsqlConnection)connection);

        if (parameters != null)
            command.Parameters.AddRange(ToNpgsqlParameters(parameters));

        var result = await command.ExecuteScalarAsync();
        if (result == null || result == DBNull.Value)
            return default;

        return (TK)Convert.ChangeType(result, typeof(T));
    }

    protected abstract T Map(IDataRecord record);

    private NpgsqlParameter[] ToNpgsqlParameters(object obj)
    {
        return obj.GetType()
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Select(p => new NpgsqlParameter("@" + p.Name, p.GetValue(obj) ?? DBNull.Value))
            .ToArray();
    }
}