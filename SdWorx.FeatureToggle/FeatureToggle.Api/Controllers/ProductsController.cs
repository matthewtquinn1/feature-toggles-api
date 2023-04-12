using FeatureToggle.Application.Products.Commands;
using FeatureToggle.Application.Products.Queries;
using FeatureToggle.Domain.Entities;
using MediatR;
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
	public async Task<List<Product>> Get()
	{
		return await Mediator.Send(new GetProductsQuery());
	}

    [HttpGet("{id:Guid}")]
    public async Task<Product> GetById(Guid id)
    {
        return await Mediator.Send(new GetProductByIdQuery(id));
    }

    [HttpPost]
    public async Task<Guid> Create(CreateProductCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpPut("{id:Guid}")]
    public async Task<Product> Update(Guid id, UpdateProductCommand command)
    {
        // TODO: Throw exception if id != command.Id.

        return await Mediator.Send(command);
    }

    [HttpDelete("{id:Guid}")]
    public async Task<Unit> Delete(Guid id)
    {
        return await Mediator.Send(new DeleteProductCommand(id));
    }
}
