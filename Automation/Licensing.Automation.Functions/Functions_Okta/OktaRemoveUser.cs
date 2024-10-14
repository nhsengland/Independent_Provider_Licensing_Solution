// disabled RemoveUser capability since users could use Okta for purposes other than IPLS
// Use OktaRemoveUserFromGroup with a GroupId of 'licensing' or 'application' instead.

/*
using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Text;
using Newtonsoft.Json;
using System.Dynamic;

namespace Licensing.Automation.Functions
{
    public class OktaRemoveUser
    {
        private readonly ILogger _logger;
        private readonly AzureFunctionSettings _azureFunctionSettings;
        private readonly OktaConnection _oktaConnection;

        public OktaRemoveUser(ILoggerFactory loggerFactory, AzureFunctionSettings azureFunctionSettings, OktaConnection oktaConnection)
        {
            _logger = loggerFactory.CreateLogger<OktaRemoveUser>();
            _azureFunctionSettings = azureFunctionSettings;
            _oktaConnection = oktaConnection;
        }

        [Function("OktaRemoveUser")]
        public async Task<HttpResponseData> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "delete")] HttpRequestData req)
        {
            _logger.LogInformation($"OktaRemoveUser() {req.Query}");
            
            try
            {
                var userId = req.Query["userId"];
                
                if (string.IsNullOrEmpty(userId))
                {
                    throw new ArgumentException(Utility.OKTA_ERRORSTRING_NO_USERID);
                }

                // Get user
                var user = new OktaUser(await _oktaConnection.User.GetUserAsync(userId));

                // Deactivate user
                if (user.Status != "DEPROVISIONED")
                {
                    await _oktaConnection.User.DeactivateUserAsync(userId);
                }

                // delete user
                await _oktaConnection.User.DeleteUserAsync(userId);
                                                
                return Utility.CreateResponse(req);
            }
            catch (Exception ex)
            {
                return Utility.CreateResponse(req, ex);
            }
        }
    }
}
*/