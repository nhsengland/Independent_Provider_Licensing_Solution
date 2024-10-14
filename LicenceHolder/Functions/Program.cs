using Domain.Logic.Extensions;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureAppConfiguration((hostBuilder, configurationBuilder) =>
    {
        if (hostBuilder.HostingEnvironment.IsDevelopment())
        {
            configurationBuilder.AddUserSecrets<Program>();
        }
    })
    .ConfigureServices((hostBuilder, services) =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();

        Database.Extensions.ServiceCollectionExtensions.AddDatabaseDependencies(services, hostBuilder.Configuration);

        ConfigureServices.AddDomainLogicDependencies(services, hostBuilder.Configuration);

        services.AddHttpClient();
    })
    .ConfigureLogging((hostBuilderContext, loggingBuilder) =>
    {
        loggingBuilder.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Warning);
    })
    .Build();

host.Run();
