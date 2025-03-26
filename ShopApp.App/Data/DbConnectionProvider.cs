using System.Data;
using Npgsql;

namespace ShopApp.Data;

public interface IConnectionProvider
{
    Task<IDbConnection> Connect();
}


public class DbConnectionProvider : IConnectionProvider
{
    private readonly string _connectionString;

    public DbConnectionProvider(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DevDBConnection");
    }

    public async Task<IDbConnection> Connect()
    {
        var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();
        return connection;
    }
}