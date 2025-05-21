using ShopApp.Data.QueriesAccess;

namespace ShopApp.Extensions;

public static class QueryProvidersCollectionExtension
{
    public static IServiceCollection AddQueryProviders(this IServiceCollection services)
    {
        // TODO: consider refactoring as a factory instead of a concrete implementation
        services.AddScoped<ProductQueryProvider>();
        services.AddScoped<EmployeeQueryProvider>();
        services.AddScoped<CategoryQueryProvider>();
        services.AddScoped<UserQueryProvider>();
        
        return services;
    }
}