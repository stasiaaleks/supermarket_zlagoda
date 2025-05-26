using AutoMapper;
using ShopApp.DAL.Repository;
using ShopApp.Data.DTO;
using ShopApp.Data.QueriesAccess;

namespace ShopApp.Services;

public interface ICheckService
{
    Task<IEnumerable<CheckDto>> GetAll();
    Task<IEnumerable<CheckWithSalesListDto>> GetAllWithSalesByPeriod(DateTime start, DateTime end);
}

public class CheckService : ICheckService
{
    private readonly IRepository<CheckDto> _checkRepo;
    private readonly IRepository<CheckWithSaleDto> _checkWithSalesRepo;
    private readonly CheckQueryProvider _queryProvider;
    private readonly IMapper _mapper;

    public CheckService(CheckQueryProvider queryProvider, IMapper mapper, IRepository<CheckDto> checkRepo, IRepository<CheckWithSaleDto> checkWithSalesRepo)
    {
        _queryProvider = queryProvider;
        _mapper = mapper;
        _checkRepo = checkRepo;
        _checkWithSalesRepo = checkWithSalesRepo;
    }
    
    public async Task<IEnumerable<CheckDto>> GetAll()
    {
        var products = await _checkRepo.GetAllAsync(_queryProvider.GetAll);
        return _mapper.Map<IEnumerable<CheckDto>>(products);
    }

    public async Task<IEnumerable<CheckWithSalesListDto>> GetAllWithSalesByPeriod(DateTime start, DateTime end)
    {
        var query = _queryProvider.GetAllWithSalesByPeriod;
        var checks = await _checkWithSalesRepo.GetAllAsync(query, new { StartDate = start, EndDate = end });
        return _mapper.Map<IEnumerable<CheckWithSalesListDto>>(checks);
    }
}