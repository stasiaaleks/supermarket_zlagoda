namespace ShopApp.Data.QueriesAccess;

public class SaleQueryProvider: IQueryByNamespaceProvider
{
    private const string FolderName = "Sale";
    
    private const string GetAllQuery = "get_all";
    private const string GetProductTotalSoldByPeriodQuery = "get_product_sold_by_period";


    public string GetAll => GetNamespace(GetAllQuery);
    public string GetProductTotalSoldByPeriod => GetNamespace(GetProductTotalSoldByPeriodQuery);

    
    public string GetNamespace(string fileName)
    {
        return $"{FolderName}.{fileName}.sql";
    }
}

