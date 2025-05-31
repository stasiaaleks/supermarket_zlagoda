namespace ShopApp.Data.QueriesAccess;

public class StoreProductQueryProvider: IQueryByNamespaceProvider
{
    private const string FolderName = "StoreProduct";
    private const string GetAllQuery = "get_all";
    private const string GetByUpcQuery = "get_by_upc";
    private const string GetProductInfoByUpcQuery = "get_product_info_by_upc";
    
    public string GetAll => GetNamespace(GetAllQuery);
    public string GetByUpc => GetNamespace(GetByUpcQuery);
    public string GetProductInfoByUpc => GetNamespace(GetProductInfoByUpcQuery);

    public string GetNamespace(string fileName)
    {
        return $"{FolderName}.{fileName}.sql";
    }
}

