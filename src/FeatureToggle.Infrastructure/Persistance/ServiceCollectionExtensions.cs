using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FeatureToggle.Infrastructure.Persistance;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// This allows us to leave the dependency of our choice of persistence in the Infrastructure project.
    /// </summary>
    /// <param name="services">ServiceCollection to extend upon.</param>
    /// <param name="connectionString">The connection string for the DbContext.</param>
    public static IServiceCollection AddPersistence(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<FeatureToggleContext>(options => options.UseSqlServer(connectionString));

        return services;
    }
}
