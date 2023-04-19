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
        // TODO: Throw exception when id != command.Id.

        return Ok(await _mediator.Send(command));
    }
}
