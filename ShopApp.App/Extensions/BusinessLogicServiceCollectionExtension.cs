using ShopApp.Services;
using ShopApp.Services.Auth;

namespace ShopApp.Extensions;

public static class BusinessLogicServiceCollectionExtension
{
    public static IServiceCollection AddBusinessLogicServices(this IServiceCollection services)
    {
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IEmployeeService, EmployeeService>();
        services.AddScoped<ICheckService, CheckService>();
        services.AddScoped<ISaleService, SaleService>();
        services.AddScoped<IStoreProductService, StoreProductService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<ICustomerCardService, CustomerCardService>();
        
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IPasswordHasher, Pbkdf2PasswordHasher>();
        return services;
    }
}