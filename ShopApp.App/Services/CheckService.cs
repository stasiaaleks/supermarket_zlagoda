using AutoMapper;
using ShopApp.DAL.Repository;
using ShopApp.Data.DTO;
using ShopApp.Data.Entities;
using ShopApp.Data.QueriesAccess;

namespace ShopApp.Services;

public interface ICheckService
{
    Task<IEnumerable<CheckDto>> GetAll();
    Task<IEnumerable<CheckWithSalesListDto>> GetAllWithSalesByPeriod(DateTime start, DateTime end);
    Task<IEnumerable<CheckWithSalesListDto>> GetAllWithSalesByPeriodAndCashier(DateTime start, DateTime end, string cashierId);
}

public class CheckService : ICheckService
{
    private readonly IRepository<Check> _checkRepo;
    private readonly IRepository<CheckWithSaleDto> _checkWithSalesRepo;
    private readonly CheckQueryProvider _queryProvider;
    private readonly IMapper _mapper;

    public CheckService(CheckQueryProvider queryProvider, IMapper mapper, IRepository<Check> checkRepo, IRepository<CheckWithSaleDto> checkWithSalesRepo)
    {
        _queryProvider = queryProvider;
        _mapper = mapper;
        _checkRepo = checkRepo;
        _checkWithSalesRepo = checkWithSalesRepo;
    }
    
    public async Task<IEnumerable<CheckDto>> GetAll()
    {
        return await _checkRepo.GetAllAsync<CheckDto>(_queryProvider.GetAll);
    }

    public async Task<IEnumerable<CheckWithSalesListDto>> GetAllWithSalesByPeriod(DateTime start, DateTime end)
    {
        var query = _queryProvider.GetAllWithSalesByPeriod;
        var checks = await _checkWithSalesRepo.GetAllAsync(query, new { StartDate = start, EndDate = end });
        return _mapper.Map<IEnumerable<CheckWithSalesListDto>>(checks);
    }

    public async Task<IEnumerable<CheckWithSalesListDto>> GetAllWithSalesByPeriodAndCashier(DateTime start, DateTime end, string cashierId)
    {
        var query = _queryProvider.GetAllWithSalesByPeriodAndEmployee;
        var checks = await _checkRepo.GetAllAsync<CheckWithSaleDto>(
            query, new { StartDate = start, EndDate = end, EmployeeId = cashierId });
        return _mapper.Map<IEnumerable<CheckWithSalesListDto>>(checks);
    }
}