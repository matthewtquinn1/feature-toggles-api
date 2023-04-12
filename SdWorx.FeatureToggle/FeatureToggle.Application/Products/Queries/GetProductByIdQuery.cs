using FeatureToggle.Domain.Entities;
using MediatR;

namespace FeatureToggle.Application.Products.Queries;

public sealed record GetProductByIdQuery(Guid id) : IRequest<Product>;

public sealed class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, Product>
{
    public Task<Product> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new Product());
    }
}
