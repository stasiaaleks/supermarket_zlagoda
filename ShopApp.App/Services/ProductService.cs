using AutoMapper;
using ShopApp.DAL.Repository;
using ShopApp.Data.DTO;
using ShopApp.Data.QueriesAccess;
using ShopApp.Data.SearchCriteria;

namespace ShopApp.Services;

public interface IProductService
{
    Task<IEnumerable<ProductWithCategoryDto>> GetAll();
    Task<IEnumerable<ProductWithCategoryDto>> Filter(ProductSearchCriteria criteria);
    Task<ProductWithCategoryDto> GetById(string id);
    Task<string> CreateProduct(CreateProductDto dto);
    Task<string> UpdateById(ProductDto dto);
    Task<bool> DeleteById(int id);
}

public class ProductService : IProductService
{
    private readonly IRepository<ProductDto> _productRepo;
    private readonly ProductQueryProvider _queryProvider;
    private readonly IMapper _mapper;

    public ProductService(IRepository<ProductDto> productRepo, ProductQueryProvider queryProvider, IMapper mapper)
    {
        _productRepo = productRepo;
        _queryProvider = queryProvider;
        _mapper = mapper;
    }
    
    public async Task<IEnumerable<ProductWithCategoryDto>> GetAll()
    {
        return await _productRepo.GetAllAsync<ProductWithCategoryDto>(_queryProvider.GetAll);
    }

    public async Task<IEnumerable<ProductWithCategoryDto>> Filter(ProductSearchCriteria criteria)
    {
        var query = _queryProvider.GetAll;
        var products = await _productRepo.FilterByPredicateAsync<ProductWithCategoryDto>(query, criteria);
        return products;
    }

    public async Task<ProductWithCategoryDto> GetById(string id)
    {
        return await _productRepo.GetByIdAsync<ProductWithCategoryDto>(_queryProvider.GetById, id);
    }
    public async Task<string> CreateProduct(CreateProductDto dto)
    {
        var product = _mapper.Map<ProductDto>(dto);
        var newId = await _productRepo.InsertAsync<string>(product, _queryProvider.CreateSingle);
        return newId;
    }
    
    public async Task<string> UpdateById(ProductDto dto)
    {
        var updatedId = await _productRepo.UpdateAsync<string>(dto, _queryProvider.UpdateById);
        return updatedId;
    }

    public async Task<bool> DeleteById(int id)
    {
        var param = new { Id = id };
        var existingEntity = await _productRepo.GetSingleAsync(_queryProvider.GetById, param);
        
        if (existingEntity == null)
            throw new ArgumentException($"No product with id {id} was found.");

        var rows = await _productRepo.DeleteAsync(_queryProvider.DeleteById, param);
        return rows > 0;
    }

}