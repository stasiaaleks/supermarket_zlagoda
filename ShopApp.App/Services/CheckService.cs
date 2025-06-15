
using System.Transactions;
using AutoMapper;
using ShopApp.DAL.Repository;
using ShopApp.Data.DTO;
using ShopApp.Data.Entities;
using ShopApp.Data.QueriesAccess;

namespace ShopApp.Services;

public interface ICheckService
{
    Task<IEnumerable<CheckDto>> GetAll();
    Task<IEnumerable<CheckWithSalesListDto>> GetAllWithSalesByPeriod(DateTime? start, DateTime? end);
    Task<IEnumerable<CheckWithSalesListDto>> GetAllWithSalesByPeriodAndCashier(DateTime? start, DateTime? end, string cashierId);
    Task<CheckSumDto> GetSumByPeriod(DateTime? start, DateTime? end);
    Task<CheckSumDto> GetSumByEmployeeAndPeriod(DateTime? start, DateTime? end, string cashierId);
    Task<CheckWithSalesListDto> GetByNumberWithSales(string number);
    Task<string?> CreateCheckWithSales(CreateCheckWithSalesListDto dto);
}

public class CheckService : ICheckService
{
    private readonly IRepository<Check> _checkRepo;
    private readonly CheckQueryProvider _queryProvider;
    private readonly ISaleService _saleService;
    private readonly IStoreProductService _storeProductService;
    private readonly IMapper _mapper;

    public CheckService(CheckQueryProvider queryProvider, IMapper mapper, IRepository<Check> checkRepo, ISaleService saleService, IStoreProductService storeProductService)
    {
        _queryProvider = queryProvider;
        _mapper = mapper;
        _checkRepo = checkRepo;
        _saleService = saleService;
        _storeProductService = storeProductService;
    }

    public async Task<IEnumerable<CheckDto>> GetAll()
    {
        return await _checkRepo.GetAllAsync<CheckDto>(_queryProvider.GetAll);
    }

    public async Task<IEnumerable<CheckWithSalesListDto>> GetAllWithSalesByPeriod(DateTime? start, DateTime? end)
    {
        var (validStart, validEnd) = ValidateNullDatetimeValues(start, end);
        var query = _queryProvider.GetAllWithSalesByPeriod;
        var checks = await _checkRepo.GetAllAsync<CheckWithSaleDto>(query, new { StartDate = validStart, EndDate = validEnd });
        return _mapper.Map<IEnumerable<CheckWithSalesListDto>>(checks);
    }

    public async Task<IEnumerable<CheckWithSalesListDto>> GetAllWithSalesByPeriodAndCashier(DateTime? start, DateTime? end, string cashierId)
    {
        var (validStart, validEnd) = ValidateNullDatetimeValues(start, end);
        var query = _queryProvider.GetAllWithSalesByPeriodAndEmployee;
        var parameters = new { StartDate = validStart, EndDate = validEnd, EmployeeId = cashierId };
        var checks = await _checkRepo.GetAllAsync<CheckWithSaleDto>(query, parameters);
        return _mapper.Map<IEnumerable<CheckWithSalesListDto>>(checks);
    }

    public async Task<CheckSumDto> GetSumByPeriod(DateTime? start, DateTime? end)
    {
        var (validStart, validEnd) = ValidateNullDatetimeValues(start, end);
        var query = _queryProvider.GetSumByPeriod;
        var parameters = new { StartDate = validStart, EndDate = validEnd };
        return await _checkRepo.GetSingleAsync<CheckSumDto>(query, parameters);
    }

    public async Task<CheckSumDto> GetSumByEmployeeAndPeriod(DateTime? start, DateTime? end, string cashierId)
    {
        var (validStart, validEnd) = ValidateNullDatetimeValues(start, end);
        var query = _queryProvider.GetSumByPeriodAndEmployee;
        var parameters = new { StartDate = validStart, EndDate = validEnd, EmployeeId = cashierId };
        return await _checkRepo.GetSingleAsync<CheckSumDto>(query, parameters);
    }

    public async Task<CheckWithSalesListDto> GetByNumberWithSales(string number)
    {
        var query = _queryProvider.GetByNum;
        var parameters = new { CheckNumber = number };
        var checks = await _checkRepo.GetAllAsync<CheckWithSaleDto>(query, parameters);
        return _mapper.Map<IEnumerable<CheckWithSalesListDto>>(checks).First();
    }

    public async Task<string?> CreateCheckWithSales(CreateCheckWithSalesListDto dto)
    {
        string? createdEntityId;
        
        using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            var checkNum = await _checkRepo.GetNextPrefixedStringId("CHK", _queryProvider.GetSeqNextVal);
            var checkToCreate = new Check()
            {
                CheckNumber = checkNum,
                CardNumber = dto.CardNumber,
                IdEmployee = dto.IdEmployee,
                PrintDate = dto.PrintDate,
                SumTotal = dto.SumTotal, // calculated on client
                VAT = dto.VAT // calculated on client
            };
            createdEntityId = await _checkRepo.InsertAsync<string>(checkToCreate, _queryProvider.CreateSingle);
            if (createdEntityId == null) throw new ArgumentException("Failed to create a check.");
            
            foreach (var s in dto.Sales)
            {
                var sale = _mapper.Map<SaleDto>(s);
                sale.CheckNumber = checkNum;
                await _saleService.CreateSale(sale);
                await _storeProductService.UpdateAmount(-s.ProductNumber, s.UPC);
            }
            
            transaction.Complete();
        }
        
        return createdEntityId;
    }
    
    private (DateTime start, DateTime end) ValidateNullDatetimeValues(DateTime? start, DateTime? end)
    {
        var validStart = start ?? DateTime.MinValue;
        var validEnd = end ?? DateTime.UtcNow.Date.AddDays(1);
        return (validStart, validEnd);
    }
}
