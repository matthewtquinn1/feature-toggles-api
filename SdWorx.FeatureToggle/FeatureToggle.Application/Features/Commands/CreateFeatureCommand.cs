using FeatureToggle.Application.Common.Exceptions;
using FeatureToggle.Application.Common.Interfaces;
using FeatureToggle.Application.FeatureStates.Commands;
using FeatureToggle.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FeatureToggle.Application.Features.Commands;

public sealed record CreateFeatureCommand(
    Guid ProductId,
    string Name,
    string Description,
    ICollection<CreateFeatureStateCommand> FeatureStates) : IRequest<Guid>;

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
            Description = request.Description,
            ProductDbId = product.DbId,
            Product = product,
            FeatureStates = request.FeatureStates
                .Select(state => new FeatureState { Environment = state.Environment, IsActive = state.IsActive })
                .ToList()
        };

        _ = await _context.Features.AddAsync(feature, cancellationToken);
        _ = await _context.SaveChangesAsync(cancellationToken);

        return feature.Id;
    }
}