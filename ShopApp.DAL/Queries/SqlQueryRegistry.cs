namespace ShopApp.DAL.Queries;

using System.Reflection;
using System;
using System.Collections.Generic;
using System.IO;

public interface IReadonlyRegistry
{
    string Load(string queryKey);
}

public class SqlQueryRegistry: IReadonlyRegistry
{
    private readonly Dictionary<string, string> _queryCache = new(StringComparer.OrdinalIgnoreCase);
    private readonly Assembly _assembly;
    private readonly string[] _resources;


    public SqlQueryRegistry()
    {
        _assembly = Assembly.GetEntryAssembly()!;
        _resources = _assembly.GetManifestResourceNames();
    }
    
    public string Load(string queryKey)
    {
        if (string.IsNullOrWhiteSpace(queryKey))
            throw new ArgumentException("Query key must be provided", nameof(queryKey));

        if (!_queryCache.TryGetValue(queryKey, out var sql))
        {
            sql = LoadQueryFromEmbeddedResource(queryKey);
            _queryCache[queryKey] = sql;
        }
    
        return sql;
    }

    private string LoadQueryFromEmbeddedResource(string queryKey)
    {
        var queryResource = _resources.FirstOrDefault(r => r.IndexOf(queryKey, StringComparison.OrdinalIgnoreCase) >= 0);
        if (queryResource == null) throw new KeyNotFoundException($"Embedded SQL resource '{queryKey}' not found");

        try
        {
            using var stream = _assembly.GetManifestResourceStream(queryResource);
            if (stream == null)
                throw new InvalidOperationException($"Failed to load resource stream for '{queryResource}'");

            using var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }
        catch (Exception e)
        {
            throw new InvalidOperationException($"Error reading embedded resource '{queryResource}'", e);
        }
    }
}
