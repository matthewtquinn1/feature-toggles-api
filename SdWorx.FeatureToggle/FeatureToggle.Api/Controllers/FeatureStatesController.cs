using FeatureToggle.Application.Features.Commands;
using FeatureToggle.Application.Features.Queries;
using FeatureToggle.Application.FeatureStates.Commands;
using FeatureToggle.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace FeatureToggle.Api.Controllers;

[ApiController]
[Route("api/feature-states")]
public class FeatureStatesController : MediatorControllerBase
{
    private readonly ILogger<FeatureStatesController> _logger;

    public FeatureStatesController(ILogger<FeatureStatesController> logger)
    {
        _logger = logger;
    }

    [HttpPatch("{id:Guid}")]
    [ProducesResponseType(typeof(Feature), StatusCodes.Status200OK)]
    public async Task<IActionResult> ToggleActiveState(Guid id, UpdateFeatureStateCommand command)
    {
        // TODO: Throw exception when id != command.Id.

        return Ok(await Mediator.Send(command));
    }
}
