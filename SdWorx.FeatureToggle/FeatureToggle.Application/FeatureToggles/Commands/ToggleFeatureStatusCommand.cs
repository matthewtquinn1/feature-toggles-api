using FeatureToggle.Domain.Entities;
using FeatureToggle.Domain.Enums;
using MediatR;

namespace FeatureToggle.Application.FeatureToggles.Commands;

public sealed record ToggleFeatureStatusCommand(
    Guid id, 
    FeatureEnvironment environment, 
    bool isActive) : IRequest<Feature>;

public sealed class ToggleFeatureStatusCommandHandler : IRequestHandler<ToggleFeatureStatusCommand, Feature>
{
    public Task<Feature> Handle(ToggleFeatureStatusCommand request, CancellationToken cancellationToken)
    {
        // TODO: Implement.
        return Task.FromResult(new Feature());
    }
}
