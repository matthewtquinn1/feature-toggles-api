using FeatureToggle.Application.FeatureStates;
using FeatureToggle.Application.Products;

namespace FeatureToggle.Application.Features;

public sealed record FeatureDto(
    Guid Id,
    string Name,
    string Description,
    ProductDto Product,
    IEnumerable<FeatureStateDto> FeatureStates);