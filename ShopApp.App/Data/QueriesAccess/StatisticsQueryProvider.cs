namespace ShopApp.Data.QueriesAccess;

public class StatisticsQueryProvider: IQueryByNamespaceProvider
{
    private const string FolderName = "Statistics";
    private const string GetPromProductsDataByCashierQuery = "cashier_prom_products_sum_quantity";
    private const string GetSameChecksAsCashierBySurnameQuery = "same_checks_as_cashier_by_surname";
    private const string GetCashiersMinProductsMinChecksQuery = "cashiers_min_products_min_checks";
    private const string GetProductsSoldOnlyPromoQuery = "get_products_sold_only_with_promo";
    private const string GetCashiersProductsCertainCategoryQuery = "cashiers_without_checks_without_category";
    private const string GetCustomersWithNumCategoriesQuery = "cust_with_more_than_x_categories_bought";

    public string GetPromProductsDataByCashier => GetNamespace(GetPromProductsDataByCashierQuery);
    public string GetSameChecksAsCashierBySurname => GetNamespace(GetSameChecksAsCashierBySurnameQuery);
    public string GetCashiersMinProductsMinChecks => GetNamespace(GetCashiersMinProductsMinChecksQuery);
    public string GetProductsSoldOnlyPromo => GetNamespace(GetProductsSoldOnlyPromoQuery);
    public string GetCashiersProductsCertainCategory => GetNamespace(GetCashiersProductsCertainCategoryQuery);
    public string GetCustomersWithNumCategories => GetNamespace(GetCustomersWithNumCategoriesQuery);


    
    public string GetNamespace(string fileName)
    {
        return $"{FolderName}.{fileName}.sql";
    }
}