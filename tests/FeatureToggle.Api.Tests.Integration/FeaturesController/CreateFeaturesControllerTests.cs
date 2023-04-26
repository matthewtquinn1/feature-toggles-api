using Bogus;
using FeatureToggle.Application.Features.Commands;
using FeatureToggle.Domain.Entities;
using FluentAssertions;
using System.Net.Http.Json;
using System.Net;
using Xunit;
using System.Diagnostics.CodeAnalysis;
using FeatureToggle.Application.Products.Commands;
using FeatureToggle.Application.Features;
using Microsoft.AspNetCore.Mvc;

namespace FeatureToggle.Api.Tests.Integration.FeaturesController;

[ExcludeFromCodeCoverage]
public sealed class CreateFeaturesControllerTests : IClassFixture<FeatureToggleApiFactory>
{
    private readonly HttpClient _httpClient;

    private readonly Faker<Product> _productGenerator = new Faker<Product>()
    .RuleFor(x => x.Name, f => f.Lorem.Word())
    .RuleFor(x => x.Description, f => f.Lorem.Sentences());

    private readonly Faker<Feature> _featureGenerator =
        new Faker<Feature>()
            .RuleFor(x => x.Name, f => $"IntegrationTestData{f.Random.Word()}Enabled")
            .RuleFor(x => x.Description, f => f.Lorem.Sentences());

    public CreateFeaturesControllerTests(FeatureToggleApiFactory apiFactory)
    {
        _httpClient = apiFactory.CreateClient();
    }

    [Fact]
    public async Task Create_CreatesFeature_WhenRequestIsValid()
    {
        // Arrange.
        var fakeProduct = _productGenerator.Generate();
        var productCommand = new CreateProductCommand(fakeProduct.Name, fakeProduct.Description);
        var productId = await (await _httpClient.PostAsJsonAsync("api/products", productCommand))
            .Content.ReadFromJsonAsync<Guid>();

        var feature = _featureGenerator.Generate();
        var command = new CreateFeatureCommand(productId, feature.Name, feature.Description);

        // Act.
        var response = await _httpClient.PostAsJsonAsync("api/features", command);

        // Assert.
        var newFeature = await response.Content.ReadFromJsonAsync<FeatureDto>();
        newFeature!.Name.Should().Be(command.Name);
        newFeature!.Description.Should().Be(command.Description);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        response.Headers.Location!.ToString().Should().Be($"http://localhost/api/features/{newFeature!.Id}");
    }

    [Fact]
    public async Task Create_ReturnsInternalServerError_WhenProductDoesNotExist()
    {
        // Arrange.
        var productId = Guid.NewGuid();

        var feature = _featureGenerator.Generate();
        var command = new CreateFeatureCommand(productId, feature.Name, feature.Description);

        // Act.
        var response = await _httpClient.PostAsJsonAsync("api/features", command);

        // Assert.
        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);        
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public async Task Create_ReturnsValidationError_WhenNameIsInvalid(string name)
    {
        // Arrange.
        var fakeProduct = _productGenerator.Generate();
        var productCommand = new CreateProductCommand(fakeProduct.Name, fakeProduct.Description);
        var productId = await (await _httpClient.PostAsJsonAsync("api/products", productCommand))
            .Content.ReadFromJsonAsync<Guid>();

        var feature = _featureGenerator.Clone().RuleFor(x => x.Name, name).Generate();
        var command = new CreateFeatureCommand(productId, feature.Name, feature.Description);

        // Act.
        var response = await _httpClient.PostAsJsonAsync("api/features", command);

        // Assert.
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var error = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
        error!.Status.Should().Be(400);
        error!.Title.Should().Be("One or more validation errors occurred.");
        error!.Errors[nameof(feature.Name)][0].Should().Be("The Name field is required.");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public async Task Create_ReturnsValidationError_WhenDescriptionIsInvalid(string description)
    {
        // Arrange.
        var fakeProduct = _productGenerator.Generate();
        var productCommand = new CreateProductCommand(fakeProduct.Name, fakeProduct.Description);
        var productId = await (await _httpClient.PostAsJsonAsync("api/products", productCommand))
            .Content.ReadFromJsonAsync<Guid>();

        var feature = _featureGenerator.Clone().RuleFor(x => x.Description, description).Generate();
        var command = new CreateFeatureCommand(productId, feature.Name, feature.Description);

        // Act.
        var response = await _httpClient.PostAsJsonAsync("api/features", command);

        // Assert.
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var error = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
        error!.Status.Should().Be(400);
        error!.Title.Should().Be("One or more validation errors occurred.");
        error!.Errors[nameof(feature.Description)][0].Should().Be("The Description field is required.");
    }
}