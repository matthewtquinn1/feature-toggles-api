using FeatureToggle.Domain.Entities;

namespace FeatureToggle.Application.FeatureStates;

public static class FeatureStateMappings
{
    public static FeatureStateDto MapToDto(this FeatureState featureState)
    {
        return new FeatureStateDto(
            featureState.Id,
            featureState.Environment,
            featureState.IsActive);
    }
}
