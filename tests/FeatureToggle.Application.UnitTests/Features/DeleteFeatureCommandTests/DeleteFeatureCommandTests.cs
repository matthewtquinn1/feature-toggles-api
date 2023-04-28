using FeatureToggle.Application.Features.Commands;
using FeatureToggle.Domain.Entities;
using FluentAssertions;
using NSubstitute;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace FeatureToggle.Application.UnitTests.Features.DeleteFeatureCommandTests;

[ExcludeFromCodeCoverage]
public sealed class DeleteFeatureCommandTests
{
    [Fact]
    public async Task Handle_WhenFeatureNotFound_ShouldReturnNull()
    {
        // Arrange.
        var featureId = Guid.NewGuid();
        var command = new DeleteFeatureCommand(featureId);

        var features = new List<Feature>
        {
            new() { Id = Guid.NewGuid(), Name = "Distinct Name 1" },
            new() { Id = Guid.NewGuid(), Name = "Distinct Name 2" },
        };

        var sut = new DeleteFeatureCommandFixture()
            .WithReturnForContextFeatures(features)
            .CreateSut();

        // Act.
        var result = await sut.Handle(command, CancellationToken.None);

        // Assert.
        result.Should().BeNull();
    }

    [Fact]
    public async Task Handle_WhenFeatureFound_ShouldDeleteFeature()
    {
        // Arrange.
        var featureId = Guid.NewGuid();
        var command = new DeleteFeatureCommand(featureId);

        var feature = new Feature { Id = featureId, Name = "Distinct Name 1" };

        var fixture = new DeleteFeatureCommandFixture()
            .WithReturnForContextFeatures(new List<Feature> { feature });
        var sut = fixture.CreateSut();

        // Act.
        await sut.Handle(command, CancellationToken.None);

        // Assert.
        fixture.ApplicationDbContext.Features.Remove(Arg.Is<Feature>(x => x.Id == featureId));
        await fixture.ApplicationDbContext.SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
