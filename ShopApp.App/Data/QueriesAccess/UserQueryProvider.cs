namespace ShopApp.Data.QueriesAccess;

public class UserQueryProvider: IQueryByNamespaceProvider
{
    private const string FolderName = "User";

    private const string GetByUsernameQuery = "get_by_username";
    private const string GetByIdQuery = "get_by_id";
    private const string GetByEmployeeIdQuery = "get_by_employee_id";
    private const string CreateSingleQuery = "create_single";
    private const string DeleteByIdQuery = "delete_by_id";
    private const string UpdateByIdQuery = "update_by_id";

    public string GetByUsername => GetNamespace(GetByUsernameQuery);
    public string GetById => GetNamespace(GetByIdQuery);
    public string GetByEmployeeId => GetNamespace(GetByEmployeeIdQuery);
    public string CreateSingle => GetNamespace(CreateSingleQuery);
    public string DeleteById => GetNamespace(DeleteByIdQuery);
    public string UpdateById => GetNamespace(UpdateByIdQuery);

    
    public string GetNamespace(string fileName)
    {
        return $"{FolderName}.{fileName}.sql";
    }
    
}