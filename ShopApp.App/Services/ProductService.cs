using ShopApp.DAL.Queries;
using ShopApp.DAL.Repository;
using ShopApp.Data.Entities;
using ShopApp.Data.QueriesAccess;

namespace ShopApp.Services;

public interface IProductService
{
    Task<IEnumerable<Product>> GetAllProductsAsync();
}

public class ProductService : IProductService
{
    private readonly IRepository<Product> _productRepo;
    private readonly IReadonlyRegistry _sqlQueryRegistry;
    private readonly ProductQueryProvider _queryProvider;

    public ProductService(IRepository<Product> productRepo, IReadonlyRegistry sqlQueryRegistry, ProductQueryProvider queryProvider)
    {
        _productRepo = productRepo;
        _sqlQueryRegistry = sqlQueryRegistry;
        _queryProvider = queryProvider;
    }
    
    public async Task<IEnumerable<Product>> GetAllProductsAsync()
    {
        var query = _sqlQueryRegistry.Load(_queryProvider.GetAllPath);
        return await _productRepo.GetAllAsync(query);
    }
}