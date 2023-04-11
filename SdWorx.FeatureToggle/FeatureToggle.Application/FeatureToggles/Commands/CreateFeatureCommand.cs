using MediatR;

namespace FeatureToggle.Application.FeatureToggles.Commands;

public sealed record CreateFeatureCommand(
    string Domain ,// Api, Frontend, both?
    string Product, // PayEngine, Exporter etc.
    string Name) : IRequest<Guid>;

public sealed class CreateFeatureCommandHandler : IRequestHandler<CreateFeatureCommand, Guid>
{
    public Task<Guid> Handle(CreateFeatureCommand request, CancellationToken cancellationToken)
    {
        // TODO: Implement.
        return Task.FromResult(Guid.NewGuid());
    }
}