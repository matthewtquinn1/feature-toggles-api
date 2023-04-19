using FeatureToggle.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FeatureToggle.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Feature> Features { get; set; }
    public DbSet<FeatureState> FeatureStates { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
