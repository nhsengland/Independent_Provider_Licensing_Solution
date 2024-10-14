using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Text;
using Newtonsoft.Json;
using System.Dynamic;
using Microsoft.Graph;

namespace Licensing.Automation.Functions
{
    public record RemoveUserFromGroupData(string GroupId, string UserId);

    public class OktaRemoveUserFromGroup
    {
        private readonly ILogger _logger;
        private readonly AzureFunctionSettings _azureFunctionSettings;
        private readonly OktaConnection _oktaConnection;
            
        public OktaRemoveUserFromGroup(ILoggerFactory loggerFactory, AzureFunctionSettings azureFunctionSettings, OktaConnection oktaConnection)
        {
            _logger = loggerFactory.CreateLogger<OktaRemoveUserFromGroup>();
            _azureFunctionSettings = azureFunctionSettings;
            _oktaConnection = oktaConnection;
        }

        [Function("OktaRemoveUserFromGroup")]
        public async Task<HttpResponseData> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req, [FromBody] RemoveUserFromGroupData data)
        {
            try
            {
                var groupId = Utility.UntokenizeOktaGroupId(_azureFunctionSettings, data.GroupId);

                // remove user from group
                await _oktaConnection.Group.UnassignUserFromGroupAsync(groupId, data.UserId);
                
                return Utility.CreateResponse(req);
            }
            catch (Exception ex)
            {
                return Utility.CreateResponse(req, ex);
            }
        }
    }
}
