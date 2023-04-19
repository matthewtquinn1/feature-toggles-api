using FeatureToggle.Application.Products.Commands;
using FeatureToggle.Application.Products.Queries;
using FeatureToggle.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FeatureToggle.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly ILogger<ProductsController> _logger;
    private readonly IMediator _mediator;

    public ProductsController(
        ILogger<ProductsController> logger,
        IMediator mediator)
	{
		_logger = logger;
        _mediator = mediator;
    }

	[HttpGet]
    [ProducesResponseType(typeof(List<Product>), StatusCodes.Status200OK)]
    public async Task<List<Product>> Get()
	{
		return await _mediator.Send(new GetProductsQuery());
	}

    [HttpGet("{id:Guid}")]
    [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var product = await _mediator.Send(new GetProductByIdQuery(id));

        return product == null
            ? NotFound()
            : Ok(product);
    }

    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    public async Task<IActionResult> Create(CreateProductCommand command)
    {
        return Ok(await _mediator.Send(command));
    }

    [HttpPut("{id:Guid}")]
    [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
    public async Task<IActionResult> Update(Guid id, UpdateProductCommand command)
    {
        // TODO: Throw exception if id != command.Id.

        return Ok(await _mediator.Send(command));
    }

    [HttpDelete("{id:Guid}")]
    [ProducesResponseType(typeof(NoContentResult), StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(Guid id)
    {
        _ = await _mediator.Send(new DeleteProductCommand(id));

        return NoContent();
    }
}
