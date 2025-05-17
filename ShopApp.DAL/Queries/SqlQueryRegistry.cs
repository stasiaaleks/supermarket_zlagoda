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
    private readonly Dictionary<string, string> _queryMap = new();

    public SqlQueryRegistry() => LoadEmbeddedSqlResources();
    
    public string Get(string key)
    {
        if (!_queryMap.TryGetValue(key, out var sql))
            throw new KeyNotFoundException($"SQL query not found for key: '{key}'");

        return sql;
    }
    
    private void LoadEmbeddedSqlResources()
    {
        var assembly = Assembly.GetEntryAssembly()!;
        var resources = assembly.GetManifestResourceNames();

        foreach (var resource in resources.Where(r => r.EndsWith(".sql", StringComparison.OrdinalIgnoreCase)))
        {
            using var stream = assembly.GetManifestResourceStream(resource);
            using var reader = new StreamReader(stream!);
            string query = reader.ReadToEnd();

            var key = NormaliseKey(resource, assembly);
            _queryMap[key] = query;
        }
    }
    
    private static string NormaliseKey(string resource, Assembly assembly)
    {
        // TODO: make this method more readable
        var prefix = assembly.GetName().Name + ".";
        var trimmed = resource.StartsWith(prefix) ? resource[prefix.Length..] : resource;
        trimmed = trimmed[(trimmed.IndexOf("sql.", StringComparison.OrdinalIgnoreCase) + 4)..]; // after "sql." directory
        trimmed = trimmed[..^4]; // strip  ".sql"
        return trimmed.Replace('.', '/');
    }
}
