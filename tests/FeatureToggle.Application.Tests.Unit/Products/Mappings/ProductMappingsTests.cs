using FeatureToggle.Application.Products;
using FeatureToggle.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace FeatureToggle.Application.Tests.Unit.Products.Mappings;

public sealed class ProductMappingsTests
{
    [Fact]
    public void MapToDto_ShouldMapProductToDto()
    {
        var productId = Guid.NewGuid();
        var product = new Product
        {
            Id = productId,
            Name = "Test",
            Description = "Test",
            Features = new List<Feature>
                {
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Product = new() { Id = productId }
                    }
                }
        };

        var result = product.MapToDto();

        result.Id.Should().Be(product.Id);
        result.Name.Should().Be(product.Name);
        result.Description.Should().Be(product.Description);
        result.Features.First().Id.Should().Be(product.Features.First().Id);
        result.Features.First().ProductId.Should().Be(product.Id);
    }
}
