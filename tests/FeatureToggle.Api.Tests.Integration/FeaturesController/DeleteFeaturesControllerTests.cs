using FeatureToggle.Application.Features;
using FluentAssertions;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Json;
using System.Net;
using Xunit;
using FeatureToggle.Application.Features.Commands;
using FeatureToggle.Application.Products.Commands;
using FeatureToggle.Application.Products;
using Bogus;
using FeatureToggle.Domain.Entities;

namespace FeatureToggle.Api.Tests.Integration.FeaturesController;

[ExcludeFromCodeCoverage]
public sealed class DeleteFeaturesControllerTests : IClassFixture<FeatureToggleApiFactory>
{
    private readonly HttpClient _httpClient;

    private readonly Faker<Product> _productGenerator = new Faker<Product>()
        .RuleFor(x => x.Name, f => $"{f.Lorem.Word()}{f.Lorem.Word()}")
        .RuleFor(x => x.Description, f => f.Lorem.Sentences());

    private readonly Faker<Feature> _featureGenerator =
        new Faker<Feature>()
            .RuleFor(x => x.Name, f => $"{f.Random.Word()}{f.Random.Word()}Enabled")
            .RuleFor(x => x.Description, f => f.Lorem.Sentences());

    public DeleteFeaturesControllerTests(FeatureToggleApiFactory apiFactory)
    {
        _httpClient = apiFactory.CreateClient();
    }

    [Fact]
    public async Task Delete_ReturnsNoContent_WhenFeatureExists()
    {
        // Arrange.
        var fakeProduct = _productGenerator.Generate();
        var productCommand = new CreateProductCommand(fakeProduct.Name, fakeProduct.Description);
        var product = await (await _httpClient.PostAsJsonAsync("api/products", productCommand))
            .Content.ReadFromJsonAsync<ProductDto>();

        var fakeFeature = _featureGenerator.Generate();
        var command = new CreateFeatureCommand(product!.Id, fakeFeature.Name, fakeFeature.Description);
        var feature = await (await _httpClient.PostAsJsonAsync("api/features", command))
            .Content.ReadFromJsonAsync<FeatureDto>();

        // Act.
        var response = await _httpClient.DeleteAsync($"api/features/{feature!.Id}");

        // Assert.
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Delete_ReturnsNotFound_WhenFeatureDoesNotExists()
    {
        // Arrange.
        var id = Guid.NewGuid();

        // Act.
        var response = await _httpClient.DeleteAsync($"api/features/{id}");

        // Assert.
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
