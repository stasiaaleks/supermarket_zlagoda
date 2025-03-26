using Dapper;
using ShopApp.Data;
using ShopApp.Data.Repositories;

namespace ShopApp.CLI.Migrations;

public class MigrationRunner
{
    private readonly IConnectionProvider _dbConnectionProvider;

    public MigrationRunner(IConnectionProvider dbConnectionProvider)
    {
        _dbConnectionProvider = dbConnectionProvider;
    }
    
    public async Task Run(string queryFilePath)
    {
        string sql = SqlQueryLoader.LoadFromFile(queryFilePath);
        using var connection = await _dbConnectionProvider.Connect();
        await connection.QueryAsync(sql);
    }
}