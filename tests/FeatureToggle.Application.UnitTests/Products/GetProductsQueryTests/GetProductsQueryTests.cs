using FeatureToggle.Application.Products.Queries;
using FeatureToggle.Domain.Entities;
using FluentAssertions;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace FeatureToggle.Application.Tests.Unit.Products.GetProductsQueryTests;

[ExcludeFromCodeCoverage]
public sealed class GetProductsQueryTests
{
    [Fact]
    public async Task Handle_WhenResultsFound_ShouldReturnResults()
    {
        // Arrange.
        var products = new List<Product>
        {
            new() {  Id = Guid.NewGuid() },
            new() {  Id = Guid.NewGuid() },
        };

        var sut = new GetProductsQueryFixture()
            .WithReturnForContextProducts(products)
            .CreateSut();

        // Act.
        var result = await sut.Handle(new GetProductsQuery(), CancellationToken.None);

        // Assert.
        result.Should().HaveCount(2);
        result.First().Id.Should().Be(products.First().Id);
        result.Last().Id.Should().Be(products.Last().Id);
    }
}
