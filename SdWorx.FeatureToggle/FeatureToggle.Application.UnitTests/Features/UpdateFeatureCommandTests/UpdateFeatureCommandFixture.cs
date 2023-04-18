using FeatureToggle.Application.Common.Interfaces;
using FeatureToggle.Application.Features.Commands;
using FeatureToggle.Domain.Entities;
using MockQueryable.NSubstitute;
using NSubstitute;
using System.Diagnostics.CodeAnalysis;

namespace FeatureToggle.Application.UnitTests.Features.UpdateFeatureCommandTests;

[ExcludeFromCodeCoverage]
internal sealed class UpdateFeatureCommandFixture
{
    internal readonly IApplicationDbContext ApplicationDbContext = Substitute.For<IApplicationDbContext>();

    public UpdateFeatureCommandHandler CreateSut() => new(ApplicationDbContext);

    internal UpdateFeatureCommandFixture WithReturnForContextProducts(IEnumerable<Product> result)
    {
        ApplicationDbContext.Products = result.AsQueryable().BuildMockDbSet();

        return this;
    }

    internal UpdateFeatureCommandFixture WithReturnForContextFeatures(IEnumerable<Feature> result)
    {
        ApplicationDbContext.Features = result.AsQueryable().BuildMockDbSet();

        return this;
    }
}
