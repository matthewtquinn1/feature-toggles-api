using FeatureToggle.Application.Products.Queries;
using FeatureToggle.Domain.Entities;
using Shouldly;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace FeatureToggle.Application.UnitTests.Products.GetProductsQueryTests;

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
        result.Count().ShouldBe(2);
        result.First().Id.ShouldBe(products.First().Id);
        result.Last().Id.ShouldBe(products.Last().Id);
    }
}
