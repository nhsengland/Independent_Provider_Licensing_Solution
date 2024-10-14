using Azure.Core;
using Azure.Identity;
using Domain.Logic.Integration.Email.Configuration;
using Microsoft.Extensions.Hosting;

namespace Domain.Logic.Integration.Email.Factories;
public class AccessTokenFactory(
    EmailConfiguration configuration,
    IHostEnvironment environment) : IAccessTokenFactory
{
    private readonly EmailConfiguration configuration = configuration;
    private readonly IHostEnvironment environment = environment;

    public async Task<string> CreateAsync()
    {
        var credential = new ManagedIdentityCredential();
        var context = new TokenRequestContext([configuration.APIAudienceHeader]);

        if (environment.IsDevelopment())
        {
            return "DEBUG_TOKEN";
        }

        return (await credential.GetTokenAsync(context)).Token;
    }
}
