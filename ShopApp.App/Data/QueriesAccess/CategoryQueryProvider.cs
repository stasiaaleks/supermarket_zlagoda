namespace ShopApp.Data.QueriesAccess;

public class CategoryQueryProvider: IQueryByNamespaceProvider
{
    private const string FolderName = "Category";
    private const string GetAll = "get_all";
    private const string GetById = "get_by_id";

    public string GetAllPath => GetNamespace(GetAll);
    public string GetByIdPath => GetNamespace(GetById);
    
    public string GetNamespace(string fileName)
    {
        return $"{FolderName}.{fileName}.sql";
    }
}

