using FeatureToggle.Application.Common.Exceptions;
using FeatureToggle.Application.Common.Interfaces;
using FeatureToggle.Domain.Entities;
using FeatureToggle.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FeatureToggle.Application.Features.Commands;

public sealed record ToggleFeatureStatusCommand(
    Guid Id, 
    FeatureEnvironment Environment,
    bool IsActive) : IRequest<Feature>;

public sealed class ToggleFeatureStatusCommandHandler : IRequestHandler<ToggleFeatureStatusCommand, Feature>
{
    private readonly IApplicationDbContext _context;

    public ToggleFeatureStatusCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Feature> Handle(ToggleFeatureStatusCommand request, CancellationToken cancellationToken)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request), "Cannot toggle a feature's status when request is null.");
        }

        var feature = await _context.Features
            .Include(feature => feature.Product)
            .Include(feature => feature.FeatureStates)
            .FirstOrDefaultAsync(feature => feature.Id == request.Id, cancellationToken);

        if (feature == null)
        {
            throw new NotFoundException(nameof(feature), request.Id);
        }

        var stateToChange = feature.FeatureStates.First(x => x.Environment == request.Environment);
        stateToChange.IsActive = !stateToChange.IsActive;

        _ = _context.Features.Update(feature);
        _ = await _context.SaveChangesAsync(cancellationToken);

        return feature;
    }
}
