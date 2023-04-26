using FeatureToggle.Application.Products;
using FeatureToggle.Application.Products.Commands;
using FeatureToggle.Application.Products.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FeatureToggle.Api.Controllers;

[ApiController]
[Route("api/products")]
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
    [ProducesResponseType(typeof(IEnumerable<ProductDto>), StatusCodes.Status200OK)]
    public async Task<IEnumerable<ProductDto>> Get()
	{
		return await _mediator.Send(new GetProductsQuery());
	}

    [HttpGet("{id:Guid}")]
    [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var product = await _mediator.Send(new GetProductByIdQuery(id));

        return product == null
            ? NotFound()
            : Ok(product);
    }

    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(CreateProductCommand command)
    {
        var product = await _mediator.Send(command);

        return CreatedAtAction(nameof(GetById), new { product.Id }, product);
    }

    [HttpPut("{id:Guid}")]
    [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
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
