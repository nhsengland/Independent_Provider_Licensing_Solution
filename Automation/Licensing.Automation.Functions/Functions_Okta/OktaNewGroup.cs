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
    public record NewGroupData(string Name, string Description);

    public class OktaNewGroup
    {
        private readonly ILogger _logger;
        private readonly AzureFunctionSettings _azureFunctionSettings;
        private readonly OktaConnection _oktaConnection;
            
        public OktaNewGroup(ILoggerFactory loggerFactory, AzureFunctionSettings azureFunctionSettings, OktaConnection oktaConnection)
        {
            _logger = loggerFactory.CreateLogger<OktaNewGroup>();
            _azureFunctionSettings = azureFunctionSettings;
            _oktaConnection = oktaConnection;
        }

        [Function("OktaNewGroup")]
        public async Task<HttpResponseData> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req, [FromBody] NewGroupData data)
        {
            try
            {
                // create group
                var group = await _oktaConnection.Group.CreateGroupAsync(new Okta.Sdk.Model.Group()
                {
                    Profile = new Okta.Sdk.Model.GroupProfile()
                    {
                        Name = data.Name,
                        Description = data.Description
                    }
                });
                
                return Utility.CreateResponse(req, new OktaGroup(group));
            }
            catch (Exception ex)
            {
                return Utility.CreateResponse(req, ex);
            }
        }
    }
}
