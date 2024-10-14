using Domain.Logic.Forms.Application;
using Domain.Logic.Forms.Application.Helpers;
using Domain.Logic.Forms.Application.Page;
using Domain.Logic.Forms.Application.Rules;
using Domain.Logic.Forms.Factories;
using Domain.Logic.Forms.Feedback;
using Domain.Logic.Forms.Helpers;
using Domain.Logic.Forms.Helpers.Session;
using Domain.Logic.Forms.PreApplication;
using Domain.Logic.Integration.CQC;
using Domain.Logic.Integration.CQC.API;
using Domain.Logic.Integration.CQC.API.Factories;
using Domain.Logic.Integration.CQC.Factories;
using Domain.Logic.Integration.Email;
using Domain.Logic.Integration.Email.Configuration;
using Domain.Logic.Integration.Email.Factories;
using Domain.Logic.Integration.StorageAccount.Queues;
using Domain.Logic.Integration.StorageAccount.Queues.Factories;
using Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;

namespace Domain.Logic.Extensions;

public static class DomainLogicDependencyRegistration
{
    public static IServiceCollection Add(this IServiceCollection services, IConfiguration configuration)
    {
        var cQCApiConfiguration = new CQCApiConfiguration();
        configuration.GetSection("CQC:API").Bind(cQCApiConfiguration);
        services.AddSingleton(cQCApiConfiguration);

        var emailConfiguration = new EmailConfiguration();
        configuration.GetSection("EmailConfiguration").Bind(emailConfiguration);
        services.AddSingleton(emailConfiguration);

        var queueClientConfiguration = new QueueClientConfiguration();
        configuration.GetSection("QueueClientConfiguration").Bind(queueClientConfiguration);
        services.AddSingleton(queueClientConfiguration);

        var homeControllerPageConfiguration = new HomeControllerPageConfiguration();
        configuration.GetSection("HomeControllerPageConfiguration").Bind(homeControllerPageConfiguration);
        services.AddSingleton(homeControllerPageConfiguration);

        services
            .AddScoped<IApplicationBusinessRules, ApplicationBusinessRules>()
            .AddScoped<IDateTimeFactory, DateTimeFactory>()
            .AddScoped<IPageToControllerMapper, PageToControllerMapper>()
            .AddScoped<ISessionDateHandler, SessionDateHandler>()
            .AddSingleton<ISessionOrchestration, SessionOrchestration>()
            .AddScoped<IDirectorOrchestration, DirectorOrchestration>()
            .AddScoped<IFeedbackOchestration, FeedbackOchestration>()
            .AddScoped<INextPageOrchestor, NextPageOrchestor>()
            .AddScoped<IResponseConverter, ResponseConverter>()
            .AddScoped<IDateConverter, DateConverter>()
            .AddScoped<IDateEvaluation, DateEvaluation>()
            .AddScoped<IContactDetailsEvaluator, ContactDetailsEvaluator>()
            .AddScoped<IQueueMessageEncoder, QueueMessageEncoder>()
            .AddScoped<IAccessTokenFactory, AccessTokenFactory>()
            .AddScoped<IEmailBodyTemplateFactory, EmailBodyTemplateFactory>()
            .AddScoped<IEmailOrchestration, EmailOrchestration>()
            .AddScoped<IQueueClientFactory, QueueClientFactory>()
            .AddScoped<IStorageAccountQueueWrapper, StorageAccountQueueWrapper>()
            .AddScoped<IEmailServiceWrapper, EmailServiceWrapper>()
            .AddScoped<IApplicationOrchestration, ApplicationOrchestration>()
            .AddScoped<ICQCAddressFactory, CQCAddressFactory>()
            .AddScoped<ICQCDateFactory, CQCDateFactory>()
            .AddScoped<ICQCApiWrapper, CQCApiWrapper>()
            .AddScoped<IReferenceIDFactory, ReferenceIDFactory>()
            .AddScoped<IPreApplicationOrchestration, PreApplicationOrchestration>()
            .AddScoped<ICQCProviderOrchestration, CQCProviderOrchestration>();

        services
            .AddHttpClient()
            .AddHttpClient(HttpClientConstants.HttpClientNameWithRetry)
                .AddTransientHttpErrorPolicy(policyBuilder =>
                    policyBuilder.WaitAndRetryAsync(
                        3, retryNumber => TimeSpan.FromSeconds(5)));

        return services;
    }
}
