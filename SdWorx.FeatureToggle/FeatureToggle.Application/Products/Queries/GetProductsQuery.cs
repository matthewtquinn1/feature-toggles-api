using FeatureToggle.Domain.Entities;
using MediatR;

namespace FeatureToggle.Application.Products.Queries;

public sealed record GetProductsQuery() : IRequest<List<Product>>;

public sealed class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, List<Product>>
{
    public Task<List<Product>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new List<Product>());
    }
}
