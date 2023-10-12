using FeatureToggle.Application.Features.Queries;
using FeatureToggle.Domain.Entities;
using FluentAssertions;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace FeatureToggle.Application.Tests.Unit.Features.GetFeaturesQueryTests;

[ExcludeFromCodeCoverage]
public sealed class GetFeaturesQueryTests
{
    [Fact]
    public async Task Handle_WhenResultsFound_ShouldReturnResults()
    {
        // Arrange.
        var features = new List<Feature>
        {
            new() {  Id = Guid.NewGuid(), Product = new Product { Id = Guid.NewGuid() } },
            new() {  Id = Guid.NewGuid(), Product = new Product { Id = Guid.NewGuid() } },
        };

        var sut = new GetFeaturesQueryFixture()
            .WithReturnForContextFeatures(features)
            .CreateSut();

        // Act.
        var result = await sut.Handle(new GetFeaturesQuery(), CancellationToken.None);

        // Assert.
        result.Should().HaveCount(2);
        result.First().Id.Should().Be(features.First().Id);
        result.Last().Id.Should().Be(features.Last().Id);
    }
}
