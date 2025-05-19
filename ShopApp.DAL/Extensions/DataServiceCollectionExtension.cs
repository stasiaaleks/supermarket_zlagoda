using Microsoft.Extensions.DependencyInjection;
using ShopApp.DAL.DbConnection;
using ShopApp.DAL.Queries;
using ShopApp.DAL.Repository;

namespace ShopApp.DAL.Extensions;

public static class DataServiceCollectionExtension
{
    public static IServiceCollection AddDataServices(this IServiceCollection services)
    {
        services.AddScoped<IConnectionProvider, DbConnectionProvider>(); 
        services.AddScoped<IReadonlyRegistry, SqlQueryRegistry>(); 
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        return services;
    }
}