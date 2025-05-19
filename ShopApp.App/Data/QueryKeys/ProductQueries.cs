namespace ShopApp.Data.QueryKeys;

public static class ProductQueries
{
    private const string FolderName = "Product";
    private const string GetAll = "get_all";
    private const string GetById = "get_by_id";
    
    public static string GetAllPath => GetNamespace(GetAll);
    public static string GetByIdPath => GetNamespace(GetById);

    // get a unique namespace to get query content by filename
    private static string GetNamespace(string fileName)
    {
        return $"{FolderName}.{fileName}.sql";
    }
}

