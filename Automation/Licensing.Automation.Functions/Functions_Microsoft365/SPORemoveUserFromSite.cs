using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Text;
using Newtonsoft.Json;
using PnP.Core.Services;
using System.ComponentModel;
using Polly;
using PnP.Framework.Sites;
using PnP.Core.Admin.Model.SharePoint;
using PnP.Framework.Provisioning.Providers.Xml.V201903;
using PnP.Framework.Provisioning.Providers.Xml;
using Microsoft.SharePoint.Client;
using PnP.Framework.Provisioning.ObjectHandlers;
using Microsoft.Online.SharePoint.TenantAdministration;
using Microsoft.Graph;
using Microsoft.Identity.Client;
using PnP.Core.Model.Security;

namespace Licensing.Automation.Functions
{
    public record SPORemoveUserFromSiteData(string SiteUrl, string UserId);

    public class SPORemoveUserFromSite
    {
        private readonly ILogger _logger;
        private readonly IPnPContextFactory _contextFactory;
        private readonly AzureFunctionSettings _azureFunctionSettings;
        
        public SPORemoveUserFromSite(IPnPContextFactory pnpContextFactory, ILoggerFactory loggerFactory, AzureFunctionSettings azureFunctionSettings)
        {
            _logger = loggerFactory.CreateLogger<SPORemoveUserFromSite>();
            _azureFunctionSettings = azureFunctionSettings;
            _contextFactory = pnpContextFactory;
        }

        [Function("SPORemoveUserFromSite")]
        public async Task<HttpResponseData> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req, [FromBody] SPORemoveUserFromSiteData data)
        {
            try
            {
                using (var pnpContext = await _contextFactory.CreateAsync(new Uri(data.SiteUrl)))
                {
                    // get UserPrincipalName via graph client
                    var graphServiceClient = new GraphServiceClient(new DelegateAuthenticationProvider((requestMessage) =>
                    {
                        return pnpContext.AuthenticationProvider.AuthenticateRequestAsync(new Uri("https://graph.microsoft.com"), requestMessage);
                    }));

                    var graphUser = await graphServiceClient.Users[data.UserId].Request().GetAsync();
                                                                                
                    // get user
                    var spoUser = await pnpContext.Web.SiteUsers.FirstOrDefaultAsync(u => string.Equals(u.UserPrincipalName, graphUser.UserPrincipalName, StringComparison.OrdinalIgnoreCase));
                    
                    if (spoUser != null)
                    {
                        // delete user
                        await spoUser.DeleteAsync();
                    }

                    return Utility.CreateResponse(req);
                }
            }
            catch (Exception ex)
            {
                return Utility.CreateResponse(req, ex);
            }
        }
    }
}
