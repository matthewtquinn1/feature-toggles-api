using FeatureToggle.Application.FeatureStates;
using FeatureToggle.Application.Products;
using FeatureToggle.Domain.Entities;

namespace FeatureToggle.Application.Features;

public static class FeatureMappings
{
    public static FeatureDto MapToDto(this Feature feature)
    {
        if (feature?.Product is null)
        {
            throw new ArgumentNullException(
                nameof(feature.Product), 
                "Cannot convert feature to DTO when product is missing");
        }

        return new FeatureDto(
            feature.Id,
            feature.Product.Id,
            feature.Name,
            feature.Description,
            feature.FeatureStates.Select(featureState => featureState.MapToDto()));
    }
}
