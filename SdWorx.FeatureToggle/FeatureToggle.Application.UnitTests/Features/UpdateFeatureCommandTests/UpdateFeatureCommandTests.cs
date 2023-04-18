using FeatureToggle.Application.Common.Exceptions;
using FeatureToggle.Application.Features.Commands;
using FeatureToggle.Domain.Entities;
using NSubstitute;
using Shouldly;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace FeatureToggle.Application.UnitTests.Features.UpdateFeatureCommandTests;

[ExcludeFromCodeCoverage]
public sealed class UpdateFeatureCommandTests
{
    [Fact]
    public async Task Handle_WhenFeatureNotFound_ShouldThrowException()
    {
        // Arrange.
        var featureId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var command = new UpdateFeatureCommand(featureId, productId, "ChangedName", "Changed Test description");

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
        Func<Task> action = () => sut.Handle(command, CancellationToken.None);

        // Assert.
        var exception = await action.ShouldThrowAsync<NotFoundException>();
        exception.Message.ShouldBe($"Entity \"feature\" ({featureId}) was not found.");
    }

    [Fact]
    public async Task Handle_WhenDuplicateFeatureFound_ShouldThrowException()
    {
        // Arrange.
        var featureId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var command = new UpdateFeatureCommand(featureId, productId, "NewName", "Existing Test description");

        var products = new List<Product> { new() { Id = productId } };

        var features = new List<Feature>
        {
            new() { Id = featureId, Name = "ExistingName" },
            new() { Id = Guid.NewGuid(), Name = "NewName" },
        };

        var sut = new UpdateFeatureCommandFixture()
            .WithReturnForContextFeatures(features)
            .WithReturnForContextProducts(products)
            .CreateSut();

        // Act.
        Func<Task> action = () => sut.Handle(command, CancellationToken.None);

        // Assert.
        var exception = await action.ShouldThrowAsync<DuplicateFoundException>();
        exception.Message.ShouldBe($"\"Feature\" already exists with Name as (NewName).");
    }

    [Fact]
    public async Task Handle_WhenProductNotFound_ShouldThrowException()
    {
        // Arrange.
        var featureId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var command = new UpdateFeatureCommand(featureId, productId, "ChangedTestName", "Changed Test description");

        var features = new List<Feature>
        {
            new() { Id = featureId, Name = "ExistingTestName", Product = new Product { Id = Guid.NewGuid() } }
        };

        var products = new List<Product> { new() { Id = Guid.NewGuid() } };

        var sut = new UpdateFeatureCommandFixture()
            .WithReturnForContextFeatures(features)
            .WithReturnForContextProducts(products)
            .CreateSut();

        // Act.
        Func<Task> action = () => sut.Handle(command, CancellationToken.None);

        // Assert.
        var exception = await action.ShouldThrowAsync<NotFoundException>();
        exception.Message.ShouldBe($"Entity \"Product\" ({productId}) was not found.");
    }

    [Fact]
    public async Task Handle_WhenProductIsChanged_ShouldAssignNewProduct()
    {
        // Arrange.
        var featureId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var command = new UpdateFeatureCommand(featureId, productId, "Changed Test name", "Changed Test description");

        var features = new List<Feature>
        {
            new() { Id = featureId, Name = "Existing name", Description = "Existing Test description", Product = new Product { Id = Guid.NewGuid() } }
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
            f =>
                f.ProductDbId == products.First().DbId &&
                f.Product.Id == products.First().Id
                ));
    }

    [Fact]
    public async Task Handle_WhenProductIsTheSame_ShouldKeepExistingProduct()
    {
        // Arrange.
        var featureId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var command = new UpdateFeatureCommand(featureId, productId, "Changed Test name", "Changed Test description");

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
            f =>
                f.ProductDbId == products.First().DbId &&
                f.Product.Id == products.First().Id
                ));
    }

    [Fact]
    public async Task Handle_WhenCommandIsValid_ShouldUpdateFeature()
    {
        // Arrange.
        var featureId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var command = new UpdateFeatureCommand(featureId, productId, "Changed Test name", "Changed Test description");

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
            f =>
                f.ProductDbId == products.First().DbId &&
                f.Product.Id == command.ProductId &&
                f.Name == command.Name &&
                f.Description == command.Description
                ));
        await fixture.ApplicationDbContext.SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
