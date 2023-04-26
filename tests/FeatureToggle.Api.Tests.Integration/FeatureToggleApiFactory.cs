using FeatureToggle.Application.Common.Interfaces;
using FeatureToggle.Infrastructure.Persistance;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Testcontainers.MsSql;
using Xunit;

namespace FeatureToggle.Api.Tests.Integration;

/// <summary>
/// The WebApplicationFactory creates an in-memory version of the API (but a real one) to run tests against.
/// We use this class to override it - allows us to configure e.g. logging, DB connections etc.
/// </summary>
public sealed class FeatureToggleApiFactory : WebApplicationFactory<IApiMarker>, IAsyncLifetime
{
    private readonly MsSqlContainer _dbContainer = new MsSqlBuilder().Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        // Turn logging behaviour off for integration tests.
        builder.ConfigureLogging(loggingBuilder => loggingBuilder.ClearProviders());

        // Replace database for integration tests with one spun up in a docker container.
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll(typeof(FeatureToggleContext));
            services.RemoveAll(typeof(IApplicationDbContext));

            services.AddPersistence(_dbContainer.GetConnectionString());
            services.AddScoped<IApplicationDbContext>(serviceProvider => serviceProvider.GetRequiredService<FeatureToggleContext>());
        });
    }

    public async Task InitializeAsync() => await _dbContainer.StartAsync();

    public new async Task DisposeAsync() => await _dbContainer.DisposeAsync();
}
