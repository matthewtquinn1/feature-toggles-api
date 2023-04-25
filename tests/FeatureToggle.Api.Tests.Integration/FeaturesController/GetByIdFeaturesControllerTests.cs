using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using Xunit;

namespace FeatureToggle.Api.Tests.Integration.FeaturesController;

[ExcludeFromCodeCoverage]
public sealed class GetByIdFeaturesControllerTests : IClassFixture<WebApplicationFactory<IApiMarker>>
{
    private readonly HttpClient _httpClient;

    public GetByIdFeaturesControllerTests(WebApplicationFactory<IApiMarker> webApplicationFactory)
    {
        _httpClient = webApplicationFactory.CreateClient();
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
