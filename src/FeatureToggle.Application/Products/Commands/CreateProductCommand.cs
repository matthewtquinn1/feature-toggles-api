using FeatureToggle.Application.Common.Exceptions;
using FeatureToggle.Application.Common.Interfaces;
using FeatureToggle.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace FeatureToggle.Application.Products.Commands;

public sealed record CreateProductCommand(
    [Required][MaxLength(250)] string Name,
    [Required][MaxLength(500)] string Description) : IRequest<ProductDto>;

public sealed class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ProductDto>
{
    private readonly IApplicationDbContext _context;

    public CreateProductCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ProductDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var existingProduct = await _context.Products
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Name == request.Name, cancellationToken);

        if (existingProduct != null)
        {
            throw new DuplicateFoundException(nameof(Product), nameof(request.Name), request.Name);
        }

        var product = new Product { Name = request.Name, Description = request.Description };

        _ = await _context.Products.AddAsync(product, cancellationToken);
        _ = await _context.SaveChangesAsync(cancellationToken);

        return product.MapToDto();
    }
}
