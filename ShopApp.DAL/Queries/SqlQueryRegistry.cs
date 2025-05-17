namespace ShopApp.DAL.Queries;

using System.Reflection;
using System;
using System.Collections.Generic;
using System.IO;

public interface ISqlQueryRegistry
{
    string Get(string queryKey);
}

public class SqlQueryRegistry: ISqlQueryRegistry
{
    private readonly Dictionary<string, string> _queryMap = new(); // SqlQueryRegistry is a singleton

    public SqlQueryRegistry() => LoadEmbeddedSqlResources();
    
    public string Get(string key)
    {
        if (!_queryMap.TryGetValue(key, out var sql))
            throw new KeyNotFoundException($"SQL query not found for key: '{key}'");

        return sql;
    }
    
    private void LoadEmbeddedSqlResources()
    {
        //var assembly = typeof(SqlQueryRegistry).Assembly;
        //var assembly = Assembly.GetExecutingAssembly(); // or typeof(SqlQueryRegistry).Assembly
       // var assembly = Assembly.GetEntryAssembly(); // works
        var assembly = Assembly.GetEntryAssembly()!;   // Program is in ShopApp.API

        var resources = assembly.GetManifestResourceNames();

        foreach (var resource in resources.Where(r => r.EndsWith(".sql", StringComparison.OrdinalIgnoreCase)))
        {
            using var stream = assembly.GetManifestResourceStream(resource);
            using var reader = new StreamReader(stream!);
            string query = reader.ReadToEnd();

            // Normalize key: Sql.Products.GetAll -> Products/GetAll
            var key = resource
                .Replace($"{assembly.GetName().Name}.Sql.", "") // Sql.Products.GetAll.sql
                .Replace(".sql", "")
                .Replace('.', '/');

            _queryMap[key] = query;
        }
    }
}
