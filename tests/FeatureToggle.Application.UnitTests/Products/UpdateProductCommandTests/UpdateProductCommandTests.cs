using FeatureToggle.Application.Common.Exceptions;
using FeatureToggle.Application.Products.Commands;
using FeatureToggle.Domain.Entities;
using NSubstitute;
using Shouldly;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace FeatureToggle.Application.UnitTests.Products.UpdateProductCommandTests;

[ExcludeFromCodeCoverage]
public sealed class UpdateProductCommandTests
{
    [Fact]
    public async Task Handle_WhenProductNotFound_ShouldReturnNull()
    {
        // Arrange.
        var productId = Guid.NewGuid();
        var command = new UpdateProductCommand(productId, "ChangedName", "Changed Test description");

        var products = new List<Product>
        {
            new() { Id = Guid.NewGuid(), Name = "DistinctName1" },
            new() { Id = Guid.NewGuid(), Name = "DistinctName2" },
        };

        var sut = new UpdateProductCommandFixture()
            .WithReturnForContextProducts(products)
            .CreateSut();

        // Act.
        var result = await sut.Handle(command, CancellationToken.None);

        // Assert.
        result.ShouldBeNull();
    }

    [Fact]
    public async Task Handle_WhenDuplicateProductFound_ShouldThrowException()
    {
        // Arrange.
        var productId = Guid.NewGuid();
        var command = new UpdateProductCommand(productId, "NewName", "Existing Test description");

        var products = new List<Product>
        {
            new() { Id = productId, Name = "ExistingName" },
            new() { Id = Guid.NewGuid(), Name = "NewName" },
        };

        var sut = new UpdateProductCommandFixture()
            .WithReturnForContextProducts(products)
            .CreateSut();

        // Act.
        Func<Task> action = () => sut.Handle(command, CancellationToken.None);

        // Assert.
        var exception = await action.ShouldThrowAsync<DuplicateFoundException>();
        exception.Message.ShouldBe($"\"Product\" already exists with Name as (NewName).");
    }

    [Fact]
    public async Task Handle_WhenCommandIsValid_ShouldUpdateProduct()
    {
        // Arrange.
        var productId = Guid.NewGuid();
        var command = new UpdateProductCommand(productId, "Changed Test name", "Changed Test description");

        var products = new List<Product>
        {
            new() { Id = productId, Name = "Existing name", Description = "Existing Test description" }
        };

        var fixture = new UpdateProductCommandFixture()
            .WithReturnForContextProducts(products);
        var sut = fixture.CreateSut();

        // Act.
        _ = await sut.Handle(command, CancellationToken.None);

        // Assert.
        fixture.ApplicationDbContext.Products.Update(Arg.Is<Product>(
            p =>
                p.Name == command.Name &&
                p.Description == command.Description
                ));
        await fixture.ApplicationDbContext.SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
