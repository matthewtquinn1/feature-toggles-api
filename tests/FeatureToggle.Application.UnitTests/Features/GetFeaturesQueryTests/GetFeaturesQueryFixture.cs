using FeatureToggle.Application.Common.Interfaces;
using FeatureToggle.Application.Features.Queries;
using FeatureToggle.Domain.Entities;
using MockQueryable.NSubstitute;
using NSubstitute;
using System.Diagnostics.CodeAnalysis;

namespace FeatureToggle.Application.UnitTests.Features.GetFeaturesQueryTests;

[ExcludeFromCodeCoverage]
internal sealed class GetFeaturesQueryFixture
{
    internal readonly IApplicationDbContext ApplicationDbContext = Substitute.For<IApplicationDbContext>();

    public GetFeaturesQueryHandler CreateSut() => new(ApplicationDbContext);

    internal GetFeaturesQueryFixture WithReturnForContextFeatures(IEnumerable<Feature> result)
    {
        ApplicationDbContext.Features = result.AsQueryable().BuildMockDbSet();

        return this;
    }
}
