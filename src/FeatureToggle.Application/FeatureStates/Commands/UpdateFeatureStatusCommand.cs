using FeatureToggle.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FeatureToggle.Application.FeatureStates.Commands;

public sealed record UpdateFeatureStateCommand(
    Guid Id,
    bool IsActive) : IRequest<FeatureStateDto?>;

public sealed class UpdateFeatureStateCommandHandler : IRequestHandler<UpdateFeatureStateCommand, FeatureStateDto?>
{
    private readonly IApplicationDbContext _context;

    public UpdateFeatureStateCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<FeatureStateDto?> Handle(UpdateFeatureStateCommand request, CancellationToken cancellationToken)
    {
        var featureState = await _context.FeatureStates
            .FirstOrDefaultAsync(feature => feature.Id == request.Id, cancellationToken);

        if (featureState == null)
        {
            return null;
        }

        featureState.IsActive = request.IsActive;

        _ = _context.FeatureStates.Update(featureState);
        _ = await _context.SaveChangesAsync(cancellationToken);

        return featureState.MapToDto();
    }
}
