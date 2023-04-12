using MediatR;

namespace FeatureToggle.Application.Products.Commands;

public sealed record CreateProductCommand(string Name) : IRequest<Guid>;

public sealed class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Guid>
{
    public Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        // TODO: Implement.
        return Task.FromResult(Guid.NewGuid());
    }
}
