using FeatureToggle.Application.Features;
using FeatureToggle.Application.Features.Commands;
using FeatureToggle.Application.Features.Queries;
using FeatureToggle.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FeatureToggle.Api.Controllers;

[ApiController]
[Route("api/features")]
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
    [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var feature = await _mediator.Send(new GetFeatureByIdQuery(id));

        return feature == null
            ? NotFound()
            : Ok(feature);
    }

    [HttpPost]
    [ProducesResponseType(typeof(FeatureDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(CreateFeatureCommand command)
    {
        var feature = await _mediator.Send(command);

        return CreatedAtAction(nameof(GetById), new { feature.Id }, feature);
    }

    [HttpPatch("{id:Guid}")]
    [ProducesResponseType(typeof(Feature), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, UpdateFeatureCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest("Cannot update feature; provided ID in URL and command do not match.");
        }

        var updatedFeature = await _mediator.Send(command);

        return updatedFeature == null
            ? NotFound()
            : Ok(updatedFeature);
    }

    [HttpDelete("{id:Guid}")]
    [ProducesResponseType(typeof(NoContentResult), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var unit = await _mediator.Send(new DeleteFeatureCommand(id));

        return unit == null
            ? NotFound()
            : NoContent();
    }
}
