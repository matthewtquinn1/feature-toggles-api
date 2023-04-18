using FeatureToggle.Application.Common.Exceptions;
using FeatureToggle.Application.Common.Interfaces;
using FeatureToggle.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FeatureToggle.Application.FeatureStates.Commands;

public sealed record UpdateFeatureStateCommand(
    Guid Id,
    bool IsActive) : IRequest<FeatureStateDto>;

public sealed class UpdateFeatureStateCommandHandler : IRequestHandler<UpdateFeatureStateCommand, FeatureStateDto>
{
    private readonly IApplicationDbContext _context;

    public UpdateFeatureStateCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<FeatureStateDto> Handle(UpdateFeatureStateCommand request, CancellationToken cancellationToken)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request), "Cannot toggle a feature's status when request is null.");
        }

        var featureState = await _context.FeatureStates
            .FirstOrDefaultAsync(feature => feature.Id == request.Id, cancellationToken);

        if (featureState == null)
        {
            throw new NotFoundException(nameof(featureState), request.Id);
        }

        featureState.IsActive = request.IsActive;

        _ = _context.FeatureStates.Update(featureState);
        _ = await _context.SaveChangesAsync(cancellationToken);

        return featureState.MapToDto();
    }
}
