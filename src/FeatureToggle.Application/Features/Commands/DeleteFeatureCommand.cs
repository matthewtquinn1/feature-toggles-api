using FeatureToggle.Application.Common.Exceptions;
using FeatureToggle.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FeatureToggle.Application.Features.Commands;

public sealed record DeleteFeatureCommand(Guid Id) : IRequest<Unit?>;

public sealed class DeleteFeatureCommandHandler : IRequestHandler<DeleteFeatureCommand, Unit?>
{
    private readonly IApplicationDbContext _context;

    public DeleteFeatureCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit?> Handle(DeleteFeatureCommand request, CancellationToken cancellationToken)
    {
        var feature = await _context.Features.FirstOrDefaultAsync(f => f.Id == request.Id, cancellationToken);

        if (feature == null)
        {
            return null;
        }

        _ = _context.Features.Remove(feature);
        _ = await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
