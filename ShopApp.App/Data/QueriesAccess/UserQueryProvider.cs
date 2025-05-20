namespace ShopApp.Data.QueriesAccess;

public class UserQueryProvider: IQueryByNamespaceProvider
{
    private const string FolderName = "User";

    private const string GetByUsernameQuery = "get_by_username";

    public string GetByUsername => GetNamespace(GetByUsernameQuery);

    
    public string GetNamespace(string fileName)
    {
        return $"{FolderName}.{fileName}.sql";
    }
    
}