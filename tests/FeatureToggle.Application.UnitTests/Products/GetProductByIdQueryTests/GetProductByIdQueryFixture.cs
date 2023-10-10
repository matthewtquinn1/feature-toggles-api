using FeatureToggle.Application.Common.Interfaces;
using FeatureToggle.Application.Products.Queries;
using FeatureToggle.Domain.Entities;
using MockQueryable.NSubstitute;
using NSubstitute;
using System.Diagnostics.CodeAnalysis;

namespace FeatureToggle.Application.Tests.Unit.Products.GetProductByIdQueryTests;

[ExcludeFromCodeCoverage]
internal sealed class GetProductByIdQueryFixture
{
    internal readonly IApplicationDbContext ApplicationDbContext = Substitute.For<IApplicationDbContext>();

    public GetProductByIdQueryHandler CreateSut() => new(ApplicationDbContext);

    internal GetProductByIdQueryFixture WithReturnForContextProducts(IEnumerable<Product> result)
    {
        ApplicationDbContext.Products = result.AsQueryable().BuildMockDbSet();

        return this;
    }
}
