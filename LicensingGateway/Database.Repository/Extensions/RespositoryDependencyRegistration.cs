using Database.Repository.Application;
using Database.Repository.ApplicationCode;
using Database.Repository.CQC;
using Database.Repository.Director;
using Database.Repository.Email;
using Database.Repository.Feedback;
using Database.Repository.Helpers;
using Database.Repository.UltimateController;
using Microsoft.Extensions.DependencyInjection;

namespace Database.Repository.Extensions;
public static class RespositoryDependencyRegistration
{
    public static IServiceCollection Add(this IServiceCollection services)
    {
        services
            .AddScoped<IRepositoryForFeedback, RepositoryForFeedback>()
            .AddScoped<IBoolConverter, BoolConverter>()
            .AddScoped<IRepositoryForUltimateController, RepositoryForUltimateController>()
            .AddScoped<IRespositoryForApplicationCode, RespositoryForApplicationCode>()
            .AddScoped<IRepositoryForEmailNotifications, RepositoryForEmailNotifications>()
            .AddScoped<IRepositoryForApplication, RepositoryForApplication>()
            .AddScoped<IRepositoryForCQCProvider, RepositoryForCQCProvider>()
            .AddScoped<IRepositoryForDirectors, RepositoryForDirectors>()
            .AddScoped<IRepositoryForPreApplication, RepositoryForPreApplication>()
            .AddScoped<IRepositoryForCQCProviderImportPage, RepositoryForCQCProviderImportPage>()
            .AddScoped<IRespositoryForCQCProviderImportInstance, RespositoryForCQCProviderImportInstance>();

        return services;
    }
}
