using FeatureToggle.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FeatureToggle.Application.Products.Commands;

public sealed record DeleteProductCommand(Guid Id) : IRequest<Unit?>;

public sealed class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, Unit?>
{
    private readonly IApplicationDbContext _context;

    public DeleteProductCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit?> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (product == null)
        {
            return null;
        }

        _ = _context.Products.Remove(product);
        _ = await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
