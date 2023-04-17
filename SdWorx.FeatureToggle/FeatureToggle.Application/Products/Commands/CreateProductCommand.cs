﻿using FeatureToggle.Application.Common.Exceptions;
using FeatureToggle.Application.Common.Interfaces;
using FeatureToggle.Application.Features.Commands;
using FeatureToggle.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FeatureToggle.Application.Products.Commands;

public sealed record CreateProductCommand(string Name, string Description) : IRequest<Guid>;

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

        var existingProduct = await _context.Products
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Name == request.Name, cancellationToken);

        if (existingProduct != null)
        {
            throw new Exception(string.Format("Product with the name {0} already exists.", request.Name));
        }

        var product = new Product { Name = request.Name, Description = request.Description };

        _ = await _context.Products.AddAsync(product, cancellationToken);
        _ = await _context.SaveChangesAsync(cancellationToken);

        return product.Id;
    }
}
