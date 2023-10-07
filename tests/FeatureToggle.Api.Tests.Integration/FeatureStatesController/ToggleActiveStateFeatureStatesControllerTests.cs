using FeatureToggle.Application.Features.Commands;
using FeatureToggle.Application.Features;
using FeatureToggle.Application.Products.Commands;
using FeatureToggle.Application.Products;
using System.Net.Http.Json;
using Xunit;
using System.Diagnostics.CodeAnalysis;
using Bogus;
using FeatureToggle.Domain.Entities;
using FeatureToggle.Application.FeatureStates.Commands;
using FeatureToggle.Application.FeatureStates;
using FluentAssertions;
using System.Net;

namespace FeatureToggle.Api.Tests.Integration.FeatureStatesController;

[ExcludeFromCodeCoverage]
public sealed class ToggleActiveStateFeatureStatesControllerTests : IClassFixture<FeatureToggleApiFactory>
{
    private readonly HttpClient _httpClient;

    private readonly Faker<Product> _productGenerator = new Faker<Product>()
        .RuleFor(x => x.Name, f => $"{f.Lorem.Word()}{f.Lorem.Word()}")
        .RuleFor(x => x.Description, f => f.Lorem.Sentences());

    private readonly Faker<Feature> _featureGenerator =
        new Faker<Feature>()
            .RuleFor(x => x.Name, f => $"{f.Random.Word()}{f.Random.Word()}Enabled")
            .RuleFor(x => x.Description, f => f.Lorem.Sentences());

    public ToggleActiveStateFeatureStatesControllerTests(FeatureToggleApiFactory featureToggleApiFactory)
	{
		_httpClient = featureToggleApiFactory.CreateClient();
	}

    [Fact]
	public async Task ToggleActiveStatus_ReturnFeatureState_WhenRequestIsValid()
	{
        // Arrange.
        var fakeProduct = _productGenerator.Generate();
        var createProductCommand = new CreateProductCommand(fakeProduct.Name, fakeProduct.Description);
        var product = await(await _httpClient.PostAsJsonAsync("api/products", createProductCommand))
            .Content.ReadFromJsonAsync<ProductDto>();

        var fakeFeature = _featureGenerator.Generate();
        var createFeatureCommand = new CreateFeatureCommand(product!.Id, fakeFeature.Name, fakeFeature.Description);
        var feature = await(await _httpClient.PostAsJsonAsync("api/features", createFeatureCommand))
            .Content.ReadFromJsonAsync<FeatureDto>();

        var featureState = feature!.FeatureStates.First();

        var updateFeatureStateCommand = new UpdateFeatureStateCommand(featureState.Id, true);

        // Act.
        var response = await _httpClient.PatchAsync($"api/feature-states/{featureState.Id}", JsonContent.Create(updateFeatureStateCommand));

        // Assert.
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var updatedFeatureState = await response.Content.ReadFromJsonAsync<FeatureStateDto>();
        featureState!.IsActive.Should().BeFalse();
        updatedFeatureState!.IsActive.Should().BeTrue();
    }

    [Fact]
    public async Task ToggleActiveStatus_ReturnNotFound_WhenFeatureStateDoesNotExist()
    {
        // Arrange.
        var fakeProduct = _productGenerator.Generate();
        var createProductCommand = new CreateProductCommand(fakeProduct.Name, fakeProduct.Description);
        var product = await (await _httpClient.PostAsJsonAsync("api/products", createProductCommand))
            .Content.ReadFromJsonAsync<ProductDto>();

        var fakeFeature = _featureGenerator.Generate();
        var createFeatureCommand = new CreateFeatureCommand(product!.Id, fakeFeature.Name, fakeFeature.Description);
        _ = await _httpClient.PostAsJsonAsync("api/features", createFeatureCommand);

        var id = Guid.NewGuid();

        var updateFeatureStateCommand = new UpdateFeatureStateCommand(id, true);

        // Act.
        var response = await _httpClient.PatchAsync($"api/feature-states/{id}", JsonContent.Create(updateFeatureStateCommand));

        // Assert.
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task ToggleActiveStatus_ReturnsBadRequest_WhenIdsDoNotMatch()
    {
        // Arrange.
        var updateFeatureStateCommand = new UpdateFeatureStateCommand(Guid.NewGuid(), true);

        // Act.
        var response = await _httpClient.PatchAsync($"api/feature-states/{Guid.NewGuid()}", JsonContent.Create(updateFeatureStateCommand));

        // Assert.
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
