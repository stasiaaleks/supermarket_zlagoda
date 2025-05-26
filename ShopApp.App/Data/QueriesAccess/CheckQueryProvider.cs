namespace ShopApp.Data.QueriesAccess;

public class CheckQueryProvider: IQueryByNamespaceProvider
{
    private const string FolderName = "Check";
    private const string GetAllQuery = "get_all";
    private const string GetAllWithSalesByPeriodQuery = "get_all_with_sales_by_period";
    private const string GetAllWithSalesByPeriodAndEmployeeQuery = "get_all_by_empl_and_period_with_sales";

    public string GetAll => GetNamespace(GetAllQuery);
    public string GetAllWithSalesByPeriod => GetNamespace(GetAllWithSalesByPeriodQuery);
    public string GetAllWithSalesByPeriodAndEmployee => GetNamespace(GetAllWithSalesByPeriodAndEmployeeQuery);
    
    public string GetNamespace(string fileName)
    {
        return $"{FolderName}.{fileName}.sql";
    }
}

