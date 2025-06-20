using System.Transactions;
using AutoMapper;
using ShopApp.DAL.Repository;
using ShopApp.Data.DTO;
using ShopApp.Data.QueriesAccess;
using ShopApp.Data.SearchCriteria;

namespace ShopApp.Services;

public interface IStoreProductService
{
    Task<IEnumerable<StoreProductInfoDto>> GetAll();
    Task<StoreProductPriceNumberDto> GetPriceAndNumberByUpc(string upc);
    Task<StoreProductInfoDto> GetProductInfoByUpc(string upc);
    Task<IEnumerable<StoreProductInfoDto>> Filter(StoreProductSearchCriteria criteria);
    Task<IEnumerable<StoreProductInfoDto>> GetFilteredPromotional(StoreProductSearchCriteria criteria);
    Task<IEnumerable<StoreProductInfoDto>> GetFilteredRegular(StoreProductSearchCriteria criteria);
    Task<string> UpdateAmount(int delta, string productUpc);
    Task<string?> CreateStoreProduct(SaveStoreProductDto infoDto);
    Task<string> UpdateStoreProduct(SaveStoreProductDto infoDto);
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
    
    public async Task<IEnumerable<StoreProductInfoDto>> GetAll()
    {
        var products = await _productRepo.GetAllAsync<StoreProductInfoDto>(_queryProvider.GetAll);
        return products;
    }

    public async Task<StoreProductPriceNumberDto> GetPriceAndNumberByUpc(string upc)
    {
        var product = await _productRepo.GetSingleAsync(_queryProvider.GetByUpc, new { UPC = upc });
        return _mapper.Map<StoreProductPriceNumberDto>(product);
    }

    public async Task<StoreProductInfoDto> GetProductInfoByUpc(string upc)
    {
        var product = await _productRepo.GetSingleAsync<StoreProductInfoDto>(_queryProvider.GetProductInfoByUpc, new { UPC = upc });
        return product;
    }
    
    public async Task<IEnumerable<StoreProductInfoDto>> Filter(StoreProductSearchCriteria criteria)
    {
        var query = _queryProvider.GetAll;
        var products = await _productRepo.FilterByPredicateAsync<StoreProductInfoDto>(query, criteria);
        return products;
    }

    public async Task<IEnumerable<StoreProductInfoDto>> GetFilteredPromotional(StoreProductSearchCriteria criteria)
    {
        var query = _queryProvider.GetAllPromotional;
        var products = await _productRepo.FilterByPredicateAsync<StoreProductInfoDto>(query, criteria);
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
    
    public async Task<IEnumerable<StoreProductInfoDto>> GetFilteredRegular(StoreProductSearchCriteria criteria)
    {
        var query = _queryProvider.GetAllRegular;
        var products = await _productRepo.FilterByPredicateAsync<StoreProductInfoDto>(query, criteria);
        return products;
    }
    
    public async Task<string?> CreateStoreProduct(SaveStoreProductDto dto)
    {
        string? result;
        if (dto.PromotionalProduct && dto.UPCProm != null)
        {
            result = await CreatePromotionalProduct(dto);
        }
        else
        {
            result = await CreateRegularProduct(dto);
        }

        return result;
    }

    public async Task<string> UpdateStoreProduct(SaveStoreProductDto dto)
    {
        string? result;
        if (dto.PromotionalProduct)
        {
            result = await UpdatePromotional(dto);
        }
        else
        {
            result = await UpdateRegular(dto);
        }
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

    private async Task<StoreProduct?> GetPromotionalByUpc(string upc)
    {
        var result = await _productRepo.GetSingleAsync<StoreProduct>(_queryProvider.GetAllPromotionalByUpc, new { UPC = upc });
        return result;
    }

    private async Task<string> UpdatePromotional(SaveStoreProductDto dto)
    {
        var existing = await GetByUpc(dto.UPC);
        dto.SellingPrice = existing.SellingPrice; // selling price is immutable for promotional products
        var entity = _mapper.Map<StoreProduct>(dto);
        return await _productRepo.UpdateAsync<string>(entity, _queryProvider.UpdateByUpc);
    }

    private async Task<string> UpdateRegular(SaveStoreProductDto dto)
    {
        string? result;

        using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            var entity = _mapper.Map<StoreProduct>(dto);
            var existing = await GetByUpc(dto.UPC);

            result = await _productRepo.UpdateAsync<string>(entity, _queryProvider.UpdateByUpc);

            if (existing.SellingPrice != dto.SellingPrice)
            {
                var promotional = await GetPromotionalByUpc(dto.UPC);
                if (promotional != null)
                {
                    promotional.SellingPrice = dto.SellingPrice * 0.8m;
                    await _productRepo.UpdateAsync<string>(promotional, _queryProvider.UpdateByUpc); 
                }
            }

            transaction.Complete();
        }

        return result;
    }

    private async Task<StoreProduct> GetByUpc(string upc)
    {
        var product = await _productRepo.GetSingleAsync(_queryProvider.GetProductInfoByUpc, new { UPC = upc });
        if (product == null) throw new ArgumentException("Product was not found to be made promotional");
        return product;
    }

    private async Task<string> CreatePromotionalProduct(SaveStoreProductDto dto)
    {
        string? result;
        using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            var baseProduct = await GetByUpc(dto.UPC);

            if (baseProduct.SellingPrice * 0.8m != dto.SellingPrice)
                throw new ArgumentException("Promotional price should be 80% from the base price");
            
            var entity = new StoreProduct()
            {
                UPC = dto.UPCProm,
                UPCProm = null,
                PromotionalProduct = true,
                ProductsNumber = dto.ProductsNumber,
                IdProduct = dto.IdProduct,
                SellingPrice = dto.SellingPrice,
            };
            result = await _productRepo.InsertAsync<string>(entity, _queryProvider.CreateSingle);
            
            baseProduct.UPCProm = dto.UPCProm;
            baseProduct.IdProduct = dto.IdProduct;
            await _productRepo.UpdateAsync<int>(baseProduct, _queryProvider.UpdateByUpc);
            
            transaction.Complete();
        }

        return result;
    }
    
    private async Task<string> CreateRegularProduct(SaveStoreProductDto dto)
    {
        var entity = _mapper.Map<StoreProduct>(dto);
        return await _productRepo.InsertAsync<string>(entity, _queryProvider.CreateSingle);
    }
    
}