using FeatureToggle.Application.Common.Interfaces;
using FeatureToggle.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FeatureToggle.Application.Features.Queries;

public sealed record GetFeatureByIdQuery(Guid Id) : IRequest<Feature?>;

public sealed class GetFeatureByIdQueryHandler : IRequestHandler<GetFeatureByIdQuery, Feature?>
{
    private readonly IApplicationDbContext _context;

    public GetFeatureByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Feature?> Handle(GetFeatureByIdQuery request, CancellationToken cancellationToken)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request), "Cannot find feature when request is null");
        }

        var feature = await _context.Features
            .Include(f => f.Product)
            .Include(f => f.FeatureStates)
            .AsNoTracking()
            .SingleOrDefaultAsync(f => f.Id == request.Id, cancellationToken);

        if (feature == null)
        {
            return null;
        }

        return feature;
    }
}
