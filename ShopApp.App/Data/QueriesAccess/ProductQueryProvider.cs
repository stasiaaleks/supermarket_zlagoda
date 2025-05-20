namespace ShopApp.Data.QueriesAccess;

public class ProductQueryProvider: IQueryByNamespaceProvider
{
    private const string FolderName = "Product";
    private const string GetAllQuery = "get_all";
    private const string GetByIdQuery = "get_by_id";
    
    public string GetAll => GetNamespace(GetAllQuery);
    public string GetById => GetNamespace(GetByIdQuery);

    public string GetNamespace(string fileName)
    {
        return $"{FolderName}.{fileName}.sql";
    }
}

