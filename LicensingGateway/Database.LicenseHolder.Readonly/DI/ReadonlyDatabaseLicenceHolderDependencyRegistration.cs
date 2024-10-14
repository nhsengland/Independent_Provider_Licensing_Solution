using Database.Helpers;
using Database.LicenceHolder.Readonly.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Database.LicenceHolder.Readonly.DI;

public static class ReadonlyDatabaseLicenceHolderDependencyRegistration
{
    public static IServiceCollection Add(this IServiceCollection services, IConfiguration configuration)
    {
        var dbConnectionStringConfiguration = DatabaseConfigurationFactory.Create(configuration, "LicenceHolder");

        services.AddDbContext<ReadonlyLicenceHolderDbContext>(options =>
        {
            options.UseSqlServer(dbConnectionStringConfiguration.ConnectionString, builder => builder
                .UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
                .EnableRetryOnFailure(
                    maxRetryCount: dbConnectionStringConfiguration.MaxRetryCount,
                    maxRetryDelay: TimeSpan.FromSeconds(dbConnectionStringConfiguration.MaxRetryDelayInSeconds),
                    dbConnectionStringConfiguration.ErrorNumbersToAdd)
                )
            // DISABLE CHANGE TRACKING
            // This is a readonly database
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        });

        services.AddScoped<ILicenceHolderRespositoryForLicence, LicenceHolderRespositoryForLicence>();

        return services;
    }
}
