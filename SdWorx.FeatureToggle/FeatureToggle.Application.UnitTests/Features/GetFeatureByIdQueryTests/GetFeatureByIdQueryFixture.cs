using FeatureToggle.Application.Common.Interfaces;
using FeatureToggle.Application.Features.Queries;
using FeatureToggle.Domain.Entities;
using MockQueryable.NSubstitute;
using NSubstitute;
using System.Diagnostics.CodeAnalysis;

namespace FeatureToggle.Application.UnitTests.Features.GetFeatureByIdQueryTests;

[ExcludeFromCodeCoverage]
internal sealed class GetFeatureByIdQueryFixture
{
    internal readonly IApplicationDbContext ApplicationDbContext = Substitute.For<IApplicationDbContext>();

    public GetFeatureByIdQueryHandler CreateSut() => new(ApplicationDbContext);

    internal GetFeatureByIdQueryFixture WithReturnForContextFeatures(IEnumerable<Feature> result)
    {
        ApplicationDbContext.Features = result.AsQueryable().BuildMockDbSet();

        return this;
    }
}
