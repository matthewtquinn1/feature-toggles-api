using FeatureToggle.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FeatureToggle.Application.Features.Queries;

public sealed record GetFeaturesQuery() : IRequest<IEnumerable<FeatureDto>>;

public sealed class GetFeaturesQueryHandler : IRequestHandler<GetFeaturesQuery, IEnumerable<FeatureDto>>
{
    private readonly IApplicationDbContext _context;

    public GetFeaturesQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

	public async Task<IEnumerable<FeatureDto>> Handle(GetFeaturesQuery request, CancellationToken cancellationToken)
	{
        return await _context.Features
            .AsNoTracking()
            .Include(f => f.Product)
            .Include(f => f.FeatureStates)
            .Select(feature => feature.MapToDto())
            .ToListAsync(cancellationToken);
    }
}
