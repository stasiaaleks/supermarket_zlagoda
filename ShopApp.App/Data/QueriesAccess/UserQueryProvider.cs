namespace ShopApp.Data.QueriesAccess;

public class UserQueryProvider: IQueryByNamespaceProvider
{
    private const string FolderName = "User";

    private const string GetByUsernameQuery = "get_by_username";
    private const string GetByIdQuery = "get_by_id";
    private const string CreateSingleQuery = "create_single";
    private const string DeleteByIdQuery = "delete_by_id";

    public string GetByUsername => GetNamespace(GetByUsernameQuery);
    public string GetById => GetNamespace(GetByIdQuery);
    public string CreateSingle => GetNamespace(CreateSingleQuery);
    public string DeleteById => GetNamespace(DeleteByIdQuery);

    
    public string GetNamespace(string fileName)
    {
        return $"{FolderName}.{fileName}.sql";
    }
    
}