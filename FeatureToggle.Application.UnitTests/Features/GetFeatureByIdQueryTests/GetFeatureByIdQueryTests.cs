using FeatureToggle.Application.Features.Queries;
using FeatureToggle.Domain.Entities;
using Shouldly;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace FeatureToggle.Application.UnitTests.Features.GetFeatureByIdQueryTests;

[ExcludeFromCodeCoverage]
public sealed class GetFeatureByIdQueryTests
{
    [Fact]
    public async Task Handle_WhenMatchFound_ShouldReturnResult()
    {
        // Arrange.
        var id = Guid.NewGuid();

        var features = new List<Feature>
        {
            new() {  Id = Guid.NewGuid() },
            new() {  Id = id },
            new() {  Id = Guid.NewGuid() },
        };

        var sut = new GetFeatureByIdQueryFixture()
            .WithReturnForContextFeatures(features)
            .CreateSut();

        // Act.
        var result = await sut.Handle(new GetFeatureByIdQuery(id), CancellationToken.None);

        // Assert.
        result.ShouldNotBeNull();
        result.Id.ShouldBe(id);
    }

    [Fact]
    public async Task Handle_WhenMatchNotFound_ShouldReturnNull()
    {
        // Arrange.
        var id = Guid.NewGuid();

        var features = new List<Feature>
        {
            new() {  Id = Guid.NewGuid() },
            new() {  Id = Guid.NewGuid() },
            new() {  Id = Guid.NewGuid() },
        };

        var sut = new GetFeatureByIdQueryFixture()
            .WithReturnForContextFeatures(features)
            .CreateSut();

        // Act.
        var result = await sut.Handle(new GetFeatureByIdQuery(id), CancellationToken.None);

        // Assert.
        result.ShouldBeNull();
    }
}
