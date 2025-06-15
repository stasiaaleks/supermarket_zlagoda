using AutoMapper;
using ShopApp.DAL.Repository;
using ShopApp.Data.DTO;
using ShopApp.Data.Entities;
using ShopApp.Data.QueriesAccess;

namespace ShopApp.Services;

public interface ISaleService
{
    Task<IEnumerable<SaleDto>> GetAll();
    Task<string> CreateSale(SaleDto dto);

    Task<ProductsSoldDto> GetProductTotalSoldByPeriod(DateTime start, DateTime end, string productUPC);
}

public class SaleService : ISaleService
{
    private readonly IRepository<Sale> _saleRepo;
    private readonly SaleQueryProvider _queryProvider;
    private readonly IMapper _mapper;

    public SaleService(SaleQueryProvider queryProvider, IMapper mapper, IRepository<Sale> saleRepo)
    {
        _queryProvider = queryProvider;
        _mapper = mapper;
        _saleRepo = saleRepo;
    }

    public async Task<IEnumerable<SaleDto>> GetAll()
    {
        return await _saleRepo.GetAllAsync<SaleDto>(_queryProvider.GetAll);
    }
    public async Task<string> CreateSale(SaleDto dto)
    {
        var sale = _mapper.Map<Sale>(dto);
        var newUpc = await _saleRepo.InsertAsync<string>(sale, _queryProvider.CreateSingle);
        return newUpc;
    }

    public async Task<ProductsSoldDto> GetProductTotalSoldByPeriod(DateTime start, DateTime end, string productUPC)
    {
        var query = _queryProvider.GetProductTotalSoldByPeriod;
        var parameters = new { StartDate = start, EndDate = end, UPC = productUPC };
        return await _saleRepo.GetSingleAsync<ProductsSoldDto>(query, parameters);
    }
}