namespace ShopApp.Data.QueriesAccess;

public class CheckQueryProvider: IQueryByNamespaceProvider
{
    private const string FolderName = "Check";
    private const string GetAllQuery = "get_all";
    private const string GetAllWithSalesByPeriodQuery = "get_all_with_sales_by_period";
    private const string GetAllByPeriodAndEmployeeQuery = "get_by_employee_and_period";

    public string GetAll => GetNamespace(GetAllQuery);
    public string GetAllWithSalesByPeriod => GetNamespace(GetAllWithSalesByPeriodQuery);
    public string GetAllByPeriodAndEmployee => GetNamespace(GetAllByPeriodAndEmployeeQuery);
    
    public string GetNamespace(string fileName)
    {
        return $"{FolderName}.{fileName}.sql";
    }
}

