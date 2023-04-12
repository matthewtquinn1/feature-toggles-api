using MediatR;

namespace FeatureToggle.Application.Features.Commands;

public sealed record CreateFeatureCommand(
    Guid ProductId,
    string Name) : IRequest<Guid>;

public sealed class CreateFeatureCommandHandler : IRequestHandler<CreateFeatureCommand, Guid>
{
    public Task<Guid> Handle(CreateFeatureCommand request, CancellationToken cancellationToken)
    {
        // TODO: Implement.
        return Task.FromResult(Guid.NewGuid());
    }
}