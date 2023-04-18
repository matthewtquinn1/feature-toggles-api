using FeatureToggle.Application.FeatureStates;
using FeatureToggle.Domain.Entities;

namespace FeatureToggle.Application.Features;

public sealed record FeatureDto(
    Guid Id,
    string Name,
    string Description,
    Product Product,
    IEnumerable<FeatureStateDto> FeatureStates);