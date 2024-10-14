using Database.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Database.DI;

public static class DatabaseDependencyRegistration
{
    public static IServiceCollection Add(this IServiceCollection services, IConfiguration configuration)
    {
        var dbConnectionStringConfiguration = DatabaseConfigurationFactory.Create(configuration, "LicensingGateway");

        services.AddDbContext<LicensingGatewayDbContext>(options =>
        {
            options.UseSqlServer(dbConnectionStringConfiguration.ConnectionString, builder => builder.MigrationsAssembly(typeof(LicensingGatewayDbContext).Assembly.FullName)
                .UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
                .EnableRetryOnFailure(
                    maxRetryCount: dbConnectionStringConfiguration.MaxRetryCount,
                    maxRetryDelay: TimeSpan.FromSeconds(dbConnectionStringConfiguration.MaxRetryDelayInSeconds),
                    dbConnectionStringConfiguration.ErrorNumbersToAdd)
                );
        });

        services
            .AddScoped<ILicensingGatewayDbContext, LicensingGatewayDbContext>();

        return services;
    }
}
