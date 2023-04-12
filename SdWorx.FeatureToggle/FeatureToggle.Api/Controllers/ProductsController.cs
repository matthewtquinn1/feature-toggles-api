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
    private ISender _mediator = null!;
    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();

    private readonly ILogger<ProductsController> _logger;

	public ProductsController(ILogger<ProductsController> logger)
	{
		_logger = logger;
	}

	[HttpGet]
	public async Task<List<Product>> Get()
	{
		return await _mediator.Send(new GetProductsQuery());
	}

    [HttpGet("{id:Guid}")]
    public async Task<Product> GetById(Guid id)
    {
        return await _mediator.Send(new GetProductByIdQuery(id));
    }

    [HttpPost]
    public async Task<Guid> Create(CreateProductCommand command)
    {
        return await _mediator.Send(command);
    }

    [HttpPut("{id:Guid}")]
    public async Task<Product> Update(Guid id, UpdateProductCommand command)
    {
        // TODO: Throw exception if id != command.Id.

        return await _mediator.Send(command);
    }

    [HttpDelete("{id:Guid}")]
    public async Task<Unit> Delete(Guid id)
    {
        return await _mediator.Send(new DeleteProductCommand(id));
    }
}
