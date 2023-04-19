using FeatureToggle.Application.Common.Exceptions;
using FeatureToggle.Application.Products.Commands;
using FeatureToggle.Application.UnitTests.Products.CreateProductCommandTests;
using FeatureToggle.Domain.Entities;
using NSubstitute;
using Shouldly;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace FeatureToggle.Application.UnitTests.Products.CreateProductsCommandTests;

[ExcludeFromCodeCoverage]
public sealed class CreateProductCommandTests
{
    [Fact]
    public async Task Handle_WhenDuplicateFeatureFound_ShouldThrowException()
    {
        // Arrange.
        var productId = Guid.NewGuid();
        var command = new CreateProductCommand("DuplicateTestName", "Test description");

        var products = new List<Product>
        {
            new() { Id = Guid.NewGuid(), Name = "Distinct Name 1" },
            new() { Id = Guid.NewGuid(), Name = "DuplicateTestName" },
        };

        var sut = new CreateProductCommandFixture()
            .WithReturnForContextProducts(products)
            .CreateSut();

        // Act.
        Func<Task> action = () => sut.Handle(command, CancellationToken.None);

        // Assert.
        var exception = await action.ShouldThrowAsync(typeof(DuplicateFoundException));
        exception.Message.ShouldBe($"\"Product\" already exists with Name as (DuplicateTestName).");
    }

    [Fact]
    public async Task Handle_WhenCommandIsValid_ShouldCreateFeature()
    {
        // Arrange.
        var productId = Guid.NewGuid();
        var command = new CreateProductCommand("New Test Name", "Test description");

        var products = new List<Product> { new() { Id = Guid.NewGuid(), Name = "Distinct Name 1" } };

        var fixture = new CreateProductCommandFixture()
            .WithReturnForContextProducts(products);
        var sut = fixture.CreateSut();

        // Act.
        _ = await sut.Handle(command, CancellationToken.None);

        // Assert.
        await fixture.ApplicationDbContext.Received().Products.AddAsync(Arg.Is<Product>(
            x =>
                x.Name == command.Name &&
                x.Description == command.Description),
            Arg.Any<CancellationToken>());

        await fixture.ApplicationDbContext.Received().SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
