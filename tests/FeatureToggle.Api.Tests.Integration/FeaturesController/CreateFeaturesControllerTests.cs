using Bogus;
using FeatureToggle.Application.Features.Commands;
using FeatureToggle.Domain.Entities;
using FluentAssertions;
using System.Net.Http.Json;
using System.Net;
using Xunit;
using System.Diagnostics.CodeAnalysis;
using FeatureToggle.Application.Products.Commands;

namespace FeatureToggle.Api.Tests.Integration.FeaturesController;

[ExcludeFromCodeCoverage]
public sealed class CreateFeaturesControllerTests : IClassFixture<FeatureToggleApiFactory>
{
    private readonly HttpClient _httpClient;

    private readonly Faker<Feature> _featureGenerator =
        new Faker<Feature>()
            .RuleFor(x => x.Name, f => $"IntegrationTestData{f.Random.Word()}Enabled")
            .RuleFor(x => x.Description, f => f.Lorem.Sentences());

    public CreateFeaturesControllerTests(FeatureToggleApiFactory apiFactory)
    {
        _httpClient = apiFactory.CreateClient();
    }

    [Fact]
    public async Task Create_ReturnsCreated_WhenFeatureIsCreated()
    {
        // Arrange.
        var fakeProduct = new Faker<Product>()
            .RuleFor(x => x.Name, f => f.Lorem.Word())
            .RuleFor(x => x.Description, f => f.Lorem.Sentence())
            .Generate();
        var productCommand = new CreateProductCommand(fakeProduct.Name, fakeProduct.Description);
        var productId = await (await _httpClient.PostAsJsonAsync("api/products", productCommand))
            .Content.ReadFromJsonAsync<Guid>();

        var feature = _featureGenerator.Generate();
        var command = new CreateFeatureCommand(productId, feature.Name, feature.Description);

        // Act.
        var response = await _httpClient.PostAsJsonAsync("api/features", command);

        // Assert.
        response.StatusCode.Should().Be(HttpStatusCode.Created);
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
}