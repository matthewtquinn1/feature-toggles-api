using FeatureToggle.Application.Common.Interfaces;
using FeatureToggle.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FeatureToggle.Infrastructure.Persistance;

public sealed class FeatureToggleContext : DbContext, IApplicationDbContext
{
    public DbSet<Feature> Features { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<FeatureState> FeatureStates { get; set; }

    public FeatureToggleContext(DbContextOptions<FeatureToggleContext> options)
        : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(product => product.DbId);

            entity
                .Property(product => product.Id)
                .HasDefaultValueSql("NewId()");

            entity
                .Property(product => product.Name)
                .HasMaxLength(250)
                .IsRequired();

            entity
                .Property(product => product.Description)
                .HasMaxLength(500)
                .IsRequired();

            entity
                .HasMany(product => product.Features)
                .WithOne(feature => feature.Product)
                .HasForeignKey(feature => feature.ProductDbId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Feature>(entity =>
        {
            entity.HasKey(feature => feature.DbId);

            entity
                .Property(feature => feature.Id)
                .HasDefaultValueSql("NewId()");

            entity
                .Property(feature => feature.Name)
                .HasMaxLength(100)
                .IsRequired();

            entity
                .Property(product => product.Description)
                .HasMaxLength(500)
                .IsRequired();

            entity
                .HasMany(feature => feature.FeatureStates)
                .WithOne(state => state.Feature)
                .HasForeignKey(x => x.FeatureDbId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<FeatureState>(entity =>
        {
            entity.HasKey(state => state.DbId);

            entity
                .Property(state => state.Id)
                .HasDefaultValueSql("NewId()");
        });
    }
}
