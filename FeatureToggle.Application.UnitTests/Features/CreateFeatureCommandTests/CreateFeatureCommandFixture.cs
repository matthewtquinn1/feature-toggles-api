using FeatureToggle.Application.Common.Interfaces;
using FeatureToggle.Application.Features.Commands;
using FeatureToggle.Domain.Entities;
using MockQueryable.NSubstitute;
using NSubstitute;
using System.Diagnostics.CodeAnalysis;

namespace FeatureToggle.Application.UnitTests.Features.CreateFeatureCommandTests;

[ExcludeFromCodeCoverage]
internal sealed class CreateFeatureCommandFixture
{
    internal readonly IApplicationDbContext ApplicationDbContext = Substitute.For<IApplicationDbContext>();

    public CreateFeatureCommandHandler CreateSut() => new(ApplicationDbContext);

    internal CreateFeatureCommandFixture WithReturnForContextProducts(IEnumerable<Product> result)
    {
        ApplicationDbContext.Products = result.AsQueryable().BuildMockDbSet();

        return this;
    }

    internal CreateFeatureCommandFixture WithReturnForContextFeatures(IEnumerable<Feature> result)
    {
        ApplicationDbContext.Features = result.AsQueryable().BuildMockDbSet();

        return this;
    }
}
