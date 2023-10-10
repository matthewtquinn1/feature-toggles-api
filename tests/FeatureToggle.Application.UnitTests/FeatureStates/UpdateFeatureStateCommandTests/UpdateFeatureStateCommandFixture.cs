using FeatureToggle.Application.Common.Interfaces;
using FeatureToggle.Application.FeatureStates.Commands;
using FeatureToggle.Domain.Entities;
using MockQueryable.NSubstitute;
using NSubstitute;
using System.Diagnostics.CodeAnalysis;

namespace FeatureToggle.Application.Tests.Unit.FeatureStates.UpdateFeatureStateCommandTests;

[ExcludeFromCodeCoverage]
internal sealed class UpdateFeatureStateCommandFixture
{
    internal readonly IApplicationDbContext ApplicationDbContext = Substitute.For<IApplicationDbContext>();

    public UpdateFeatureStateCommandHandler CreateSut() => new(ApplicationDbContext);

    internal UpdateFeatureStateCommandFixture WithReturnForContextFeatureStates(IEnumerable<FeatureState> result)
    {
        ApplicationDbContext.FeatureStates = result.AsQueryable().BuildMockDbSet();

        return this;
    }
}
