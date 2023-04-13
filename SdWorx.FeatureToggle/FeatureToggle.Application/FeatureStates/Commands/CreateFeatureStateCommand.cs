using FeatureToggle.Domain.Enums;

namespace FeatureToggle.Application.FeatureStates.Commands;

public sealed record CreateFeatureStateCommand(
        FeatureEnvironment Environment,
        bool IsActive);
