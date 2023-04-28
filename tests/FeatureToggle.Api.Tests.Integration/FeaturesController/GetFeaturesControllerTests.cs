using FeatureToggle.Application.Features;
using FeatureToggle.Application.Features.Commands;
using FeatureToggle.Application.Products.Commands;
using FeatureToggle.Application.Products;
using FluentAssertions;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http.Json;
using Xunit;
using Bogus;
using FeatureToggle.Domain.Entities;

namespace FeatureToggle.Api.Tests.Integration.FeaturesController;

[ExcludeFromCodeCoverage]
public sealed class GetFeaturesControllerTests : IClassFixture<FeatureToggleApiFactory>
{
    private readonly HttpClient _httpClient;

    private readonly Faker<Product> _productGenerator = new Faker<Product>()
        .RuleFor(x => x.Name, f => $"{f.Lorem.Word()}{f.Lorem.Word()}")
        .RuleFor(x => x.Description, f => f.Lorem.Sentences());

    private readonly Faker<Feature> _featureGenerator =
        new Faker<Feature>()
            .RuleFor(x => x.Name, f => $"{f.Random.Word()}{f.Random.Word()}Enabled")
            .RuleFor(x => x.Description, f => f.Lorem.Sentences());

    public GetFeaturesControllerTests(FeatureToggleApiFactory apiFactory)
    {
        _httpClient = apiFactory.CreateClient();
    }

    [Fact]
    public async Task Get_ReturnAllFeatures_WhenFeaturesExist()
    {
        // Arrange.
        var fakeProduct = _productGenerator.Generate();
        var productCommand = new CreateProductCommand(fakeProduct.Name, fakeProduct.Description);
        var product = await (await _httpClient.PostAsJsonAsync("api/products", productCommand))
            .Content.ReadFromJsonAsync<ProductDto>();

        var fakeFeature = _featureGenerator.Generate();
        var command = new CreateFeatureCommand(product!.Id, fakeFeature.Name, fakeFeature.Description);
        _ = await _httpClient.PostAsJsonAsync("api/features", command);

        // Act.
        var response = await _httpClient.GetAsync($"api/features");

        // Assert.
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var featureResponse = await response.Content.ReadFromJsonAsync<List<FeatureDto>>();
        featureResponse.Should().NotBeEmpty();
        featureResponse![0].Name.Should().Be(command.Name);
        featureResponse![0].Description.Should().Be(command.Description);

        // Dispose.
        await _httpClient.DeleteAsync($"api/features/{featureResponse[0].Id}");
    }

    [Fact]
    public async Task Get_ReturnsEmptyResult_WhenNoFeaturesExist()
    {
        // Act.
        var response = await _httpClient.GetAsync($"api/features");

        // Assert.
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var featureResponse = await response.Content.ReadFromJsonAsync<List<FeatureDto>>();
        featureResponse.Should().BeEmpty();
    }
}
