using FeatureToggle.Application.Features.Commands;
using FeatureToggle.Domain.Entities;
using FluentAssertions;
using NSubstitute;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace FeatureToggle.Application.UnitTests.Features.UpdateFeatureCommandTests;

[ExcludeFromCodeCoverage]
public sealed class UpdateFeatureCommandTests
{
    [Fact]
    public async Task Handle_WhenFeatureNotFound_ShouldReturnNull()
    {
        // Arrange.
        var featureId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var command = new UpdateFeatureCommand(featureId, "Changed Test description");

        var features = new List<Feature>
        {
            new() { Id = Guid.NewGuid(), Name = "DistinctName1" },
            new() { Id = Guid.NewGuid(), Name = "DistinctName2" },
        };

        var products = new List<Product> { new() { Id = productId } };

        var sut = new UpdateFeatureCommandFixture()
            .WithReturnForContextFeatures(features)
            .WithReturnForContextProducts(products)
            .CreateSut();

        // Act.
        var result = await sut.Handle(command, CancellationToken.None);

        // Assert.
        result.Should().BeNull();
    }

    [Fact]
    public async Task Handle_WhenCommandIsValid_ShouldUpdateFeature()
    {
        // Arrange.
        var featureId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var command = new UpdateFeatureCommand(featureId, "Changed Test description");

        var features = new List<Feature>
        {
            new() { Id = featureId, Name = "Existing name", Description = "Existing Test description", Product = new Product { Id = productId } }
        };

        var products = new List<Product> { new() { Id = productId, DbId = 44 } };

        var fixture = new UpdateFeatureCommandFixture()
            .WithReturnForContextFeatures(features)
            .WithReturnForContextProducts(products);
        var sut = fixture.CreateSut();

        // Act.
        _ = await sut.Handle(command, CancellationToken.None);

        // Assert.
        fixture.ApplicationDbContext.Features.Update(Arg.Is<Feature>(
            f => f.Description == command.Description));
        await fixture.ApplicationDbContext.SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
