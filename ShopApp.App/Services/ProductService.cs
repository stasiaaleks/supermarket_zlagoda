using AutoMapper;
using ShopApp.DAL.Repository;
using ShopApp.Data.DTO;
using ShopApp.Data.QueriesAccess;
using ShopApp.Data.SearchCriteria;

namespace ShopApp.Services;

public interface IProductService
{
    Task<IEnumerable<ProductDto>> GetAll();
    Task<IEnumerable<ProductDto>> Filter(ProductSearchCriteria criteria);
    Task<ProductDto> GetById(string id);
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
    
    public async Task<IEnumerable<ProductDto>> GetAll()
    {
        var products = await _productRepo.GetAllAsync(_queryProvider.GetAll);
        return _mapper.Map<IEnumerable<ProductDto>>(products);
    }

    public async Task<IEnumerable<ProductDto>> Filter(ProductSearchCriteria criteria)
    {
        var query = _queryProvider.GetAll;
        var products = await _productRepo.FilterByPredicateAsync(query, criteria);
        return products;
    }

    public async Task<ProductDto> GetById(string id)
    {
        var product = await _productRepo.GetByIdAsync(_queryProvider.GetById, id);
        return _mapper.Map<ProductDto>(product);
    }
}