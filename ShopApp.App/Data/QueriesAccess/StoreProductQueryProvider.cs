namespace ShopApp.Data.QueriesAccess;

public class StoreProductQueryProvider: IQueryByNamespaceProvider
{
    private const string FolderName = "StoreProduct";
    private const string GetAllQuery = "get_all"; 
    private const string GetByUpcQuery = "get_by_upc";  
    private const string GetProductInfoByUpcQuery = "get_product_info_by_upc";  
    private const string GetAllPromotionalQuery = "get_all_promotional"; 
    private const string GetAllPromotionalByUpcQuery = "get_promotional_by_regular_upc"; 
    private const string GetAllRegularQuery = "get_all_regular"; 
    private const string CreateSingleQuery = "create_single";
    private const string UpdateByUpcQuery = "update_by_upc";
    private const string DeleteByUpcQuery = "delete_by_upc";
    
    
    public string GetAll => GetNamespace(GetAllQuery);
    public string GetByUpc => GetNamespace(GetByUpcQuery);
    public string GetProductInfoByUpc => GetNamespace(GetProductInfoByUpcQuery);
    public string GetAllPromotional => GetNamespace(GetAllPromotionalQuery);
    public string GetAllPromotionalByUpc => GetNamespace(GetAllPromotionalByUpcQuery);
    public string GetAllRegular => GetNamespace(GetAllRegularQuery);
    public string CreateSingle => GetNamespace(CreateSingleQuery);
    public string UpdateByUpc => GetNamespace(UpdateByUpcQuery);
    public string DeleteByUpc => GetNamespace(DeleteByUpcQuery);

    public string GetNamespace(string fileName)
    {
        return $"{FolderName}.{fileName}.sql";
    }
}

