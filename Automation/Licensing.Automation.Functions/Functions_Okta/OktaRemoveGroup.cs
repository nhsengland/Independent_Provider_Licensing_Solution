using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Text;
using Newtonsoft.Json;
using System.Dynamic;

namespace Licensing.Automation.Functions
{
    public class OktaRemoveGroup
    {
        private readonly ILogger _logger;
        private readonly AzureFunctionSettings _azureFunctionSettings;
        private readonly OktaConnection _oktaConnection;

        public OktaRemoveGroup(ILoggerFactory loggerFactory, AzureFunctionSettings azureFunctionSettings, OktaConnection oktaConnection)
        {
            _logger = loggerFactory.CreateLogger<OktaRemoveGroup>();
            _azureFunctionSettings = azureFunctionSettings;
            _oktaConnection = oktaConnection;
        }

        [Function("OktaRemoveGroup")]
        public async Task<HttpResponseData> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "delete")] HttpRequestData req)
        {
            _logger.LogInformation($"OktaRemoveGroup() {req.Query}");
            
            try
            {
                var groupId = req.Query["groupId"];
                
                if (string.IsNullOrEmpty(groupId))
                {
                    throw new ArgumentException(Utility.OKTA_ERRORSTRING_NO_GROUPID);
                }

                // delete group
                await _oktaConnection.Group.DeleteGroupAsync(groupId);
                                                 
                return Utility.CreateResponse(req);
            }
            catch (Exception ex)
            {
                return Utility.CreateResponse(req, ex);
            }
        }
    }
}
