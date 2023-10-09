using FeatureToggle.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

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
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request), "Cannot find product when request is null");
        }

        return await _context.Products
            .AsNoTracking()
            .Include(p => p.Features)
                .ThenInclude(f => f.FeatureStates)
            .Select(p => p.MapToDto())
            .FirstOrDefaultAsync(f => f.Id == request.Id, cancellationToken);
    }
}
