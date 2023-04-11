using FeatureToggle.Domain.Entities;
using MediatR;

namespace FeatureToggle.Application.FeatureToggles.Queries;

public record GetFeaturesByIdQuery(Guid id) : IRequest<Feature>;

public sealed class GetFeaturesByIdQueryHandler : IRequestHandler<GetFeaturesByIdQuery, Feature>
{
	public Task<Feature> Handle(GetFeaturesByIdQuery request, CancellationToken cancellationToken)
	{
		// TODO: Implement.
		return Task.FromResult(new Feature());
	}
}
