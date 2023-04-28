using Bogus;
using FeatureToggle.Domain.Entities;
using FluentAssertions;
using System.Net.Http.Json;
using System.Net;
using Xunit;
using System.Diagnostics.CodeAnalysis;
using FeatureToggle.Application.Products.Commands;
using Microsoft.AspNetCore.Mvc;
using FeatureToggle.Application.Products;

namespace FeatureToggle.Api.Tests.Integration.ProductsController;

[ExcludeFromCodeCoverage]
public sealed class CreateProductsControllerTests : IClassFixture<FeatureToggleApiFactory>
{
    private readonly HttpClient _httpClient;

    private readonly Faker<Product> _productGenerator = new Faker<Product>()
        .RuleFor(x => x.Name, f => f.Lorem.Word())
        .RuleFor(x => x.Description, f => f.Lorem.Sentences());

    public CreateProductsControllerTests(FeatureToggleApiFactory apiFactory)
    {
        _httpClient = apiFactory.CreateClient();
    }

    [Fact]
    public async Task Create_CreatesProduct_WhenRequestIsValid()
    {
        // Arrange.
        var fakeProduct = _productGenerator.Generate();
        var command = new CreateProductCommand(fakeProduct.Name, fakeProduct.Description);

        // Act.
        var response = await _httpClient.PostAsJsonAsync("api/products", command);

        // Assert.
        var newProduct = await response.Content.ReadFromJsonAsync<ProductDto>();
        newProduct!.Name.Should().Be(command.Name);
        newProduct!.Description.Should().Be(command.Description);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        response.Headers.Location!.ToString().Should().Be($"http://localhost/api/products/{newProduct!.Id}");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public async Task Create_ReturnsValidationError_WhenNameIsInvalid(string name)
    {
        // Arrange.
        var fakeProduct = _productGenerator.Clone().RuleFor(x => x.Name, name).Generate();
        var command = new CreateProductCommand(fakeProduct.Name, fakeProduct.Description);

        // Act.
        var response = await _httpClient.PostAsJsonAsync("api/products", command);

        // Assert.
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var error = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
        error!.Status.Should().Be(400);
        error!.Title.Should().Be("One or more validation errors occurred.");
        error!.Errors[nameof(command.Name)][0].Should().Be("The Name field is required.");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public async Task Create_ReturnsValidationError_WhenDescriptionIsInvalid(string description)
    {
        // Arrange.
        var fakeProduct = _productGenerator.Clone().RuleFor(x => x.Description, description).Generate();
        var command = new CreateProductCommand(fakeProduct.Name, fakeProduct.Description);

        // Act.
        var response = await _httpClient.PostAsJsonAsync("api/products", command);

        // Assert.
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var error = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
        error!.Status.Should().Be(400);
        error!.Title.Should().Be("One or more validation errors occurred.");
        error!.Errors[nameof(command.Description)][0].Should().Be("The Description field is required.");
    }
}