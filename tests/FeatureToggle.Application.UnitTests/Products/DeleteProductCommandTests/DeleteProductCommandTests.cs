﻿using FeatureToggle.Application.Products.Commands;
using FeatureToggle.Domain.Entities;
using NSubstitute;
using Shouldly;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace FeatureToggle.Application.UnitTests.Products.DeleteProductCommandTests;

[ExcludeFromCodeCoverage]
public sealed class DeleteProductCommandTests
{
    [Fact]
    public async Task Handle_WhenProductNotFound_ShouldReturnNull()
    {
        // Arrange.
        var productId = Guid.NewGuid();
        var command = new DeleteProductCommand(productId);

        var products = new List<Product>
        {
            new() { Id = Guid.NewGuid(), Name = "DistinctName1" },
            new() { Id = Guid.NewGuid(), Name = "DistinctName2" },
        };

        var sut = new DeleteProductCommandFixture()
            .WithReturnForContextProducts(products)
            .CreateSut();

        // Act.
        var result = await sut.Handle(command, CancellationToken.None);

        // Assert.
        result.ShouldBeNull();
    }

    [Fact]
    public async Task Handle_WhenProductFound_ShouldDeleteProduct()
    {
        // Arrange.
        var productId = Guid.NewGuid();
        var command = new DeleteProductCommand(productId);

        var product = new Product { Id = productId, Name = "DistinctName1" };

        var fixture = new DeleteProductCommandFixture()
            .WithReturnForContextProducts(new List<Product> { product });
        var sut = fixture.CreateSut();

        // Act.
        await sut.Handle(command, CancellationToken.None);

        // Assert.
        fixture.ApplicationDbContext.Products.Remove(Arg.Is<Product>(x => x.Id == productId));
        await fixture.ApplicationDbContext.SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
