namespace ShopApp.Data.Repositories;

public static class SqlQueryLoader
{
    private static readonly Dictionary<string, string> QueryCache = new(); // idea for queries cache

    public static string LoadFromFile(string filePath)
    {
        if (QueryCache.TryGetValue(filePath, out var value)) return value;
        if (!File.Exists(filePath)) throw new FileNotFoundException($"SQL query not found: {filePath}");

        QueryCache[filePath] = File.ReadAllText(filePath);
        
        return QueryCache[filePath];
    }
}

