using FeatureToggle.Application.Features.Queries;
using FeatureToggle.Application.Products.Queries;
using FeatureToggle.Domain.Entities;
using FluentAssertions;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace FeatureToggle.Application.UnitTests.Products.GetProductByIdQueryTests;

[ExcludeFromCodeCoverage]
public sealed class GetProductByIdQueryTests
{
    [Fact]
    public async Task Handle_WhenMatchFound_ShouldReturnResult()
    {
        // Arrange.
        var id = Guid.NewGuid();

        var products = new List<Product>
        {
            new() {  Id = Guid.NewGuid() },
            new() {  Id = id },
            new() {  Id = Guid.NewGuid() },
        };

        var sut = new GetProductByIdQueryFixture()
            .WithReturnForContextProducts(products)
            .CreateSut();

        // Act.
        var result = await sut.Handle(new GetProductByIdQuery(id), CancellationToken.None);

        // Assert.
        result.Should().NotBeNull();
        result!.Id.Should().Be(id);
    }

    [Fact]
    public async Task Handle_WhenMatchNotFound_ShouldReturnNull()
    {
        // Arrange.
        var id = Guid.NewGuid();

        var products = new List<Product>
        {
            new() {  Id = Guid.NewGuid() },
            new() {  Id = Guid.NewGuid() },
            new() {  Id = Guid.NewGuid() },
        };

        var sut = new GetProductByIdQueryFixture()
            .WithReturnForContextProducts(products)
            .CreateSut();

        // Act.
        var result = await sut.Handle(new GetProductByIdQuery(id), CancellationToken.None);

        // Assert.
        result.Should().BeNull();
    }
}
