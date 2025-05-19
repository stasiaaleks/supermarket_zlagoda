using ShopApp.DAL.Queries;
using ShopApp.DAL.Repository;
using ShopApp.Data.Entities;
using ShopApp.Data.QueryKeys;

namespace ShopApp.Services;

public interface IProductService
{
    Task<IEnumerable<Product>> GetAllProductsAsync();
}

public class ProductService : IProductService
{
    private readonly IRepository<Product> _productRepo;
    private readonly IReadonlyRegistry _sqlQueryRegistry;

    public ProductService(IRepository<Product> productRepo, IReadonlyRegistry sqlQueryRegistry)
    {
        _productRepo = productRepo;
        _sqlQueryRegistry = sqlQueryRegistry;
    }
    
    public async Task<IEnumerable<Product>> GetAllProductsAsync()
    {
        var query = _sqlQueryRegistry.Load(ProductQueries.GetAllPath);
        return await _productRepo.GetAllAsync(query);
    }
}