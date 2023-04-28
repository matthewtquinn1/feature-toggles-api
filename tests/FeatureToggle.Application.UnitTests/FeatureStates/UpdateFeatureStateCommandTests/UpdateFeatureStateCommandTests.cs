using FeatureToggle.Application.FeatureStates.Commands;
using FeatureToggle.Domain.Entities;
using FluentAssertions;
using NSubstitute;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace FeatureToggle.Application.UnitTests.FeatureStates.UpdateFeatureStateCommandTests;

[ExcludeFromCodeCoverage]
public sealed class UpdateFeatureStateCommandTests
{
    [Fact]
    public async Task Handle_WhenFeatureStateNotFound_ShouldReturnNull()
    {
        // Arrange.
        var featureStateId = Guid.NewGuid();
        var command = new UpdateFeatureStateCommand(featureStateId, true);

        var featureStates = new List<FeatureState>
        {
            new() { Id = Guid.NewGuid() },
            new() { Id = Guid.NewGuid() },
        };

        var sut = new UpdateFeatureStateCommandFixture()
            .WithReturnForContextFeatureStates(featureStates)
            .CreateSut();

        // Act.
        var result = await sut.Handle(command, CancellationToken.None);

        // Assert.
        result.Should().BeNull();
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task Handle_WhenCommandIsValid_ShouldUpdateFeature(bool isActive)
    {
        // Arrange.
        var featureStateId = Guid.NewGuid();
        var command = new UpdateFeatureStateCommand(featureStateId, isActive);

        var featureStates = new List<FeatureState>
        {
            new() { Id = featureStateId, IsActive = false },
            new() { Id = Guid.NewGuid(), IsActive = false },
        };

        var fixture = new UpdateFeatureStateCommandFixture()
            .WithReturnForContextFeatureStates(featureStates);
        var sut = fixture.CreateSut();

        // Act.
        _ = await sut.Handle(command, CancellationToken.None);

        // Assert.
        fixture.ApplicationDbContext.FeatureStates.Update(Arg.Is<FeatureState>(
            f => f.IsActive == command.IsActive));
        await fixture.ApplicationDbContext.SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
