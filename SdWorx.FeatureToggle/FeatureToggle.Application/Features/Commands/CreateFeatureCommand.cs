using FeatureToggle.Application.Common.Exceptions;
using FeatureToggle.Application.Common.Interfaces;
using FeatureToggle.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FeatureToggle.Application.Features.Commands;

public sealed record CreateFeatureCommand(
    Guid ProductId,
    string Name,
    ICollection<FeatureState> FeatureStates) : IRequest<Guid>;

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

        var feature = new Feature
        {
            Name = request.Name,
            ProductDbId = product.DbId,
            Product = product,
            FeatureStates = request.FeatureStates
        };

        _ = await _context.Features.AddAsync(feature, cancellationToken);
        _ = await _context.SaveChangesAsync(cancellationToken);

        return feature.Id;
    }
}