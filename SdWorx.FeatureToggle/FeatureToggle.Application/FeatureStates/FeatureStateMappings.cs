using FeatureToggle.Application.Features;
using FeatureToggle.Domain.Entities;

namespace FeatureToggle.Application.FeatureStates;

internal static class FeatureStateMappings
{
    internal static FeatureStateDto MapToDto(this FeatureState featureState)
    {
        return new FeatureStateDto(
            featureState.Id,
            featureState.Environment,
            featureState.IsActive,
            featureState.Feature.MapToDto());
    }
}
