using FeatureToggle.Application.FeatureToggles.Commands;
using FeatureToggle.Application.FeatureToggles.Queries;
using FeatureToggle.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FeatureToggle.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FeaturesController : ControllerBase
{
    private ISender _mediator = null!;
    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();

    private readonly ILogger<FeaturesController> _logger;

    public FeaturesController(ILogger<FeaturesController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public async Task<List<Feature>> Get()
    {
        return await Mediator.Send(new GetFeaturesQuery());
    }

    [HttpGet("{id:Guid}")]
    public async Task<Feature> GetById(Guid id)
    {
        return await Mediator.Send(new GetFeaturesByIdQuery(id));
    }

    [HttpPost]
    public async Task<Guid> Create(CreateFeatureCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpPut("{id:Guid}")]
    public async Task<Feature> Update(Guid id, UpdateFeatureCommand command)
    {
        // TODO: Throw exception when id != command.Id.

        return await Mediator.Send(command);
    }

    [HttpPatch("{id:Guid}")]
    public async Task<Feature> ToggleActiveState(Guid id, ToggleFeatureStatusCommand command)
    {
        // TODO: Throw exception when id != command.Id.

        return await Mediator.Send(command);
    }

    [HttpDelete("{id:Guid}")]
    public async Task<Unit> Delete(Guid id)
    {
        return await Mediator.Send(new DeleteFeatureCommand(id));
    }
}
