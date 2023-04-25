using FeatureToggle.Application.Features.Commands;
using FeatureToggle.Application.Features.Queries;
using FeatureToggle.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FeatureToggle.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FeaturesController : ControllerBase
{
    private readonly ILogger<FeaturesController> _logger;
    private readonly IMediator _mediator;

    public FeaturesController(
        ILogger<FeaturesController> logger,
        IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Feature>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get()
    {
        return Ok(await _mediator.Send(new GetFeaturesQuery()));
    }

    [HttpGet("{id:Guid}")]
    [ProducesResponseType(typeof(Feature), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var feature = await _mediator.Send(new GetFeatureByIdQuery(id));

        return feature == null
            ? NotFound()
            : Ok(feature);
    }

    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    public async Task<IActionResult> Create(CreateFeatureCommand command)
    {
        return CreatedAtAction(nameof(Create), await _mediator.Send(command));
    }

    [HttpPatch("{id:Guid}")]
    [ProducesResponseType(typeof(Feature), StatusCodes.Status200OK)]
    public async Task<IActionResult> Update(Guid id, UpdateFeatureCommand command)
    {
        // TODO: Throw exception when id != command.Id.

        return Ok(await _mediator.Send(command));
    }

    [HttpDelete("{id:Guid}")]
    [ProducesResponseType(typeof(NoContentResult), StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(Guid id)
    {
        _ = await _mediator.Send(new DeleteFeatureCommand(id));

        return NoContent();
    }
}
