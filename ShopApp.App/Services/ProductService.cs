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

    public ProductService(IRepository<Product> productRepo)
    {
        _productRepo = productRepo;
    }
    
    public async Task<IEnumerable<Product>> GetAllProductsAsync()
    {
        string filePath = "Data/sql/Product/get_all_products.sql";
        return await _productRepo.GetAllAsync(filePath);
    }
}