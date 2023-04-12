using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace FeatureToggle.Infrastructure.Persistance;

/// <summary>
/// This is used by EF Core Migrations when it wishes to run Migrations from somewhere other than the project with a program.cs.
/// </summary>
public sealed class FeatureToggleContextFactory : IDesignTimeDbContextFactory<FeatureToggleContext>
{
    public FeatureToggleContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<FeatureToggleContext>();
        optionsBuilder.UseSqlServer("Data Source=localhost; Initial Catalog=FeatureToggle; Integrated Security=true");

        return new FeatureToggleContext(optionsBuilder.Options);
    }
}
