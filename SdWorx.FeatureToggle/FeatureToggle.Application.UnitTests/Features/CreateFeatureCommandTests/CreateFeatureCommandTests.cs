using FeatureToggle.Application.Common.Exceptions;
using FeatureToggle.Application.Features.Commands;
using FeatureToggle.Domain.Entities;
using FeatureToggle.Domain.Enums;
using NSubstitute;
using Shouldly;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace FeatureToggle.Application.UnitTests.Features.CreateFeatureCommandTests;

[ExcludeFromCodeCoverage]
public sealed class CreateFeatureCommandTests
{
    [Fact]
    public async Task Handle_WhenProductNotFound_ShouldThrowException()
    {
        // Arrange.
        var productId = Guid.NewGuid();
        var command = new CreateFeatureCommand(productId, "Test name", "Test description");

        var products = new List<Product>
        {
            new() { Id = Guid.NewGuid() },
            new() { Id = Guid.NewGuid() },
        };

        var features = new List<Feature>
        {
            new() { Id = Guid.NewGuid(), Name = "Distinct Name 1" },
            new() { Id = Guid.NewGuid(), Name = "Distinct Name 2" },
        };

        var sut = new CreateFeatureCommandFixture()
            .WithReturnForContextProducts(products)
            .WithReturnForContextFeatures(features)
            .CreateSut();

        // Act.
        Func<Task> action = () => sut.Handle(command, CancellationToken.None);

        // Assert.
        var exception = await action.ShouldThrowAsync<NotFoundException>();
        exception.Message.ShouldBe($"Entity \"product\" ({productId}) was not found.");
    }

    [Fact]
    public async Task Handle_WhenDuplicateFeatureFound_ShouldThrowException()
    {
        // Arrange.
        var productId = Guid.NewGuid();
        var command = new CreateFeatureCommand(productId, "DuplicateTestName", "Test description");

        var products = new List<Product> { new() { Id = productId } };

        var features = new List<Feature>
        {
            new() { Id = Guid.NewGuid(), Name = "Distinct Name 1" },
            new() { Id = Guid.NewGuid(), Name = "DuplicateTestName" },
        };

        var sut = new CreateFeatureCommandFixture()
            .WithReturnForContextProducts(products)
            .WithReturnForContextFeatures(features)
            .CreateSut();

        // Act.
        Func<Task> action = () => sut.Handle(command, CancellationToken.None);

        // Assert.
        var exception = await action.ShouldThrowAsync(typeof(DuplicateFoundException));
        exception.Message.ShouldBe($"\"Feature\" already exists with Name as (DuplicateTestName).");
    }

    [Fact]
    public async Task Handle_WhenCommandIsValid_ShouldCreateFeature()
    {
        // Arrange.
        var productId = Guid.NewGuid();
        var command = new CreateFeatureCommand(productId, "New Test Name", "Test description");

        var products = new List<Product> { new() { Id = productId } };

        var features = new List<Feature>
        {
            new() { Id = Guid.NewGuid(), Name = "Distinct Name 1" },
            new() { Id = Guid.NewGuid(), Name = "Distinct Name 2" },
        };

        var fixture = new CreateFeatureCommandFixture()
            .WithReturnForContextProducts(products)
            .WithReturnForContextFeatures(features);
        var sut = fixture.CreateSut();

        // Act.
        _ = await sut.Handle(command, CancellationToken.None);

        // Assert.
        await fixture.ApplicationDbContext.Received().Features.AddAsync(Arg.Is<Feature>(
            x =>
                x.Name == command.Name &&
                x.Description == command.Description &&
                x.Product.Id == command.ProductId &&
                x.FeatureStates.Count == Enum.GetNames(typeof(FeatureEnvironment)).Length &&
                x.FeatureStates.All(state => !state.IsActive)),
            Arg.Any<CancellationToken>());

        await fixture.ApplicationDbContext.Received().SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
