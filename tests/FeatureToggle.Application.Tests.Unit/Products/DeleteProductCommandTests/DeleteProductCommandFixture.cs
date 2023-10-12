using FeatureToggle.Application.Common.Interfaces;
using FeatureToggle.Application.Products.Commands;
using FeatureToggle.Domain.Entities;
using MockQueryable.NSubstitute;
using NSubstitute;

namespace FeatureToggle.Application.Tests.Unit.Products.DeleteProductCommandTests;

internal sealed class DeleteProductCommandFixture
{
    internal readonly IApplicationDbContext ApplicationDbContext = Substitute.For<IApplicationDbContext>();

    public DeleteProductCommandHandler CreateSut() => new(ApplicationDbContext);

    internal DeleteProductCommandFixture WithReturnForContextProducts(IEnumerable<Product> result)
    {
        ApplicationDbContext.Products = result.AsQueryable().BuildMockDbSet();

        return this;
    }
}
