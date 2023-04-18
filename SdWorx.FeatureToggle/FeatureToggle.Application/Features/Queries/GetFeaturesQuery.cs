using FeatureToggle.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FeatureToggle.Application.Features.Queries;

public sealed record GetFeaturesQuery() : IRequest<List<FeatureDto>>;

public sealed class GetFeaturesQueryHandler : IRequestHandler<GetFeaturesQuery, List<FeatureDto>>
{
    private readonly IApplicationDbContext _context;

    public GetFeaturesQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

	public async Task<List<FeatureDto>> Handle(GetFeaturesQuery request, CancellationToken cancellationToken)
	{
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request), "Cannot find feature when request is null");
        }

        return await _context.Features
            .Include(f => f.Product)
            .Include(f => f.FeatureStates)
            .AsNoTracking()
            .Select(feature => feature.MapToDto())
            .ToListAsync(cancellationToken);
    }
}
