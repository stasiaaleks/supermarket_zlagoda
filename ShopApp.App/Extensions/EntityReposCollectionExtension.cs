using ShopApp.Repositories;

namespace ShopApp.Extensions;

public static class EntityReposCollectionExtension
{
    public static IServiceCollection AddEntityRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        return services;
    }
}