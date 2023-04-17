using FeatureToggle.Application.Common.Exceptions;
using FeatureToggle.Application.Common.Interfaces;
using FeatureToggle.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FeatureToggle.Application.Features.Commands;

public sealed record UpdateFeatureCommand(
    Guid Id,
    Guid ProductId,
    string Name) : IRequest<Feature>;

public sealed class UpdateFeatureCommandHandler : IRequestHandler<UpdateFeatureCommand, Feature>
{
    private readonly IApplicationDbContext _context;

    public UpdateFeatureCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Feature> Handle(UpdateFeatureCommand request, CancellationToken cancellationToken)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request), "Cannot update a feature when request is null.");
        }

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
            .FirstOrDefaultAsync(f => f.Name == request.Name, cancellationToken);

        if (existingFeature != null)
        {
            throw new DuplicateFoundException(nameof(Feature), nameof(request.Name), request.Name);
        }

        feature.Name = request.Name;

        // Only look for the product in the request when it is changed.
        var product = feature.Product.Id == request.ProductId
            ? feature.Product
            : await _context.Products.FirstOrDefaultAsync(p => p.Id == request.ProductId, cancellationToken)
                ?? throw new NotFoundException(nameof(feature.Product), request.ProductId);

        feature.Product = product;
        feature.ProductDbId = product.DbId;

        _ = _context.Features.Update(feature);
        _ = await _context.SaveChangesAsync(cancellationToken);

        return feature;
    }
}