using FeatureToggle.Application.FeatureStates.Commands;
using FeatureToggle.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FeatureToggle.Api.Controllers;

[ApiController]
[Route("api/feature-states")]
public class FeatureStatesController : ControllerBase
{
    private readonly ILogger<FeatureStatesController> _logger;
    private readonly IMediator _mediator;

    public FeatureStatesController(
        ILogger<FeatureStatesController> logger,
        IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpPatch("{id:Guid}")]
    [ProducesResponseType(typeof(Feature), StatusCodes.Status200OK)]
    public async Task<IActionResult> ToggleActiveState(Guid id, UpdateFeatureStateCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest("Cannot update feature state; provided ID in URL and command do not match.");
        }

        var featureState = await _mediator.Send(command);

        return featureState == null
            ? NotFound()
            : Ok(featureState);
    }
}
