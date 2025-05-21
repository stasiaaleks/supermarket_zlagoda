namespace ShopApp.Data.QueriesAccess;

public class UserQueryProvider: IQueryByNamespaceProvider
{
    private const string FolderName = "User";

    private const string GetByUsernameQuery = "get_by_username";
    private const string CreateSingleQuery = "create_single";

    public string GetByUsername => GetNamespace(GetByUsernameQuery);
    public string CreateSingle => GetNamespace(CreateSingleQuery);

    
    public string GetNamespace(string fileName)
    {
        return $"{FolderName}.{fileName}.sql";
    }
    
}