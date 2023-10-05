using FeatureToggle.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FeatureToggle.Application.Products.Queries;

public sealed record GetProductsQuery() : IRequest<IEnumerable<ProductDto>>;

public sealed class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, IEnumerable<ProductDto>>
{
    private readonly IApplicationDbContext _context;

    public GetProductsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ProductDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request), "Cannot find feature when request is null");
        }

        return await _context.Products
            .AsNoTracking()
            .Include(p => p.Features)
                .ThenInclude(f => f.FeatureStates)
            .Select(x => x.MapToDto())
            .ToListAsync(cancellationToken);
    }
}
