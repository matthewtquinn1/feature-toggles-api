using FeatureToggle.Domain.Entities;
using MediatR;

namespace FeatureToggle.Application.FeatureToggles.Commands;

public sealed record UpdateFeatureCommand(
    int DbId,
    Guid Id,
    string Domain,
    Product Product,
    string Name,
    IEnumerable<FeatureState> EnvironmentStates) : IRequest<Feature>;

public sealed class UpdateFeatureCommandHandler : IRequestHandler<UpdateFeatureCommand, Feature>
{
    public Task<Feature> Handle(UpdateFeatureCommand request, CancellationToken cancellationToken)
    {
        // TODO: Implement.
        return Task.FromResult(new Feature());
    }
}