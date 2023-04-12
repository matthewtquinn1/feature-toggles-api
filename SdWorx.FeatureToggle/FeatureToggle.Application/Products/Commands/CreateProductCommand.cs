using FeatureToggle.Application.Common.Exceptions;
using FeatureToggle.Application.Common.Interfaces;
using FeatureToggle.Application.Features.Commands;
using FeatureToggle.Domain.Entities;
using MediatR;

namespace FeatureToggle.Application.Products.Commands;

public sealed record CreateProductCommand(string Name) : IRequest<Guid>;

public sealed class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateProductCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request), "Cannot create a product when request is null.");
        }

        var product = new Product { Name = request.Name };

        _ = await _context.Products.AddAsync(product, cancellationToken);
        _ = await _context.SaveChangesAsync(cancellationToken);

        return product.Id;
    }
}
