using FeatureToggle.Application.Features;
using FeatureToggle.Domain.Enums;

namespace FeatureToggle.Application.FeatureStates;

public sealed record FeatureStateDto(
    Guid Id,
    FeatureEnvironment Environment,
    bool IsActive,
    FeatureDto Feature);
