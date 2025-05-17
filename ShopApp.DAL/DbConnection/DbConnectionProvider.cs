using System.Data;
using Npgsql;

namespace ShopApp.DAL.DbConnection;

public interface IConnectionProvider
{
    Task<IDbConnection> Connect();
}


public class DbConnectionProvider : IConnectionProvider
{
    private readonly string _connectionString;

    public DbConnectionProvider(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<IDbConnection> Connect()
    {
        var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();
        return connection;
    }
}