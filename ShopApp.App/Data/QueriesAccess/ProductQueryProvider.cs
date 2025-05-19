namespace ShopApp.Data.QueriesAccess;

public class ProductQueryProvider: IQueryByNamespaceProvider
{
    private const string FolderName = "Product";
    private const string GetAll = "get_all";
    private const string GetById = "get_by_id";
    
    public string GetAllPath => GetNamespace(GetAll);
    public string GetByIdPath => GetNamespace(GetById);

    // get a unique namespace to get query content by filename
    public string GetNamespace(string fileName)
    {
        return $"{FolderName}.{fileName}.sql";
    }
}

