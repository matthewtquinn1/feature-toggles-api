using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FeatureToggle.Infrastructure.Persistance;

public static class WebApplicationExtensions
{
    public static void ApplyFeatureToggleMigrations(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;

        var context = services.GetRequiredService<FeatureToggleContext>();
        context.Database.Migrate();
    }
}
