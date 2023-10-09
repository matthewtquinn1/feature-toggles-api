using FeatureToggle.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FeatureToggle.Application.Features.Queries;

public sealed record GetFeatureByIdQuery(Guid Id) : IRequest<FeatureDto?>;

public sealed class GetFeatureByIdQueryHandler : IRequestHandler<GetFeatureByIdQuery, FeatureDto?>
{
    private readonly IApplicationDbContext _context;

    public GetFeatureByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<FeatureDto?> Handle(GetFeatureByIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.Features
            .AsNoTracking()
            .Include(f => f.Product)
            .Include(f => f.FeatureStates)
            .Select(f => f.MapToDto())
            .FirstOrDefaultAsync(f => f.Id == request.Id, cancellationToken);
    }
}
