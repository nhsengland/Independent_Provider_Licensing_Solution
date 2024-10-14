using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Text;
using Newtonsoft.Json;
using System.Dynamic;
using Microsoft.Graph;
using AngleSharp.Css.Dom;

namespace Licensing.Automation.Functions
{
    public record EnsureUserData(string Application, string UserId, string FirstName, string LastName, string Email, string PrimaryPhone, string Organization);

    public class OktaEnsureUser
    {
        private readonly ILogger _logger;
        private readonly AzureFunctionSettings _azureFunctionSettings;
        private readonly OktaConnection _oktaConnection;
            
        public OktaEnsureUser(ILoggerFactory loggerFactory, AzureFunctionSettings azureFunctionSettings, OktaConnection oktaConnection)
        {
            _logger = loggerFactory.CreateLogger<OktaEnsureUser>();
            _azureFunctionSettings = azureFunctionSettings;
            _oktaConnection = oktaConnection;
        }

        [Function("OktaEnsureUser")]
        public async Task<HttpResponseData> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req, [FromBody] EnsureUserData data)
        {
            try
            {
                dynamic result = new ExpandoObject();
                result.ExistingUser = false;
                result.ActivationUrl = string.Empty;

                if (string.IsNullOrEmpty(data.Application))
                {
                    throw new ArgumentException(Utility.OKTA_ERRORSTRING_NO_APP);
                }

                // Check for existing user
                if (string.IsNullOrEmpty(data.UserId))
                {
                    try
                    {
                        // locate user by email address
                        result.User = new OktaUser(await _oktaConnection.User.GetUserAsync(data.Email));
                        result.ExistingUser = true;
                    }
                    catch (Okta.Sdk.Client.ApiException ex)
                    {
                        // throw anything other than a 404
                        // (NB. If we receive a 404 we'll proceed to create the user)
                        if (ex.ErrorCode != 404)
                        {
                            throw;
                        }
                    }
                }
                else
                {
                    // locate user by Okta User ID
                    // In this scenario, user must exist otherwise an exception will be thrown
                    result.User = new OktaUser(await _oktaConnection.User.GetUserAsync(data.UserId));
                    result.ExistingUser = true;
                }
                
                var groupId = Utility.UntokenizeOktaGroupId(_azureFunctionSettings, data.Application, true);

                if (result.ExistingUser)
                {
                    // reactivate user if required, suppress email notification
                    switch (result.User.Status)
                    {
                        case "PROVISIONED":
                            result.ActivationUrl = (await _oktaConnection.User.ReactivateUserAsync(result.User.Id, false)).ActivationUrl;
                            break;
                        case "STAGED":
                        case "DEPROVISIONED":
                            result.ActivationUrl = (await _oktaConnection.User.ActivateUserAsync(result.User.Id, false)).ActivationUrl;
                            break;
                    }

                    // ensure that users is added to the correct group
                    // (NB. User may already exist in Okta, but may not currently exist in group)
                    await _oktaConnection.Group.AssignUserToGroupAsync(groupId, result.User.Id);

                    return Utility.CreateResponse(req, result);
                }

                // create new user and add to group (user will only be staged, and no activation email will be sent)
                result.User = new OktaUser(await _oktaConnection.User.CreateUserAsync(new Okta.Sdk.Model.CreateUserRequest()
                {
                    Profile = new Okta.Sdk.Model.UserProfile()
                    {
                        FirstName = data.FirstName,
                        LastName = data.LastName,
                        Email = data.Email,
                        Login = data.Email,
                        PrimaryPhone = data.PrimaryPhone,
                        Organization = data.Organization
                    },
                    GroupIds = [groupId],
                }, false));
                
                // Get activation url, suppress email notification
                result.ActivationUrl = (await _oktaConnection.User.ActivateUserAsync(result.User.Id, false)).ActivationUrl;
                                
                return Utility.CreateResponse(req, result);
            }
            catch (Exception ex)
            {
                return Utility.CreateResponse(req, ex);
            }
        }
    }
}
