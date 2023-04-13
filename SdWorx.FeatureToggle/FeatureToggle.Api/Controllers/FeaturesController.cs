using FeatureToggle.Application.Features.Commands;
using FeatureToggle.Application.Features.Queries;
using FeatureToggle.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace FeatureToggle.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FeaturesController : MediatorControllerBase
{
    private readonly ILogger<FeaturesController> _logger;

    public FeaturesController(ILogger<FeaturesController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<Feature>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get()
    {
        return Ok(await Mediator.Send(new GetFeaturesQuery()));
    }

    [HttpGet("{id:Guid}")]
    [ProducesResponseType(typeof(Feature), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById(Guid id)
    {
        return Ok(await Mediator.Send(new GetFeatureByIdQuery(id)));
    }

    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    public async Task<IActionResult> Create(CreateFeatureCommand command)
    {
        return Ok(await Mediator.Send(command));
    }

    // TODO: This needs reworked.
    [HttpPut("{id:Guid}")]
    [ProducesResponseType(typeof(Feature), StatusCodes.Status200OK)]
    public async Task<IActionResult> Update(Guid id, UpdateFeatureCommand command)
    {
        // TODO: Throw exception when id != command.Id.

        return Ok(await Mediator.Send(command));
    }

    // TODO: This needs reworked.
    [HttpPatch("{id:Guid}")]
    [ProducesResponseType(typeof(Feature), StatusCodes.Status200OK)]
    public async Task<IActionResult> ToggleActiveState(Guid id, ToggleFeatureStatusCommand command)
    {
        // TODO: Throw exception when id != command.Id.

        return Ok(await Mediator.Send(command));
    }

    [HttpDelete("{id:Guid}")]
    [ProducesResponseType(typeof(NoContentResult), StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(Guid id)
    {
        _ = await Mediator.Send(new DeleteFeatureCommand(id));

        return NoContent();
    }
}
