using FeatureToggle.Application.Products.Commands;
using FeatureToggle.Application.Products;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using Xunit;
using Bogus;
using FeatureToggle.Domain.Entities;
using FeatureToggle.Application.Features;
using Microsoft.AspNetCore.Mvc;

namespace FeatureToggle.Api.Tests.Integration.ProductsController;

public sealed class UpdateProductsControllerTests : IClassFixture<FeatureToggleApiFactory>
{
    private readonly HttpClient _httpClient;

    private readonly Faker<Product> _productGenerator = new Faker<Product>()
        .RuleFor(x => x.Name, f => $"{f.Lorem.Word()}{f.Lorem.Word()}")
        .RuleFor(x => x.Description, f => f.Lorem.Sentences());

    public UpdateProductsControllerTests(FeatureToggleApiFactory apiFactory)
    {
        _httpClient = apiFactory.CreateClient();
    }

    [Fact]
    public async Task Update_UpdatesProduct_WhenRequestIsValid()
    {
        // Arrange.
        var fakeProduct = _productGenerator.Generate();
        var createProductCommand = new CreateProductCommand(fakeProduct.Name, fakeProduct.Description);
        var product = await (await _httpClient.PostAsJsonAsync("api/products", createProductCommand))
            .Content.ReadFromJsonAsync<ProductDto>();

        var updateProductCommand = new UpdateProductCommand(product!.Id, "A new name", "A new description");

        // Act.
        var response = await _httpClient.PutAsJsonAsync($"api/products/{product!.Id}", updateProductCommand);

        // Assert.
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var updatedProduct = await response.Content.ReadFromJsonAsync<FeatureDto>();
        updatedProduct!.Name.Should().Be(updateProductCommand.Name);
        updatedProduct!.Description.Should().Be(updateProductCommand.Description);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public async Task Update_ReturnsValidationError_WhenNameIsInvalid(string name)
    {
        // Arrange.
        var fakeProduct = _productGenerator.Generate();
        var createProductCommand = new CreateProductCommand(fakeProduct.Name, fakeProduct.Description);
        var product = await (await _httpClient.PostAsJsonAsync("api/products", createProductCommand))
            .Content.ReadFromJsonAsync<ProductDto>();

        var updateProductCommand = new UpdateProductCommand(product!.Id, name, "A new description");

        // Act.
        var response = await _httpClient.PutAsJsonAsync($"api/products/{product!.Id}", updateProductCommand);

        // Assert.
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var error = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
        error!.Status.Should().Be(400);
        error!.Title.Should().Be("One or more validation errors occurred.");
        error!.Errors[nameof(product.Name)][0].Should().Be("The Name field is required.");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public async Task Update_ReturnsValidationError_WhenDescriptionIsInvalid(string description)
    {
        // Arrange.
        var fakeProduct = _productGenerator.Generate();
        var createProductCommand = new CreateProductCommand(fakeProduct.Name, fakeProduct.Description);
        var product = await (await _httpClient.PostAsJsonAsync("api/products", createProductCommand))
            .Content.ReadFromJsonAsync<ProductDto>();

        var updateProductCommand = new UpdateProductCommand(product!.Id, "A new name", description);

        // Act.
        var response = await _httpClient.PutAsJsonAsync($"api/products/{product!.Id}", updateProductCommand);

        // Assert.
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var error = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
        error!.Status.Should().Be(400);
        error!.Title.Should().Be("One or more validation errors occurred.");
        error!.Errors[nameof(product.Description)][0].Should().Be("The Description field is required.");
    }

    [Fact]
    public async Task Update_ReturnsNotFound_WhenProductDoesNotExist()
    {
        // Arrange.
        var id = Guid.NewGuid();
        var updateProductCommand = new UpdateProductCommand(id, "New name", "New description");

        // Act.
        var response = await _httpClient.PutAsJsonAsync($"api/products/{id}", updateProductCommand);

        // Assert.
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Update_ReturnsBadRequest_WhenIdsDoNotMatch()
    {
        // Arrange.
        var updateProductCommand = new UpdateProductCommand(Guid.NewGuid(), "New name", "New description");

        // Act.
        var response = await _httpClient.PutAsJsonAsync($"api/products/{Guid.NewGuid()}", updateProductCommand);

        // Assert.
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}