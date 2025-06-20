
using ShopApp.DAL.Repository;
using ShopApp.Data.DTO;
using ShopApp.Data.QueriesAccess;

namespace ShopApp.Services;


public interface IStatisticsService
{
    Task<CashierPromProductsNumericData> GetPromProductsSumQuantityByCashier(string idEmployee);
    Task<IEnumerable<CashierCheckData>> GetCashiersWithSameChecks(string surname);
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

    public async Task<CashierPromProductsNumericData> GetPromProductsSumQuantityByCashier(string idEmployee)
    {
        var parameters = new { IdEmployee = idEmployee };
        return await _repository.GetSingleAsync<CashierPromProductsNumericData>(_queryProvider.GetPromProductsDataByCashier, parameters);
    }

    public async Task<IEnumerable<CashierCheckData>> GetCashiersWithSameChecks(string surname)
    {
        var parameters = new { Surname = surname };
        return await _repository.GetAllAsync<CashierCheckData>(_queryProvider.GetSameChecksAsCashierBySurname, parameters);
    }
}