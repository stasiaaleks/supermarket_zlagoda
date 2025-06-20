using AutoMapper;
using ShopApp.DAL.Repository;
using ShopApp.Data.DTO;
using ShopApp.Data.QueriesAccess;
using ShopApp.Data.SearchCriteria;

namespace ShopApp.Services;

public interface IStoreProductService
{
    Task<IEnumerable<StoreProductDto>> GetAll();
    Task<StoreProductPriceNumberDto> GetPriceAndNumberByUpc(string upc);
    Task<StoreProductDto> GetProductInfoByUpc(string upc);
    Task<IEnumerable<StoreProductDto>> Filter(StoreProductSearchCriteria criteria);
    Task<IEnumerable<StoreProductDto>> GetFilteredPromotional(StoreProductSearchCriteria criteria);
    Task<IEnumerable<StoreProductDto>> GetFilteredRegular(StoreProductSearchCriteria criteria);
    Task<string> UpdateAmount(int delta, string productUpc);
    Task<string> CreateStoreProduct(StoreProductDto dto);
    Task<string> UpdateStoreProduct(StoreProductDto dto);
    Task<bool> DeleteStoreProduct(string upc);
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
        var products = await _productRepo.GetAllAsync<StoreProductDto>(_queryProvider.GetAll);
        return products;
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
    
    public async Task<IEnumerable<StoreProductDto>> Filter(StoreProductSearchCriteria criteria)
    {
        var query = _queryProvider.GetAll;
        var products = await _productRepo.FilterByPredicateAsync<StoreProductDto>(query, criteria);
        return products;
    }

    public async Task<IEnumerable<StoreProductDto>> GetFilteredPromotional(StoreProductSearchCriteria criteria)
    {
        var query = _queryProvider.GetAllPromotional;
        var products = await _productRepo.FilterByPredicateAsync<StoreProductDto>(query, criteria);
        return products;
    }
    public async Task<string> UpdateAmount(int delta, string productUpc)
    {
        var product = await _productRepo.GetSingleAsync<StoreProduct>(_queryProvider.GetByUpc, new { UPC = productUpc });
        if (product == null) throw new ArgumentException($"Failed to find a product with UPC {productUpc}");
        
        var query = _queryProvider.UpdateByUpc;
        var prevAmount = product.ProductsNumber;
        var newAmount = prevAmount + delta;
        if (newAmount < 0) throw new ArgumentException($"Product number is: {prevAmount}. Attempting to withdraw {-delta}");
        product.ProductsNumber = newAmount;
        
        return await _productRepo.UpdateAsync<string>(product, query);
    }
    
    // consider adding SortableCriteria or similar refactoring
    // to make fetching of promotional/other products (and general filtering usage) cleaner
    public async Task<IEnumerable<StoreProductDto>> GetFilteredRegular(StoreProductSearchCriteria criteria)
    {
        var query = _queryProvider.GetAllRegular;
        var products = await _productRepo.FilterByPredicateAsync<StoreProductDto>(query, criteria);
        return products;
    }
    public async Task<string> CreateStoreProduct(StoreProductDto dto)
    {
        var entity = _mapper.Map<StoreProduct>(dto);
        var result = await _productRepo.InsertAsync<string>(entity, _queryProvider.CreateSingle);
        return result;
    }

    public async Task<string> UpdateStoreProduct(StoreProductDto dto)
    {
        var entity = _mapper.Map<StoreProduct>(dto);
        var result = await _productRepo.UpdateAsync<string>(entity, _queryProvider.UpdateByUpc);
        return result;
    }

    public async Task<bool> DeleteStoreProduct(string upc)
    {
        var productParams = new { UPC = upc };
        var existingEntity = await _productRepo.GetSingleAsync(_queryProvider.GetByUpc, productParams);
        if (existingEntity == null) 
            throw new ArgumentException($"No store product with UPC '{upc}' was found.");

        var rows = await _productRepo.DeleteAsync(_queryProvider.DeleteByUpc, productParams);
        return rows > 0;
    }
}