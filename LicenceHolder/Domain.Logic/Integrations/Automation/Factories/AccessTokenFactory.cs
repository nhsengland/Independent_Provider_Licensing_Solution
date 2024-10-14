
using Azure.Core;
using Azure.Identity;
using Microsoft.Extensions.Hosting;

namespace Domain.Logic.Integrations.Automation.Factories;

public class AccessTokenFactory(
    AutomationConfiguration automationConfiguration,
    IHostEnvironment environment) : IAccessTokenFactory
{
    private readonly AutomationConfiguration automationConfiguration = automationConfiguration;

    public async Task<string> CreateAsync()
    {
        var credential = new ManagedIdentityCredential();
        var context = new TokenRequestContext([automationConfiguration.APIAudienceHeader]);
        
        if (environment.IsDevelopment())
        {
            return "DEBUG_TOKEN";
        }
        
        var accessToken = await credential.GetTokenAsync(context);
        return accessToken.Token;
    }
}
