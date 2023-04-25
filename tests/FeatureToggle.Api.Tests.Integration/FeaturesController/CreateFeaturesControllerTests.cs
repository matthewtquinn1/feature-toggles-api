using Bogus;
using FeatureToggle.Application.Features.Commands;
using FeatureToggle.Application.Products;
using FeatureToggle.Domain.Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using System.Net;
using Xunit;
using System.Diagnostics.CodeAnalysis;

namespace FeatureToggle.Api.Tests.Integration.FeaturesController;

[ExcludeFromCodeCoverage]
public sealed class CreateFeaturesControllerTests : IClassFixture<WebApplicationFactory<IApiMarker>>, IAsyncLifetime
{
    private readonly HttpClient _httpClient;

    private readonly Faker<Feature> _featureGenerator =
        new Faker<Feature>()
            .RuleFor(x => x.Name, f => $"IntegrationTestData{f.Random.Word()}Enabled")
            .RuleFor(x => x.Description, f => f.Lorem.Sentences());

    private readonly List<Guid> _createdIds = new();

    public CreateFeaturesControllerTests(WebApplicationFactory<IApiMarker> webApplicationFactory)
    {
        _httpClient = webApplicationFactory.CreateClient();
    }

    [Fact]
    public async Task Create_ReturnsCreated_WhenFeatureIsCreated()
    {
        // Arrange.
        var product = (await (await _httpClient.GetAsync("api/products"))
            .Content.ReadFromJsonAsync<IEnumerable<ProductDto>>())
            !.First();

        var feature = _featureGenerator.Generate();
        var command = new CreateFeatureCommand(product.Id, feature.Name, feature.Description);

        // Act.
        var response = await _httpClient.PostAsJsonAsync("api/features", command);

        // Assert.
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        // Dispose.
        _createdIds.Add(await response.Content.ReadFromJsonAsync<Guid>());
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

    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync()
    {
        foreach (var id in _createdIds)
        {
            await _httpClient.DeleteAsync($"api/features/{id}");
        }
    }
}