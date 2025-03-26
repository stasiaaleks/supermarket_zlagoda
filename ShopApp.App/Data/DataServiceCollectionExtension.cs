namespace ShopApp.Data;

public static class DataServiceCollectionExtension
{
    public static IServiceCollection AddDataServices(this IServiceCollection services)
    {
        services.AddScoped<IConnectionProvider, DbConnectionProvider>(); 
        return services;
    }
}