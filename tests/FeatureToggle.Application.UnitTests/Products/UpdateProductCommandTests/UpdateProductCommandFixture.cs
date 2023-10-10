using FeatureToggle.Application.Common.Interfaces;
using FeatureToggle.Application.Products.Commands;
using FeatureToggle.Domain.Entities;
using MockQueryable.NSubstitute;
using NSubstitute;
using System.Diagnostics.CodeAnalysis;

namespace FeatureToggle.Application.Tests.Unit.Products.UpdateProductCommandTests;

[ExcludeFromCodeCoverage]
internal sealed class UpdateProductCommandFixture
{
    internal readonly IApplicationDbContext ApplicationDbContext = Substitute.For<IApplicationDbContext>();

    public UpdateProductCommandHandler CreateSut() => new(ApplicationDbContext);

    internal UpdateProductCommandFixture WithReturnForContextProducts(IEnumerable<Product> result)
    {
        ApplicationDbContext.Products = result.AsQueryable().BuildMockDbSet();

        return this;
    }
}
