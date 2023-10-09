using FeatureToggle.Application.Features;
using FeatureToggle.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace FeatureToggle.Application.UnitTests.Features.Mappings;

public sealed class FeatureMappingsTests
{
    [Fact]
    public void MapToDto_ShouldMapFeatureToDto()
    {
        var feature = new Feature
        {
            Id = Guid.NewGuid(),
            Name = "Name",
            Description= "Description",
            Product = new() { Id = Guid.NewGuid() },
            FeatureStates = new List<FeatureState> { new() { Id = Guid.NewGuid() } },
        };

        var result = feature.MapToDto();

        result.Id.Should().Be(feature.Id);
        result.ProductId.Should().Be(feature.Product.Id);
        result.Name.Should().Be(feature.Name);
        result.Description.Should().Be(feature.Description);
        result.FeatureStates.First().Id.Should().Be(feature.FeatureStates.First().Id);
    }

    [Fact]
    public void MapToDto_ShouldThrowException_WhenProductIsMissing()
    {
        var feature = new Feature
        {
            Id = Guid.NewGuid(),
            Name = "Name",
            Description = "Description",
            FeatureStates = new List<FeatureState> { new() { Id = Guid.NewGuid() } },
        };

        Action result = () => feature.MapToDto();

        result.Should().Throw<ArgumentNullException>();
    }
}
