using Bogus;
using FeatureToggle.Application.Products;
using FeatureToggle.Application.Products.Commands;
using FeatureToggle.Domain.Entities;
using FluentAssertions;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace FeatureToggle.Api.Tests.Integration.ProductsController;

[ExcludeFromCodeCoverage]
public sealed class GetByIdProductsControllerTests : IClassFixture<FeatureToggleApiFactory>
{
    private readonly HttpClient _httpClient;

    private readonly Faker<Product> _productGenerator = new Faker<Product>()
        .RuleFor(x => x.Name, f => f.Lorem.Word())
        .RuleFor(x => x.Description, f => f.Lorem.Sentences());

    public GetByIdProductsControllerTests(FeatureToggleApiFactory apiFactory)
    {
        _httpClient = apiFactory.CreateClient();
    }

    [Fact]
    public async Task GetById_ReturnsProduct_WhenProductExists()
    {
        // Arrange.
        var product = _productGenerator.Generate();
        var createCommand = new CreateProductCommand(product.Name, product.Description);
        var createdProduct = await (await _httpClient.PostAsJsonAsync("api/products", createCommand))
            .Content.ReadFromJsonAsync<ProductDto>();

        // Act.
        var response = await _httpClient.GetAsync($"api/products/{createdProduct!.Id}");

        // Assert.
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var retrievedProduct = await response.Content.ReadFromJsonAsync<ProductDto>();
        retrievedProduct!.Name.Should().Be(createdProduct.Name);
        retrievedProduct!.Description.Should().Be(createdProduct.Description);
    }

    [Fact]
    public async Task GetById_ReturnsNotFound_WhenProductDoesNotExist()
    {
        // Act.
        var response = await _httpClient.GetAsync($"api/products/{Guid.NewGuid()}");

        // Assert.
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
