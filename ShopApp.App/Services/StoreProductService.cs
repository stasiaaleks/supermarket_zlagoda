using AutoMapper;
using ShopApp.DAL.Repository;
using ShopApp.Data.DTO;
using ShopApp.Data.QueriesAccess;

namespace ShopApp.Services;

public interface IStoreProductService
{
    Task<IEnumerable<StoreProductDto>> GetAll();
    Task<StoreProductPriceNumberDto> GetPriceAndNumberByUpc(string upc);
    Task<StoreProductDto> GetProductInfoByUpc(string upc);
}

public class StoreProductService : IStoreProductService
{
    private readonly IRepository<StoreProduct> _productRepo;
    private readonly StoreProductQueryProvider _queryProvider;
    private readonly IMapper _mapper;

    public StoreProductService(IRepository<StoreProduct> productRepo, StoreProductQueryProvider queryProvider, IMapper mapper)
    {
        _productRepo = productRepo;
        _queryProvider = queryProvider;
        _mapper = mapper;
    }
    
    public async Task<IEnumerable<StoreProductDto>> GetAll()
    {
        var products = await _productRepo.GetAllAsync(_queryProvider.GetAll);
        return _mapper.Map<IEnumerable<StoreProductDto>>(products);
    }

    public async Task<StoreProductPriceNumberDto> GetPriceAndNumberByUpc(string upc)
    {
        var product = await _productRepo.GetSingleAsync(_queryProvider.GetByUpc, new { UPC = upc });
        return _mapper.Map<StoreProductPriceNumberDto>(product);
    }

    public async Task<StoreProductDto> GetProductInfoByUpc(string upc)
    {
        var product = await _productRepo.GetSingleAsync<StoreProductDto>(_queryProvider.GetProductInfoByUpc, new { UPC = upc });
        return product;
    }
}