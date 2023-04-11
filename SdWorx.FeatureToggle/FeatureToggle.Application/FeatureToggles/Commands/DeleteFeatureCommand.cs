using MediatR;

namespace FeatureToggle.Application.FeatureToggles.Commands;

public sealed record DeleteFeatureCommand(Guid id) : IRequest<Unit>;

public sealed class DeleteFeatureCommandHandler : IRequestHandler<DeleteFeatureCommand, Unit>
{
    public Task<Unit> Handle(DeleteFeatureCommand request, CancellationToken cancellationToken)
    {
        // TODO: Implement.
        return Task.FromResult(Unit.Value);
    }
}
