using FeatureToggle.Application.Features.Queries;
using FeatureToggle.Domain.Entities;
using FluentAssertions;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace FeatureToggle.Application.Tests.Unit.Features.GetFeatureByIdQueryTests;

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
            new() {  Id = Guid.NewGuid(), Product = new Product { Id = Guid.NewGuid() } },
            new() {  Id = id, Product = new Product { Id = Guid.NewGuid() }},
            new() {  Id = Guid.NewGuid(), Product = new Product { Id = Guid.NewGuid() } },
        };

        var sut = new GetFeatureByIdQueryFixture()
            .WithReturnForContextFeatures(features)
            .CreateSut();

        // Act.
        var result = await sut.Handle(new GetFeatureByIdQuery(id), CancellationToken.None);

        // Assert.
        result.Should().NotBeNull();
        result!.Id.Should().Be(id);
    }

    [Fact]
    public async Task Handle_WhenMatchNotFound_ShouldReturnNull()
    {
        // Arrange.
        var id = Guid.NewGuid();

        var features = new List<Feature>
        {
            new() {  Id = Guid.NewGuid(), Product = new Product { Id = Guid.NewGuid() } },
            new() {  Id = Guid.NewGuid(), Product = new Product { Id = Guid.NewGuid() } },
            new() {  Id = Guid.NewGuid(), Product = new Product { Id = Guid.NewGuid() } },
        };

        var sut = new GetFeatureByIdQueryFixture()
            .WithReturnForContextFeatures(features)
            .CreateSut();

        // Act.
        var result = await sut.Handle(new GetFeatureByIdQuery(id), CancellationToken.None);

        // Assert.
        result.Should().BeNull();
    }
}
