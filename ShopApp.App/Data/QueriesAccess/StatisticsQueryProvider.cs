namespace ShopApp.Data.QueriesAccess;

public class StatisticsQueryProvider: IQueryByNamespaceProvider
{
    private const string FolderName = "Statistics";
    private const string GetPromProductsDataByCashierQuery = "cashier_prom_products_sum_quantity";
    private const string GetSameChecksAsCashierBySurnameQuery = "same_checks_as_cashier_by_surname";
    
    public string GetPromProductsDataByCashier => GetNamespace(GetPromProductsDataByCashierQuery);
    public string GetSameChecksAsCashierBySurname => GetNamespace(GetSameChecksAsCashierBySurnameQuery);
    
    
    public string GetNamespace(string fileName)
    {
        return $"{FolderName}.{fileName}.sql";
    }
}