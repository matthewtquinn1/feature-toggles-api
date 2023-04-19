using FeatureToggle.Application.Features.Queries;
using FeatureToggle.Domain.Entities;
using Shouldly;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace FeatureToggle.Application.UnitTests.Features.GetFeaturesQueryTests;

[ExcludeFromCodeCoverage]
public sealed class GetFeaturesQueryTests
{
    [Fact]
    public async Task Handle_WhenResultsFound_ShouldReturnResults()
    {
        // Arrange.
        var features = new List<Feature>
        {
            new() {  Id = Guid.NewGuid() },
            new() {  Id = Guid.NewGuid() },
        };

        var sut = new GetFeaturesQueryFixture()
            .WithReturnForContextFeatures(features)
            .CreateSut();

        // Act.
        var result = await sut.Handle(new GetFeaturesQuery(), CancellationToken.None);

        // Assert.
        result.Count().ShouldBe(2);
        result.First().Id.ShouldBe(features.First().Id);
        result.Last().Id.ShouldBe(features.Last().Id);
    }
}
