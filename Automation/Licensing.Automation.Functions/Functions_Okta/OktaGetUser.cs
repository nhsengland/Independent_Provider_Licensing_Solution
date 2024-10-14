using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Text;
using Newtonsoft.Json;
using System.Dynamic;

namespace Licensing.Automation.Functions
{
    public class OktaGetUser
    {
        private readonly ILogger _logger;
        private readonly AzureFunctionSettings _azureFunctionSettings;
        private readonly OktaConnection _oktaConnection;

        public OktaGetUser(ILoggerFactory loggerFactory, AzureFunctionSettings azureFunctionSettings, OktaConnection oktaConnection)
        {
            _logger = loggerFactory.CreateLogger<OktaGetUser>();
            _azureFunctionSettings = azureFunctionSettings;
            _oktaConnection = oktaConnection;
        }

        [Function("OktaGetUser")]
        public async Task<HttpResponseData> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req)
        {
            _logger.LogInformation($"OktaGetUser() {req.Query}");
            
            try
            {
                var userId = req.Query["userId"];
                bool enumGroups = false;

                if (string.IsNullOrEmpty(userId))
                {
                    throw new ArgumentException(Utility.OKTA_ERRORSTRING_NO_USERID);
                }

                bool.TryParse(req.Query["enumGroups"], out enumGroups);

                // get user
                var user = await _oktaConnection.User.GetUserAsync(userId);

                dynamic result = new ExpandoObject();
                
                result.User = new OktaUser(user);

                // get groups user is a member of
                if (enumGroups)
                {
                    var groups = _oktaConnection.User.ListUserGroups(user.Id).Select((group) => new OktaGroup(group)).ToBlockingEnumerable();
 
                    result.Groups = groups;
                }
                                
                return Utility.CreateResponse(req, result);
            }
            catch (Exception ex)
            {
                return Utility.CreateResponse(req, ex);
            }
        }
    }
}
