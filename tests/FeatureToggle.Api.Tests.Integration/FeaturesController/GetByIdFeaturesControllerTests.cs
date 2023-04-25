using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using Xunit;

namespace FeatureToggle.Api.Tests.Integration.FeaturesController;

[ExcludeFromCodeCoverage]
public sealed class GetByIdFeaturesControllerTests : IClassFixture<FeatureToggleApiFactory>
{
    private readonly HttpClient _httpClient;

    public GetByIdFeaturesControllerTests(FeatureToggleApiFactory apiFactory)
    {
        _httpClient = apiFactory.CreateClient();
    }

    [Fact]
    public async Task GetById_ReturnsNotFound_WhenFeatureDoesNotExist()
    {
        // Act.
        var response = await _httpClient.GetAsync($"api/features/{Guid.NewGuid()}");

        // Assert.
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
