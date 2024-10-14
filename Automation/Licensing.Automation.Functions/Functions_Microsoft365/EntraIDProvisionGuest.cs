using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Text;
using Newtonsoft.Json;
using PnP.Core.Services;
using System.ComponentModel;
using Polly;
using Microsoft.Graph;

namespace Licensing.Automation.Functions
{
    public record EntraIDProvisionGuestData(string DisplayName, string EmailAddress, bool SendInvitationMessage = true);

    public class EntraIDProvisionGuest
    {
        private readonly ILogger _logger;
        private readonly AzureFunctionSettings _azureFunctionSettings;
        private readonly IPnPContextFactory _contextFactory;

        public EntraIDProvisionGuest(IPnPContextFactory pnpContextFactory, ILoggerFactory loggerFactory, AzureFunctionSettings azureFunctionSettings)
        {
            _logger = loggerFactory.CreateLogger<EntraIDProvisionGuest>();
            _azureFunctionSettings = azureFunctionSettings;
            _contextFactory = pnpContextFactory;
        }

        [Function("EntraIDProvisionGuest")]
        public async Task<HttpResponseData> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req, [FromBody] EntraIDProvisionGuestData data)
        {
            try
            {
                using (var pnpContext = await _contextFactory.CreateAsync("Default"))
                {
                    var graphServiceClient = new GraphServiceClient(new DelegateAuthenticationProvider((requestMessage) =>
                    {
                        return pnpContext.AuthenticationProvider.AuthenticateRequestAsync(new Uri("https://graph.microsoft.com"), requestMessage);
                    }));

                    // See if user already exists in Entra ID as a non-guest user (test against userPrincipalName)
                    var existingUserResult = await graphServiceClient.Users.Request()
                        .Filter($"userPrincipalName eq '{data.EmailAddress}' and userType eq 'Member'")
                        .Select(u => new
                        {
                            u.Id,
                            u.DisplayName,
                            u.UserPrincipalName
                        })
                        .GetAsync();

                    if (existingUserResult.Count > 0)
                    {
                        return Utility.CreateResponse(req, new
                        {
                            UserId = existingUserResult[0].Id,
                            existingUserResult[0].DisplayName,
                            EmailAddress = existingUserResult[0].UserPrincipalName,
                            Status = "ExistingMember"
                        });
                    }

                    // create guest user
                    var result = await graphServiceClient.Invitations.Request().AddAsync(new Invitation
                    {
                        InvitedUserDisplayName = data.DisplayName,
                        InvitedUserEmailAddress = data.EmailAddress,
                        InviteRedirectUrl = _azureFunctionSettings.M365InviteRedirectUrl,
                        SendInvitationMessage = data.SendInvitationMessage
                    });
                                        
                    return Utility.CreateResponse(req, new
                    {
                        UserId = result.InvitedUser.Id,
                        DisplayName = result.InvitedUserDisplayName,
                        EmailAddress = result.InvitedUserEmailAddress,
                        RedeemUrl = result.InviteRedeemUrl,
                        result.Status
                    });
                }
            }
            catch (Exception ex)
            {
                return Utility.CreateResponse(req, ex);
            }
        }
    }
}
