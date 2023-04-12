using FeatureToggle.Application.Common.Exceptions;
using FeatureToggle.Application.Common.Interfaces;
using FeatureToggle.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FeatureToggle.Application.Products.Commands;

public sealed record UpdateProductCommand(Guid Id, string Name) : IRequest<Product>;

public sealed class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Product>
{
    private readonly IApplicationDbContext _context;

    public UpdateProductCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Product> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request), "Cannot update a product when request is null.");
        }

        var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (product == null)
        {
            throw new NotFoundException(nameof(product), request.Id);
        }

        product.Name = request.Name;

        _ = _context.Products.Update(product);
        _ = await _context.SaveChangesAsync(cancellationToken);

        return product;
    }
}
