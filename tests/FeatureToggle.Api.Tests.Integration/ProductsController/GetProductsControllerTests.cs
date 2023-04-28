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
public sealed class GetProductsControllerTests : IClassFixture<FeatureToggleApiFactory>
{
    private readonly HttpClient _httpClient;

    private readonly Faker<Product> _productGenerator = new Faker<Product>()
        .RuleFor(x => x.Name, f => f.Lorem.Word())
        .RuleFor(x => x.Description, f => f.Lorem.Sentences());

    public GetProductsControllerTests(FeatureToggleApiFactory apiFactory)
    {
        _httpClient = apiFactory.CreateClient();
    }

    [Fact]
    public async Task Get_ReturnAllProducts_WhenProductsExist()
    {
        // Arrange.
        var product = _productGenerator.Generate();
        var createCommand = new CreateProductCommand(product.Name, product.Description);
        _ = await _httpClient.PostAsJsonAsync("api/products", createCommand);

        // Act.
        var response = await _httpClient.GetAsync($"api/products");

        // Assert.
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var productsResponse = await response.Content.ReadFromJsonAsync<List<ProductDto>>();
        productsResponse!.Single().Name.Should().Be(createCommand!.Name);
        productsResponse!.Single().Description.Should().Be(createCommand!.Description);

        // Cleanup.
        await _httpClient.DeleteAsync($"api/products/{productsResponse![0].Id}");
    }

    [Fact]
    public async Task Get_ReturnsEmptyResult_WhenNoProductsExist()
    {
        // Act.
        var response = await _httpClient.GetAsync($"api/products");

        // Assert.
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var productResponse = await response.Content.ReadFromJsonAsync<IEnumerable<ProductDto>>();
        productResponse.Should().BeEmpty();
    }
}
