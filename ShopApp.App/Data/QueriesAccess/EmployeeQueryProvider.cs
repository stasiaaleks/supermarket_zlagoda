namespace ShopApp.Data.QueriesAccess;

public class EmployeeQueryProvider: IQueryByNamespaceProvider
{
    private const string FolderName = "Employee";

    private const string GetByIdQuery = "get_by_id";
    private const string GetByUsernameQuery = "get_by_username";
    private const string GetRoleByIdQuery = "get_role_by_id";
    private const string GetSeqNextValQuery = "get_seq_nextval";
    private const string GetAllSortedBySurnameQuery = "get_all_sorted_by_surname";
    private const string GetAllQuery = "get_all";
    private const string GetAllCashiersSortedBySurnameQuery = "get_cashiers_sorted";
    private const string GetContactsBySurnameQuery = "get_contact_by_surname";
    
    private const string CreateSingleQuery = "create_single";
    private const string UpdateByIdQuery = "update_by_id";
    private const string DeleteByIdQuery = "delete_by_id";
    
    public string GetById => GetNamespace(GetByIdQuery);
    public string GetByUsername => GetNamespace(GetByUsernameQuery);
    public string GetRoleById => GetNamespace(GetRoleByIdQuery);
    public string GetAllSortedBySurname => GetNamespace(GetAllSortedBySurnameQuery);
    public string GetAll => GetNamespace(GetAllQuery);
    public string GetAllCashiersSortedBySurname => GetNamespace(GetAllCashiersSortedBySurnameQuery);
    public string GetContactsBySurname => GetNamespace(GetContactsBySurnameQuery);
    public string GetSeqNextVal => GetNamespace(GetSeqNextValQuery);
    
    public string CreateSingle => GetNamespace(CreateSingleQuery);
    public string DeleteById => GetNamespace(DeleteByIdQuery);
    public string UpdateById => GetNamespace(UpdateByIdQuery);
    
    public string GetNamespace(string fileName)
    {
        return $"{FolderName}.{fileName}.sql";
    }
    
}