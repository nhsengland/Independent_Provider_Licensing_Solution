using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Text;
using Newtonsoft.Json;
using System.Dynamic;

namespace Licensing.Automation.Functions
{
    public class OktaGetGroupMembers
    {
        private readonly ILogger _logger;
        private readonly AzureFunctionSettings _azureFunctionSettings;
        private readonly OktaConnection _oktaConnection;

        public OktaGetGroupMembers(ILoggerFactory loggerFactory, AzureFunctionSettings azureFunctionSettings, OktaConnection oktaConnection)
        {
            _logger = loggerFactory.CreateLogger<OktaGetGroupMembers>();
            _azureFunctionSettings = azureFunctionSettings;
            _oktaConnection = oktaConnection;
        }

        [Function("OktaGetGroupMembers")]
        public async Task<HttpResponseData> RunAsync([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req)
        {
            _logger.LogInformation($"OktaGetGroupMembers() {req.Query}");
            
            try
            {
                if (string.IsNullOrEmpty(req.Query["groupId"]))
                {
                    throw new ArgumentException(Utility.OKTA_ERRORSTRING_NO_GROUPID);
                }

                var groupId = Utility.UntokenizeOktaGroupId(_azureFunctionSettings, req.Query["groupId"]);
 
                // get group members
                var users = _oktaConnection.Group.ListGroupUsers(groupId).Select((user) => new OktaUser(user)).ToBlockingEnumerable();

                return Utility.CreateResponse(req, users);
            }
            catch (Exception ex)
            {
                return Utility.CreateResponse(req, ex);
            }
        }
    }
}
