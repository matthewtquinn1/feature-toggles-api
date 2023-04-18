using FeatureToggle.Application.Common.Interfaces;
using FeatureToggle.Application.Products.Commands;
using FeatureToggle.Domain.Entities;
using MockQueryable.NSubstitute;
using NSubstitute;
using System.Diagnostics.CodeAnalysis;

namespace FeatureToggle.Application.UnitTests.Products.CreateProductCommandTests;

[ExcludeFromCodeCoverage]
internal sealed class CreateProductCommandFixture
{
    internal readonly IApplicationDbContext ApplicationDbContext = Substitute.For<IApplicationDbContext>();

    public CreateProductCommandHandler CreateSut() => new(ApplicationDbContext);

    internal CreateProductCommandFixture WithReturnForContextProducts(IEnumerable<Product> result)
    {
        ApplicationDbContext.Products = result.AsQueryable().BuildMockDbSet();

        return this;
    }

    internal CreateProductCommandFixture WithReturnForContextFeatures(IEnumerable<Feature> result)
    {
        ApplicationDbContext.Features = result.AsQueryable().BuildMockDbSet();

        return this;
    }
}
