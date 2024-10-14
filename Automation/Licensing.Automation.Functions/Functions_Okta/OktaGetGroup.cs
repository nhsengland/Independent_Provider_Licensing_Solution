using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Text;
using Newtonsoft.Json;
using System.Dynamic;
using Okta.Sdk.Client;

namespace Licensing.Automation.Functions
{
    public class OktaGetGroup
    {
        private readonly ILogger _logger;
        private readonly AzureFunctionSettings _azureFunctionSettings;
        private readonly OktaConnection _oktaConnection;

        public OktaGetGroup(ILoggerFactory loggerFactory, AzureFunctionSettings azureFunctionSettings, OktaConnection oktaConnection)
        {
            _logger = loggerFactory.CreateLogger<OktaGetGroup>();
            _azureFunctionSettings = azureFunctionSettings;
            _oktaConnection = oktaConnection;
        }

        [Function("OktaGetGroup")]
        public async Task<HttpResponseData> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req)
        {
            _logger.LogInformation($"OktaGetGroup() {req.Query}");
            
            try
            {
                var name = req.Query["name"];
                Okta.Sdk.Model.Group? group;

                if (!string.IsNullOrEmpty(req.Query["groupId"]))
                {
                    var groupId = Utility.UntokenizeOktaGroupId(_azureFunctionSettings, req.Query["groupId"]);
                    group = await _oktaConnection.Group.GetGroupAsync(groupId);
                }
                else if (!string.IsNullOrEmpty(name)) {

                    group = _oktaConnection.Group.ListGroups(search: $"type eq \"OKTA_GROUP\" and profile.name eq \"{name}\"", limit: 1).ToBlockingEnumerable().FirstOrDefault();

                    if (group == null) {
                        return req.CreateResponse(HttpStatusCode.NotFound);
                    }
                }
                else
                {
                    throw new ArgumentException(Utility.OKTA_ERRORSTRING_NO_GROUPID_OR_NAME);
                }

                return Utility.CreateResponse(req, new OktaGroup(group));

            }
            catch (Exception ex)
            {
                return Utility.CreateResponse(req, ex);
            }
        }
    }
}
