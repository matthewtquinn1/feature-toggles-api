using Bogus;
using FeatureToggle.Application.Features.Commands;
using FeatureToggle.Application.Products.Commands;
using FeatureToggle.Application.Products;
using FeatureToggle.Domain.Entities;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using Xunit;
using FeatureToggle.Application.Features;
using Microsoft.AspNetCore.Mvc;

namespace FeatureToggle.Api.Tests.Integration.FeaturesController;

public sealed class UpdateFeaturesControllerTests : IClassFixture<FeatureToggleApiFactory>
{
    private readonly HttpClient _httpClient;

    private readonly Faker<Product> _productGenerator = new Faker<Product>()
        .RuleFor(x => x.Name, f => $"{f.Lorem.Word()}{f.Lorem.Word()}")
        .RuleFor(x => x.Description, f => f.Lorem.Sentences());

    private readonly Faker<Feature> _featureGenerator =
        new Faker<Feature>()
            .RuleFor(x => x.Name, f => $"{f.Random.Word()}{f.Random.Word()}Enabled")
            .RuleFor(x => x.Description, f => f.Lorem.Sentences());

    public UpdateFeaturesControllerTests(FeatureToggleApiFactory apiFactory)
    {
        _httpClient = apiFactory.CreateClient();
    }

    [Fact]
    public async Task Update_UpdatesFeature_WhenRequestIsValid()
    {
        // Arrange.
        var fakeProduct = _productGenerator.Generate();
        var createProductCommand = new CreateProductCommand(fakeProduct.Name, fakeProduct.Description);
        var product = await (await _httpClient.PostAsJsonAsync("api/products", createProductCommand))
            .Content.ReadFromJsonAsync<ProductDto>();

        var fakeFeature = _featureGenerator.Generate();
        var createFeatureCommand = new CreateFeatureCommand(product!.Id, fakeFeature.Name, fakeFeature.Description);
        var feature = await (await _httpClient.PostAsJsonAsync("api/features", createFeatureCommand))
            .Content.ReadFromJsonAsync<FeatureDto>();

        var updateFeatureCommand = new UpdateFeatureCommand(feature!.Id, "A new description");

        // Act.
        var response = await _httpClient.PatchAsync($"api/features/{feature!.Id}", JsonContent.Create(updateFeatureCommand));

        // Assert.
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var updatedFeature = await response.Content.ReadFromJsonAsync<FeatureDto>();
        updatedFeature!.Description.Should().Be(updateFeatureCommand.Description);
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

        var fakeFeature = _featureGenerator.Generate();
        var createFeatureCommand = new CreateFeatureCommand(product!.Id, fakeFeature.Name, fakeFeature.Description);
        var feature = await (await _httpClient.PostAsJsonAsync("api/features", createFeatureCommand))
            .Content.ReadFromJsonAsync<FeatureDto>();

        var updateFeatureCommand = new UpdateFeatureCommand(feature!.Id, description);

        // Act.
        var response = await _httpClient.PatchAsync($"api/features/{feature!.Id}", JsonContent.Create(updateFeatureCommand));

        // Assert.
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var error = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
        error!.Status.Should().Be(400);
        error!.Title.Should().Be("One or more validation errors occurred.");
        error!.Errors[nameof(feature.Description)][0].Should().Be("The Description field is required.");
    }

    [Fact]
    public async Task Update_ReturnsNotFound_WhenFeatureDoesNotExist()
    {
        // Arrange.
        var id = Guid.NewGuid();

        var updateFeatureCommand = new UpdateFeatureCommand(id, "New description");

        // Act.
        var response = await _httpClient.PatchAsync($"api/features/{id}", JsonContent.Create(updateFeatureCommand));

        // Assert.
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}