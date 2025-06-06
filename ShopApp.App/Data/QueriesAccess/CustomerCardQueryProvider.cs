namespace ShopApp.Data.QueriesAccess;

public class CustomerCardQueryProvider: IQueryByNamespaceProvider
{
    private const string FolderName = "CustomerCard";
    private const string GetAllQuery = "get_all";
    private const string GetByIdQuery = "get_by_id";
    
    private const string CreateSingleQuery = "create_single";
    private const string DeleteByCardNumQuery = "delete_by_card_num";
    private const string UpdateByCardNumQuery = "update_by_card_num";

    public string GetAll => GetNamespace(GetAllQuery);
    public string GetById => GetNamespace(GetByIdQuery);
    public string CreateSingle => GetNamespace(CreateSingleQuery);
    public string DeleteByCardNum => GetNamespace(DeleteByCardNumQuery);
    public string UpdateByCardNum => GetNamespace(UpdateByCardNumQuery);
    
    public string GetNamespace(string fileName)
    {
        return $"{FolderName}.{fileName}.sql";
    }
}

