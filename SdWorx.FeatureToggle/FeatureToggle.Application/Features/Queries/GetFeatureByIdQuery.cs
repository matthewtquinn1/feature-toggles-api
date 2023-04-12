using FeatureToggle.Domain.Entities;
using MediatR;

namespace FeatureToggle.Application.Features.Queries;

public sealed record GetFeatureByIdQuery(Guid id) : IRequest<Feature>;

public sealed class GetFeatureByIdQueryHandler : IRequestHandler<GetFeatureByIdQuery, Feature>
{
	public Task<Feature> Handle(GetFeatureByIdQuery request, CancellationToken cancellationToken)
	{
		// TODO: Implement.
		return Task.FromResult(new Feature());
	}
}
