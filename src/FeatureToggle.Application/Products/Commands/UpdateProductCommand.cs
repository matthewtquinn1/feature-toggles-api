using FeatureToggle.Application.Common.Exceptions;
using FeatureToggle.Application.Common.Interfaces;
using FeatureToggle.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace FeatureToggle.Application.Products.Commands;

public sealed record UpdateProductCommand(
    Guid Id, 
    [Required][MaxLength(250)] string Name,
    [Required][MaxLength(500)] string Description) : IRequest<ProductDto?>;

public sealed class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, ProductDto?>
{
    private readonly IApplicationDbContext _context;

    public UpdateProductCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ProductDto?> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (product == null)
        {
            return null;
        }

        var existingProduct = await _context.Products
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Name == request.Name && p.Id != request.Id, cancellationToken);

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
