using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Logging;

namespace FeatureToggle.Api.Tests.Integration;

/// <summary>
/// The WebApplicationFactory creates an in-memory version of the API (but a real one) to run tests against.
/// We use this class to override it - allows us to configure e.g. logging, DB connections etc.
/// </summary>
public sealed class FeatureToggleApiFactory : WebApplicationFactory<IApiMarker>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        // Turn logging behaviour off for integration tests.
        builder.ConfigureLogging(loggingBuilder => loggingBuilder.ClearProviders());
    }
}
