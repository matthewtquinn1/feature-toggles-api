using FeatureToggle.Application.FeatureStates;
using FeatureToggle.Domain.Entities;
using FeatureToggle.Domain.Enums;
using FluentAssertions;
using Xunit;

namespace FeatureToggle.Application.Tests.Unit.FeatureStates.Mappings;

public sealed class FeatureStateMappingsTests
{
    [Fact]
    public void MapToDto_ShouldMapFeatureStateToDto()
    {
        var featureState = new FeatureState
        {
            Id = Guid.NewGuid(),
            Environment = FeatureEnvironment.Local,
            IsActive = true,
        };

        var result = featureState.MapToDto();

        result.Id.Should().Be(featureState.Id);
        result.Environment.Should().Be(featureState.Environment);
        result.IsActive.Should().Be(featureState.IsActive);
    }
}
