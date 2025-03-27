using ShopApp.Data.Entities;
using ShopApp.Data.Repositories;

namespace ShopApp.Services;

public interface IProductService
{
    Task<IEnumerable<Product>> GetAllProductsAsync();
}

public class ProductService : IProductService
{
    private readonly IRepository<Product> _productRepo;
    private readonly Dictionary<string, string> QueryMap = SqlQueryRegistry.SqlQueriesMap;

    public ProductService(IRepository<Product> productRepo)
    {
        _productRepo = productRepo;
    }
    
    public async Task<IEnumerable<Product>> GetAllProductsAsync()
    {
        return await _productRepo.GetAllAsync(QueryMap[SqlQueryRegistry.SqlQueriesKeys.GetAllProducts]);
    }
}