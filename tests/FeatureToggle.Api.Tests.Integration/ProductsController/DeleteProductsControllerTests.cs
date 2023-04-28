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
public sealed class DeleteProductsControllerTests : IClassFixture<FeatureToggleApiFactory>
{
    private readonly HttpClient _httpClient;

    private readonly Faker<Product> _productGenerator = new Faker<Product>()
        .RuleFor(x => x.Name, f => f.Lorem.Word())
        .RuleFor(x => x.Description, f => f.Lorem.Sentences());

    public DeleteProductsControllerTests(FeatureToggleApiFactory apiFactory)
    {
        _httpClient = apiFactory.CreateClient();
    }

    [Fact]
    public async Task Delete_ReturnsNoContent_WhenProductExists()
    {
        // Arrange.
        var fakeProduct = _productGenerator.Generate();
        var createCommand = new CreateProductCommand(fakeProduct.Name, fakeProduct.Description);
        var product = await (await _httpClient.PostAsJsonAsync("api/products", createCommand))
            .Content.ReadFromJsonAsync<ProductDto>();

        // Act.
        var response = await _httpClient.DeleteAsync($"api/products/{product!.Id}");

        // Assert.
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Delete_ReturnsNotFound_WhenProductDoesNotExists()
    {
        // Arrange.
        var productId = Guid.NewGuid();

        // Act.
        var response = await _httpClient.DeleteAsync($"api/products/{productId}");

        // Assert.
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
