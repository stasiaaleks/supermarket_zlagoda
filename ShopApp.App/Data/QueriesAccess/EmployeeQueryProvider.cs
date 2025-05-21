namespace ShopApp.Data.QueriesAccess;

public class EmployeeQueryProvider: IQueryByNamespaceProvider
{
    private const string FolderName = "Employee";

    private const string GetByIdQuery = "get_by_id";
    private const string GetByUsernameQuery = "get_by_username";
    private const string GetRoleByIdQuery = "get_role_by_id";
    private const string CreateSingleQuery = "create_single";
    
    public string GetById => GetNamespace(GetByIdQuery);
    public string GetByUsername => GetNamespace(GetByUsernameQuery);
    public string GetRoleById => GetNamespace(GetRoleByIdQuery);
    public string CreateSingle => GetNamespace(CreateSingleQuery);
    
    public string GetNamespace(string fileName)
    {
        return $"{FolderName}.{fileName}.sql";
    }
    
}