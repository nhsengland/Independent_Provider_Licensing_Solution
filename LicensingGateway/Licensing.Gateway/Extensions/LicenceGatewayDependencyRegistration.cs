using Database.DI;
using Database.Repository.Extensions;
using Domain.Logic.Extensions;
using Licensing.Gateway.Factories;

namespace Licensing.Gateway.Extensions;

public static class LicenceGatewayDependencyRegistration
{
    public static IServiceCollection Add(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddScoped<IViewModelFactory, ViewModelFactory>();

        DomainLogicDependencyRegistration.Add(services, configuration);

        DatabaseDependencyRegistration.Add(services, configuration);

        RespositoryDependencyRegistration.Add(services);

        return services;
    }
}
