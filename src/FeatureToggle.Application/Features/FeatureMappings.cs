using FeatureToggle.Application.FeatureStates;
using FeatureToggle.Application.Products;
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
            feature.FeatureStates.Select(featureState => featureState.MapToDto()));
    }
}
