namespace ShopApp.Data.Repositories;

using System;
using System.Collections.Generic;
using System.IO;

public class SqlQueryRegistry
{
    public static readonly Dictionary<string, string> SqlQueriesMap = new();
    
    public static class SqlQueriesKeys
    {
        public const string GetAllProducts = "get_all_products";
        public const string GetProductById = "get_product_by_id";
    }
    
    public void FindAndMapAllQueries()
    {
        string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
        string[] sqlFiles = Directory.GetFiles(appDirectory, "*.sql", SearchOption.AllDirectories);

        foreach (var file in sqlFiles)
        {
            string queryName = Path.GetFileNameWithoutExtension(file);
            string queryText = File.ReadAllText(file);
            SqlQueriesMap[queryName] = queryText;
        }
    }
}
