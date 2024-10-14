using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Database.Repositories;
using Database.Contexts;
using Database.Repositories.User;
using Database.Repositories.Orchestrate;
using Database.Repositories.License;
using Database.Repositories.ChangeRequests;
using Database.Repositories.EmailNotification;
using Database.Entites.Factories;
using Database.Repositories.Tasks;
using Database.Logic;
using Database.Repositories.Feedback;

namespace Database.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddDatabaseDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext(configuration);
        services.AddRepositories();
    }

    private static void AddDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("LicenseHolderGateway");
        var errorNumbersToAdd = configuration.GetSection("SQL:DatabaseErrorNumbersForRetry").Value ?? throw new Exception("Unable to obtain configuration: SQL:DatabaseErrorNumbersForRetry");
        var maxRetryDelay = int.Parse(configuration.GetSection("SQL:MaxRetryDelayInSeconds").Value ?? throw new Exception("Unable to obtain configuration: SQL:MaxRetryDelayInSeconds"));
        var maxRetryCount = int.Parse(configuration.GetSection("SQL:MaxRetryCount").Value ?? throw new Exception("Unable to obtain configuration: SQL:MaxRetryCount"));

        services.AddDbContext<LicenceHolderDbContext>(options =>
                options.UseSqlServer(connectionString,
                    builder => builder.MigrationsAssembly(typeof(LicenceHolderDbContext).Assembly.FullName)
                    .UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
                    .EnableRetryOnFailure(
                        maxRetryCount: maxRetryCount,
                        maxRetryDelay: TimeSpan.FromSeconds(maxRetryDelay),
                        errorNumbersToAdd.Split(',').Select(int.Parse).ToArray())
                    ));
    }

    private static void AddRepositories(this IServiceCollection services)
    {
        services
            .AddScoped<IRepositoryForFeedback, RepositoryForFeedback>()
            .AddScoped<IStringLengthRestriction, StringLengthRestriction>()
            .AddScoped<IRepositoryForFinancialMonitoringTasks, RepositoryForFinancialMonitoringTasks>()
            .AddScoped<IRepositoryForAnnualCertificateTasks, RepositoryForAnnualCertificateTasks>()
            .AddScoped<IEmailNotificationFactory, EmailNotificationFactory>()
            .AddScoped<IRepositoryForEmailNotification, RepositoryForEmailNotification>()
            .AddScoped<IRepositoryForChangeRequests, RepositoryForChangeRequests>()
            .AddScoped<IRepositoryForLicense, RepositoryForLicense>()
            .AddScoped<IRepositoryOrchestrator, RepositoryOrchestrator>()
            .AddScoped<IRepositoryForUser, RepositoryForUser>()
            .AddScoped<IMessageRepository, MessageRepository>()
            .AddScoped<IOrganisationRepository, OrganisationRepository>()
            .AddScoped<ICompanyRepository, CompanyRepository>();

    }
}
