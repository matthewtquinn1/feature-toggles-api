using FeatureToggle.Application.Features;
using FeatureToggle.Domain.Entities;

namespace FeatureToggle.Application.Products;

public static class ProductMappings
{
    public static ProductDto MapToDto(this Product product)
    {
        return new ProductDto(
            product.Id,
            product.Name,
            product.Description,
            product.Features.Select(feature => feature.MapToDto()));
    }
}
