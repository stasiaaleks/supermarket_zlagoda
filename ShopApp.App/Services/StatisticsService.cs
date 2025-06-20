
using ShopApp.DAL.Repository;
using ShopApp.Data.DTO;
using ShopApp.Data.QueriesAccess;

namespace ShopApp.Services;


public interface IStatisticsService
{
    Task<IEnumerable<CashierPromProductsNumericData>> GetPromProductsSumQuantityByCashier(string idEmployee);
    Task<IEnumerable<CashierCheckData>> GetCashiersWithSameChecks(string surname);
    Task<IEnumerable<CashierChecksCountData>> GetCashiersMinProductsMinChecks(int minProducts, int minChecks);
    Task<IEnumerable<ProductsSoldOnlyPromo>> GetProductsSoldOnlyPromo();
    Task<IEnumerable<CashierProductsCertainCategory>> GetCashierProductsCertainCategory(string categoryName);
    Task<IEnumerable<CustomersWithNumCategories>> GetCustomersWithNumCategories(int categoryCount);

}

public class StatisticsService: IStatisticsService
{
    private readonly IRepository<Product> _repository;
    private readonly StatisticsQueryProvider _queryProvider;

    public StatisticsService(IRepository<Product> repository, StatisticsQueryProvider queryProvider)
    {
        _repository = repository;
        _queryProvider = queryProvider;
    }

    public async Task<IEnumerable<CashierPromProductsNumericData>> GetPromProductsSumQuantityByCashier(string idEmployee)
    {
        var parameters = new { IdEmployee = idEmployee };
        return await _repository.GetAllAsync<CashierPromProductsNumericData>(_queryProvider.GetPromProductsDataByCashier, parameters);
    }


    public async Task<IEnumerable<CashierCheckData>> GetCashiersWithSameChecks(string surname)
    {
        var parameters = new { Surname = surname };
        return await _repository.GetAllAsync<CashierCheckData>(_queryProvider.GetSameChecksAsCashierBySurname, parameters);
    }

    public async Task<IEnumerable<CashierChecksCountData>> GetCashiersMinProductsMinChecks(int minProducts, int minChecks)
    {
        var parameters = new { MinProducts = minProducts, MinChecks = minChecks };
        return await _repository.GetAllAsync<CashierChecksCountData>(_queryProvider.GetCashiersMinProductsMinChecks, parameters);
    }
    
    public async Task<IEnumerable<ProductsSoldOnlyPromo>> GetProductsSoldOnlyPromo()
    {
        return await _repository.GetAllAsync<ProductsSoldOnlyPromo>(_queryProvider.GetProductsSoldOnlyPromo);
    }
    public async Task<IEnumerable<CashierProductsCertainCategory>> GetCashierProductsCertainCategory(string categoryName)
    {
        var parameters = new { CategoryName = categoryName };
        return await _repository.GetAllAsync<CashierProductsCertainCategory>(_queryProvider.GetCashiersProductsCertainCategory, parameters);
    }
    
    public async Task<IEnumerable<CustomersWithNumCategories>> GetCustomersWithNumCategories(int categoryCount)
    {
        var parameters = new { CategoryCount = categoryCount };
        return await _repository.GetAllAsync<CustomersWithNumCategories>(_queryProvider.GetCustomersWithNumCategories, parameters);
    }  
}