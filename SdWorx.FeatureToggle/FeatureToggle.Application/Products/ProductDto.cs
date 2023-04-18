using FeatureToggle.Application.Features;

namespace FeatureToggle.Application.Products;

public sealed record ProductDto(
        Guid Id,
        string Name,
        string Description,
        IEnumerable<FeatureDto> Features);
