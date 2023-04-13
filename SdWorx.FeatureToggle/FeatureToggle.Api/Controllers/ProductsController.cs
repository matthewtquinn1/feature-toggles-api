using FeatureToggle.Application.Products.Commands;
using FeatureToggle.Application.Products.Queries;
using FeatureToggle.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace FeatureToggle.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : MediatorControllerBase
{
    private readonly ILogger<ProductsController> _logger;

	public ProductsController(ILogger<ProductsController> logger)
	{
		_logger = logger;
	}

	[HttpGet]
    [ProducesResponseType(typeof(List<Product>), StatusCodes.Status200OK)]
    public async Task<List<Product>> Get()
	{
		return await Mediator.Send(new GetProductsQuery());
	}

    [HttpGet("{id:Guid}")]
    [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById(Guid id)
    {
        return Ok(await Mediator.Send(new GetProductByIdQuery(id)));
    }

    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    public async Task<IActionResult> Create(CreateProductCommand command)
    {
        return Ok(await Mediator.Send(command));
    }

    [HttpPut("{id:Guid}")]
    [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
    public async Task<IActionResult> Update(Guid id, UpdateProductCommand command)
    {
        // TODO: Throw exception if id != command.Id.

        return Ok(await Mediator.Send(command));
    }

    [HttpDelete("{id:Guid}")]
    [ProducesResponseType(typeof(NoContentResult), StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(Guid id)
    {
        _ = await Mediator.Send(new DeleteProductCommand(id));

        return NoContent();
    }
}
