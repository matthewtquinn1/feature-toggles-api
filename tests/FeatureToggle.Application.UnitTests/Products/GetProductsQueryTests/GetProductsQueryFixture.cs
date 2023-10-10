using FeatureToggle.Application.Common.Interfaces;
using FeatureToggle.Application.Products.Queries;
using FeatureToggle.Domain.Entities;
using MockQueryable.NSubstitute;
using NSubstitute;
using System.Diagnostics.CodeAnalysis;

namespace FeatureToggle.Application.Tests.Unit.Products.GetProductsQueryTests;

[ExcludeFromCodeCoverage]
internal sealed class GetProductsQueryFixture
{
    internal readonly IApplicationDbContext ApplicationDbContext = Substitute.For<IApplicationDbContext>();

    public GetProductsQueryHandler CreateSut() => new(ApplicationDbContext);

    internal GetProductsQueryFixture WithReturnForContextProducts(IEnumerable<Product> result)
    {
        ApplicationDbContext.Products = result.AsQueryable().BuildMockDbSet();

        return this;
    }
}
