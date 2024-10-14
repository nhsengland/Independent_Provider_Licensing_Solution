using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PnP.Core.Services.Builder.Configuration;
using PnP.Core.Auth.Services.Builder.Configuration;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.Configuration;
using Okta.Sdk.Model;
using PnP.Core.Auth;

// example: https://github.com/pnp/pnpcore/blob/dev/samples/Demo.AzureFunction.OutOfProcess.AppOnly/Program.cs
namespace Licensing.Automation.Functions
{
    public class Program
    {
        public static void Main()
        {
            AzureFunctionSettings azureFunctionSettings = new AzureFunctionSettings();
        
            var host = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults()
                .ConfigureServices((context, services) =>
                {
                    services.AddApplicationInsightsTelemetryWorkerService();
                    services.ConfigureFunctionsApplicationInsights();
                    
                    // Add our global configuration instance
                    services.AddSingleton(options =>
                    {
                        var configuration = context.Configuration;
                        configuration.Bind(azureFunctionSettings);
                        return configuration;
                    });

                    // Add our configuration class
                    services.AddSingleton(options => { return azureFunctionSettings; });

                    // Add our Okta connection class
                    services.AddSingleton(options =>
                    {
                        return new OktaConnection(azureFunctionSettings);
                    });

                    // Add and configure PnP Core SDK
                    services.AddPnPCore(options =>
                    {
                        // Disable PnP telemetry
                        options.DisableTelemetry = true;

                        if (string.IsNullOrEmpty(azureFunctionSettings.M365LocalDevCertificateThumbprint))
                        {
                            // Configure Managed Identity Authentication
                            var authProvider = new ManagedIdentityTokenProvider();

                            options.DefaultAuthenticationProvider = authProvider;

                            options.Sites.Add("Default", new PnPCoreSiteOptions
                            {
                                SiteUrl = azureFunctionSettings.M365RootSiteUrl,
                                AuthenticationProvider = authProvider
                            });
                        }
                        else
                        {
                            // Configure Certificate Authentication
                            var authProvider = new X509CertificateAuthenticationProvider(azureFunctionSettings.M365LocalDevClientId,
                                azureFunctionSettings.M365LocalDevTenantId,
                                StoreName.My,
                                StoreLocation.CurrentUser,
                                azureFunctionSettings.M365LocalDevCertificateThumbprint);

                            options.DefaultAuthenticationProvider = authProvider;

                            options.Sites.Add("Default", new PnPCoreSiteOptions
                            {
                                SiteUrl = azureFunctionSettings.M365RootSiteUrl,
                                AuthenticationProvider = authProvider
                            });
                        }
                    });
                })
                .Build();

            host.Run();
        }
    }
}
