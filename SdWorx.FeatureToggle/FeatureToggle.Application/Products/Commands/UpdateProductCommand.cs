using FeatureToggle.Domain.Entities;
using MediatR;

namespace FeatureToggle.Application.Products.Commands;

public sealed record UpdateProductCommand(Guid Id, string Name) : IRequest<Product>;

public sealed class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Product>
{
    public Task<Product> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        // TODO: Implement.
        return Task.FromResult(new Product {  Id = Guid.NewGuid() , DbId = 2, Name = "Pay Engine API", Features = new List<Feature>() });
    }
}
