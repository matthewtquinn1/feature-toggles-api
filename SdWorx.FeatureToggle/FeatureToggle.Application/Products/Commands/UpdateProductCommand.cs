using FeatureToggle.Application.Common.Exceptions;
using FeatureToggle.Application.Common.Interfaces;
using FeatureToggle.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FeatureToggle.Application.Products.Commands;

public sealed record UpdateProductCommand(Guid Id, string Name, string Description) : IRequest<ProductDto>;

public sealed class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, ProductDto>
{
    private readonly IApplicationDbContext _context;

    public UpdateProductCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ProductDto> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (product == null)
        {
            throw new NotFoundException(nameof(product), request.Id);
        }

        var existingProduct = await _context.Products
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Name == request.Name, cancellationToken);

        if (existingProduct != null)
        {
            throw new DuplicateFoundException(nameof(Product), nameof(request.Name), request.Name);
        }

        product.Name = request.Name;
        product.Description = request.Description;

        _ = _context.Products.Update(product);
        _ = await _context.SaveChangesAsync(cancellationToken);

        return product.MapToDto();
    }
}
