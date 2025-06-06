namespace ShopApp.Data.QueriesAccess;

public class ProductQueryProvider: IQueryByNamespaceProvider
{
    private const string FolderName = "Product";
    private const string GetAllQuery = "get_all";
    private const string GetByIdQuery = "get_by_id";
    
    private const string CreateSingleQuery = "create_single";
    private const string UpdateByIdQuery = "update_by_id";
    private const string DeleteByIdQuery = "delete_by_id";
    
    public string GetAll => GetNamespace(GetAllQuery);
    public string GetById => GetNamespace(GetByIdQuery);
    public string CreateSingle => GetNamespace(CreateSingleQuery);
    public string UpdateById => GetNamespace(UpdateByIdQuery);
    public string DeleteById => GetNamespace(DeleteByIdQuery);

    public string GetNamespace(string fileName)
    {
        return $"{FolderName}.{fileName}.sql";
    }
}

