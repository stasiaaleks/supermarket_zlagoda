namespace ShopApp.Data.QueriesAccess;

public class CategoryQueryProvider: IQueryByNamespaceProvider
{
    private const string FolderName = "Category";
    private const string GetAllQuery = "get_all";
    private const string GetByNumQuery = "get_by_num";
    
    private const string CreateSingleQuery = "create_single";
    private const string DeleteByNumQuery = "delete_by_num";
    private const string UpdateByNumQuery = "update_by_num";

    public string GetAll=> GetNamespace(GetAllQuery);
    public string GetByNum => GetNamespace(GetByNumQuery);
    public string CreateSingle => GetNamespace(CreateSingleQuery);
    public string DeleteByNum => GetNamespace(DeleteByNumQuery);
    public string UpdateByNum => GetNamespace(UpdateByNumQuery);
    
    public string GetNamespace(string fileName)
    {
        return $"{FolderName}.{fileName}.sql";
    }
}