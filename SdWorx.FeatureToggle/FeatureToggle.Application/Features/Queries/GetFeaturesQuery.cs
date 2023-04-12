using FeatureToggle.Application.Common.Interfaces;
using FeatureToggle.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FeatureToggle.Application.Features.Queries;

public sealed record GetFeaturesQuery() : IRequest<List<Feature>>;

public sealed class GetFeaturesQueryHandler : IRequestHandler<GetFeaturesQuery, List<Feature>>
{
    private readonly IApplicationDbContext _context;

    public GetFeaturesQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

	public async Task<List<Feature>> Handle(GetFeaturesQuery request, CancellationToken cancellationToken)
	{
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request), "Cannot find feature when request is null");
        }

        return await _context.Features
            .Include(f => f.Product)
            .Include(f => f.FeatureStates)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}
