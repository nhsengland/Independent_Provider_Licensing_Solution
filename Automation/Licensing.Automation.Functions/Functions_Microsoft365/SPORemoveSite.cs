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

namespace Licensing.Automation.Functions
{
    public record RemoveSiteData(string SiteUrl);

    public class SPORemoveSite
    {
        private readonly ILogger _logger;
        private readonly IPnPContextFactory _contextFactory;
        private readonly AzureFunctionSettings _azureFunctionSettings;
        
        public SPORemoveSite(IPnPContextFactory pnpContextFactory, ILoggerFactory loggerFactory, AzureFunctionSettings azureFunctionSettings)
        {
            _logger = loggerFactory.CreateLogger<SPORemoveSite>();
            _azureFunctionSettings = azureFunctionSettings;
            _contextFactory = pnpContextFactory;
        }
        [Function("SPORemoveSite")]
        public async Task<HttpResponseData> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "delete")] HttpRequestData req, [FromBody] RemoveSiteData data)
        {
            try
            {
                using (var pnpContext = await _contextFactory.CreateAsync("Default"))
                {
                    await pnpContext.GetSiteCollectionManager().RecycleSiteCollectionAsync(new Uri(data.SiteUrl));

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
