using MediatR;

namespace FeatureToggle.Application.Products.Commands;

public sealed record DeleteProductCommand(Guid Id) : IRequest<Unit>;

public sealed class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, Unit>
{
    public Task<Unit> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        // TODO: Implement.
        return Task.FromResult(Unit.Value);
    }
}
