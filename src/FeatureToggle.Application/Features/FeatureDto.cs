using FeatureToggle.Application.FeatureStates;

namespace FeatureToggle.Application.Features;

public sealed record FeatureDto(
    Guid Id,
    string Name,
    string Description,
    IEnumerable<FeatureStateDto> FeatureStates);
