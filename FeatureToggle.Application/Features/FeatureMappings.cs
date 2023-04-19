using FeatureToggle.Application.FeatureStates;
using FeatureToggle.Domain.Entities;

namespace FeatureToggle.Application.Features;

internal static class FeatureMappings
{
    internal static FeatureDto MapToDto(this Feature feature)
    {
        return new FeatureDto(
            feature.Id,
            feature.Name,
            feature.Description,
            feature.Product,
            feature.FeatureStates.Select(featureState => featureState.MapToDto()));
    }
}
