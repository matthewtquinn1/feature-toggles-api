using FeatureToggle.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace FeatureToggle.Application.Products.Queries;

public sealed record GetProductByIdQuery(Guid Id) : IRequest<ProductDto?>;

public sealed class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductDto?>
{
    private readonly IApplicationDbContext _context;

    public GetProductByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ProductDto?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        return (await _context.Products
            .AsNoTracking()
            .Include(p => p.Features)
                .ThenInclude(f => f.FeatureStates)
            .FirstOrDefaultAsync(f => f.Id == request.Id, cancellationToken))
            ?.MapToDto(); // TODO: investigate why EF Core cannot convert this Linq to SQL.
    }
}
