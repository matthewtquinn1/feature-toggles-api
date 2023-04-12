using FeatureToggle.Domain.Entities;
using MediatR;

namespace FeatureToggle.Application.Features.Queries;

public sealed record GetFeaturesQuery() : IRequest<List<Feature>>;

public sealed class GetFeaturesQueryHandler : IRequestHandler<GetFeaturesQuery, List<Feature>>
{
	public Task<List<Feature>> Handle(GetFeaturesQuery request, CancellationToken cancellationToken)
	{
		// TODO: Implement.
		return Task.FromResult(new List<Feature>());
	}
}
