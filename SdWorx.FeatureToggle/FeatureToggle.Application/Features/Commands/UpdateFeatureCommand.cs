using FeatureToggle.Application.Common.Exceptions;
using FeatureToggle.Application.Common.Interfaces;
using FeatureToggle.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FeatureToggle.Application.Features.Commands;

public sealed record UpdateFeatureCommand(
    Guid Id,
    string Name,
    string Description) : IRequest<FeatureDto>;

public sealed class UpdateFeatureCommandHandler : IRequestHandler<UpdateFeatureCommand, FeatureDto>
{
    private readonly IApplicationDbContext _context;

    public UpdateFeatureCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<FeatureDto> Handle(UpdateFeatureCommand request, CancellationToken cancellationToken)
    {
        var feature = await _context.Features
            .Include(f => f.Product)
            .Include(f => f.FeatureStates)
            .FirstOrDefaultAsync(f => f.Id == request.Id, cancellationToken);

        if (feature == null)
        {
            throw new NotFoundException(nameof(feature), request.Id);
        }

        var existingFeature = await _context.Features
            .AsNoTracking()
            .FirstOrDefaultAsync(f => f.Name == request.Name && f.Id != request.Id, cancellationToken);

        if (existingFeature != null)
        {
            throw new DuplicateFoundException(nameof(Feature), nameof(request.Name), request.Name);
        }

        feature.Name = request.Name;
        feature.Description = request.Description;

        _ = _context.Features.Update(feature);
        _ = await _context.SaveChangesAsync(cancellationToken);

        return feature.MapToDto();
    }
}