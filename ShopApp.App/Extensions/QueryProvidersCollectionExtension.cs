using ShopApp.Data.QueriesAccess;

namespace ShopApp.Extensions;

public static class QueryProvidersCollectionExtension
{
    public static IServiceCollection AddQueryProviders(this IServiceCollection services)
    {
        // suggestion: refactor as a factory instead of a concrete implementation
        services.AddScoped<ProductQueryProvider>();
        services.AddScoped<EmployeeQueryProvider>();
        services.AddScoped<CategoryQueryProvider>();
        services.AddScoped<CustomerCardQueryProvider>();
        services.AddScoped<UserQueryProvider>();
        services.AddScoped<CheckQueryProvider>();
        services.AddScoped<SaleQueryProvider>();
        services.AddScoped<StoreProductQueryProvider>();
        services.AddScoped<StatisticsQueryProvider>();
        
        return services;
    }
}