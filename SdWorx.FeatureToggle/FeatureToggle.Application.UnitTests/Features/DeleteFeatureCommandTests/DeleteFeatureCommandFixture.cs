using FeatureToggle.Application.Common.Interfaces;
using FeatureToggle.Application.Features.Commands;
using FeatureToggle.Domain.Entities;
using MockQueryable.NSubstitute;
using NSubstitute;

namespace FeatureToggle.Application.UnitTests.Features.DeleteFeatureCommandTests;

internal sealed class DeleteFeatureCommandFixture
{
    internal readonly IApplicationDbContext ApplicationDbContext = Substitute.For<IApplicationDbContext>();

    public DeleteFeatureCommandHandler CreateSut() => new(ApplicationDbContext);

    internal DeleteFeatureCommandFixture WithReturnForContextFeatures(IEnumerable<Feature> result)
    {
        ApplicationDbContext.Features = result.AsQueryable().BuildMockDbSet();

        return this;
    }
}
