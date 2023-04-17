using FeatureToggle.Application.Common.Exceptions;
using FeatureToggle.Application.Common.Interfaces;
using FeatureToggle.Domain.Entities;
using FeatureToggle.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FeatureToggle.Application.Features.Commands;

public sealed record CreateFeatureCommand(
    Guid ProductId,
    string Name,
    string Description) : IRequest<Guid>;

public sealed class CreateFeatureCommandHandler : IRequestHandler<CreateFeatureCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateFeatureCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateFeatureCommand request, CancellationToken cancellationToken)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request), "Cannot create a feature when request is null.");
        }

        var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == request.ProductId, cancellationToken);

        if (product == null)
        {
            throw new NotFoundException(nameof(product), request.ProductId);
        }

        var existingFeature = await _context.Features
            .AsNoTracking()
            .FirstOrDefaultAsync(f => f.Name == request.Name, cancellationToken);

        if (existingFeature != null)
        {
            throw new Exception(string.Format("Feature with the name {0} already exists.", request.Name));
        }

        var feature = new Feature
        {
            Name = request.Name,
            Description = request.Description,
            ProductDbId = product.DbId,
            Product = product,
        };

        // Simpler to just create all states for a feature and default values to false.
        var featureStates = new List<FeatureState>();
        foreach (var state in (FeatureEnvironment[])Enum.GetValues(typeof(FeatureEnvironment)))
        {
            featureStates.Add(new() { Environment = state, IsActive = false });
        }
        feature.FeatureStates = featureStates;

        _ = await _context.Features.AddAsync(feature, cancellationToken);
        _ = await _context.SaveChangesAsync(cancellationToken);

        return feature.Id;
    }
}