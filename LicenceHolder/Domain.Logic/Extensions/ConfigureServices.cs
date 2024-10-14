using Domain.Logic.Factories;
using Domain.Logic.Features.Dashboard;
using Domain.Logic.Features.Feedback;
using Domain.Logic.Features.Licence;
using Domain.Logic.Features.Messages;
using Domain.Logic.Features.Messages.Factories;
using Domain.Logic.Features.Tasks;
using Domain.Logic.Features.Team;
using Domain.Logic.Features.Team.Delete;
using Domain.Logic.Features.Team.InviteUser;
using Domain.Logic.Features.Team.ManageUsers;
using Domain.Logic.Features.Team.Maps;
using Domain.Logic.Features.YourProfile;
using Domain.Logic.Integrations.Automation;
using Domain.Logic.Integrations.Automation.Factories;
using Domain.Logic.Integrations.Session;
using Domain.Logic.Integrations.StorageAccount.Queues;
using Domain.Logic.Integrations.StorageAccount.Queues.Factories;
using Domain.Objects;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;

namespace Domain.Logic.Extensions;

public static class ConfigureServices
{
    public static IServiceCollection AddDomainLogicDependencies(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var messagingConfiguration = new MessagesConfiguration()
        {
            PageSize = int.Parse(configuration.GetSection("Notifications:PageSize").Value ?? "10")
        };
        services.AddSingleton(messagingConfiguration);

        var addUserHandlerConfiguration = new AddUserHandlerConfiguration()
        {
            LicenseHolderApplicationURL = configuration.GetSection("FunctionConfiguration:LicenseHolderURL").Value ?? string.Empty,
        };
        services.AddSingleton(addUserHandlerConfiguration);

        var settingsControllerConfiguration = new YourProfileConfiguration()
        {
            OktaUrl = configuration.GetSection("Okta:Domain").Value ?? string.Empty,
        };
        services.AddSingleton(settingsControllerConfiguration);

        var queueConfiguration = new QueueClientConfiguration();
        configuration.GetSection("QueueClientConfiguration").Bind(queueConfiguration);
        services.AddSingleton(queueConfiguration);

        var automationConfiguration = new AutomationConfiguration();
        configuration.GetSection("AutomationConfiguration").Bind(automationConfiguration);
        services.AddSingleton(automationConfiguration);

        services
            .AddScoped<ISessionOrchestration, SessionOrchestration>()
            .AddScoped<IFeedbackHandler, FeedbackHandler>()
            .AddScoped<IAddressConcatenation, AddressConcatenation>()
            .AddScoped<IDateTimeFactory, DateTimeFactory>()
            .AddScoped<IDeleteUserHandler, DeleteUserHandler>()
            .AddScoped<IYourProfileHandler, YourProfileHandler>()
            .AddScoped<IMessageTitleFactory, MessageTitleFactory>()
            .AddScoped<IMessageBodyFactory, MessageBodyFactory>()
            .AddScoped<ISkipFactory, SkipFactory>()
            .AddScoped<IPageCountFactory, PageCountFactory>()
            .AddScoped<ITaskControllerHandler, TaskControllerHandler>()
            .AddScoped<IQueueMessageFactory, QueueMessageFactory>()
            .AddScoped<IDateEvaluation, DateEvaluation>()
            .AddScoped<IDateConverter, DateConverter>()
            .AddScoped<ILicenceControllerHandler, LicenceControllerHandler>()
            .AddScoped<IRemoveUserFromGroupFactory, RemoveUserFromGroupFactory>()
            .AddScoped<IAccessLevelMapper, AccessLevelMapper>()
            .AddScoped<IEmailBodyTemplateFactory, EmailBodyTemplateFactory>()
            .AddScoped<ICreateOktaUserFactory, CreateOktaUserFactory>()
            .AddScoped<IAutomationAPIWrapper, AutomationAPIWrapper>()
            .AddScoped<IAccessTokenFactory, AccessTokenFactory>()
            .AddScoped<IAddUserHandler, AddUserHandler>()
            .AddScoped<IQueueMessageEncoder, QueueMessageEncoder>()
            .AddScoped<IQueueClientFactory, QueueClientFactory>()
            .AddScoped<IStorageAccountQueueWrapper, StorageAccountQueueWrapper>()
            .AddScoped<IInviteNewUserHandler, InviteNewUserHandler>()
            .AddScoped<ITeamManagementHandler, TeamManagementHandler>()
            .AddScoped<IManageUsersHandler, ManageUsersHandler>()
            .AddScoped<IMessagesHandler, MessagesHandler>()
            .AddScoped<IDashboardViewModelHandler, DashboardViewModelHandler>()
            .AddScoped<ITypeConverter, TypeConverter>()
            .AddScoped<IMessageDateFactory, MessageDateFactory>()
            .AddScoped<IMessagePropertyFactory, MessagePropertyFactory>();

        services
            .AddHttpClient()
            .AddHttpClient(HttpClientConstants.HttpClientNameWithRetry)
                .AddTransientHttpErrorPolicy(policyBuilder =>
                    policyBuilder.WaitAndRetryAsync(
                        3, retryNumber => TimeSpan.FromSeconds(5)));

        return services;
    }
}
