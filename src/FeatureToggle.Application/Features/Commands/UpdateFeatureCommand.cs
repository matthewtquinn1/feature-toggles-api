using FeatureToggle.Application.Common.Exceptions;
using FeatureToggle.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace FeatureToggle.Application.Features.Commands;

public sealed record UpdateFeatureCommand(
    Guid Id,
    [Required][MaxLength(500)] string Description) : IRequest<FeatureDto?>;

public sealed class UpdateFeatureCommandHandler : IRequestHandler<UpdateFeatureCommand, FeatureDto?>
{
    private readonly IApplicationDbContext _context;

    public UpdateFeatureCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<FeatureDto?> Handle(UpdateFeatureCommand request, CancellationToken cancellationToken)
    {
        var feature = await _context.Features
            .Include(f => f.Product)
            .Include(f => f.FeatureStates)
            .FirstOrDefaultAsync(f => f.Id == request.Id, cancellationToken);

        if (feature == null)
        {
            return null;
        }

        feature.Description = request.Description;

        _ = _context.Features.Update(feature);
        _ = await _context.SaveChangesAsync(cancellationToken);

        return feature.MapToDto();
    }
}