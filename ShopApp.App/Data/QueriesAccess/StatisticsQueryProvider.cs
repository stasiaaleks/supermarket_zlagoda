namespace ShopApp.Data.QueriesAccess;

public class StatisticsQueryProvider: IQueryByNamespaceProvider
{
    private const string FolderName = "Statistics";
    private const string GetPromProductsDataByCashierQuery = "cashier_prom_products_sum_quantity";
    private const string GetSameChecksAsCashierBySurnameQuery = "same_checks_as_cashier_by_surname";
    private const string GetCashiersMinProductsMinChecksQuery = "cashiers_min_products_min_checks";
    private const string GetProductsSoldOnlyPromoQuery = "get_products_sold_only_with_promo";
    
    public string GetPromProductsDataByCashier => GetNamespace(GetPromProductsDataByCashierQuery);
    public string GetSameChecksAsCashierBySurname => GetNamespace(GetSameChecksAsCashierBySurnameQuery);
    public string GetCashiersMinProductsMinChecks => GetNamespace(GetCashiersMinProductsMinChecksQuery);
    public string GetProductsSoldOnlyPromo => GetNamespace(GetProductsSoldOnlyPromoQuery);
    
    public string GetNamespace(string fileName)
    {
        return $"{FolderName}.{fileName}.sql";
    }
}