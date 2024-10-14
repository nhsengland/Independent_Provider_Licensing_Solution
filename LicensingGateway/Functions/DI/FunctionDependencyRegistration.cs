using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Functions.Functions;
using Functions.Factories;

namespace Functions.DI;
static class FunctionDependencyRegistration
{
    public static IServiceCollection Add(this IServiceCollection services, IConfiguration configuration)
    {
        var cQCFunctionConfiguration = new CQCFunctionConfiguration();
        configuration.GetSection("FunctionConfiguration:CQC").Bind(cQCFunctionConfiguration);
        services.AddSingleton(cQCFunctionConfiguration);

        services.AddScoped<IInputModelFactory, InputModelFactory>();

        return services;
    }
}
