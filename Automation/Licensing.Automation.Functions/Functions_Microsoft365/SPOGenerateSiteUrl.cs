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
    public class SPOGenerateSiteUrl
    {
        private readonly ILogger _logger;
        private readonly IPnPContextFactory _contextFactory;
        private readonly AzureFunctionSettings _azureFunctionSettings;
        
        public SPOGenerateSiteUrl(IPnPContextFactory pnpContextFactory, ILoggerFactory loggerFactory, AzureFunctionSettings azureFunctionSettings)
        {
            _logger = loggerFactory.CreateLogger<SPOGenerateSiteUrl>();
            _azureFunctionSettings = azureFunctionSettings;
            _contextFactory = pnpContextFactory;
        }

        [Function("SPOGenerateSiteUrl")]
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req)
        {
            try
            {
                return Utility.CreateResponse(req, new
                {
                    SiteUrl = string.Format("{0}/sites/{1}{2}", _azureFunctionSettings.M365RootSiteUrl, _azureFunctionSettings.M365SitePrefix, Guid.NewGuid().ToString("N"))
                });
            }
            catch (Exception ex)
            {
                return Utility.CreateResponse(req, ex);
            }
        }
    }
}
