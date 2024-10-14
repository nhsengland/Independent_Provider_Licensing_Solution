using Database.DI;
using Database.LicenceHolder.Readonly.DI;
using Database.Repository.Extensions;
using Domain.Logic.Extensions;
using Functions.DI;
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
        DomainLogicDependencyRegistration.Add(services, hostBuilder.Configuration);

        DatabaseDependencyRegistration.Add(services, hostBuilder.Configuration);

        ReadonlyDatabaseLicenceHolderDependencyRegistration.Add(services, hostBuilder.Configuration);

        RespositoryDependencyRegistration.Add(services);

        FunctionDependencyRegistration.Add(services, hostBuilder.Configuration);

        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        services.AddHttpClient();
    })
    .ConfigureLogging((hostBuilderContext, loggingBuilder) =>
    {
        loggingBuilder.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Warning);
    })
    .Build();

host.Run();
