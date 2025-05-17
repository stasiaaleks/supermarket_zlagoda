using Dapper;
using ShopApp.DAL.DbConnection;

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
        string sql = LoadFromFile(queryFilePath);
        using var connection = await _dbConnectionProvider.Connect();
        await connection.QueryAsync(sql);
    }
    
    private string LoadFromFile(string filePath)
    {
        if (!File.Exists(filePath)) throw new FileNotFoundException($"SQL query not found: {filePath}");
        return File.ReadAllText(filePath);
    }
}