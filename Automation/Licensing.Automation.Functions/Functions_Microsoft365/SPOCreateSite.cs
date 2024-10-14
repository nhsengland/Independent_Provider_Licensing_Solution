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
    public record CreateSiteData(string Title, string SiteUrl);

    public class SPOCreateSite
    {
        private readonly ILogger _logger;
        private readonly IPnPContextFactory _contextFactory;
        private readonly AzureFunctionSettings _azureFunctionSettings;
        
        public SPOCreateSite(IPnPContextFactory pnpContextFactory, ILoggerFactory loggerFactory, AzureFunctionSettings azureFunctionSettings)
        {
            _logger = loggerFactory.CreateLogger<SPOCreateSite>();
            _azureFunctionSettings = azureFunctionSettings;
            _contextFactory = pnpContextFactory;
        }

        [Function("SPOCreateSite")]
        public async Task<HttpResponseData> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req, [FromBody] CreateSiteData data)
        {
            try
            {
                using (var pnpContext = await _contextFactory.CreateAsync("Default"))
                {
                    var siteOptions = new TeamSiteWithoutGroupOptions(new Uri(data.SiteUrl), data.Title)
                    {
                        Owner = _azureFunctionSettings.M365SiteOwner
                    };

                    using (var newSite = await pnpContext.GetSiteCollectionManager().CreateSiteCollectionAsync(siteOptions))
                    {
                    
                        return Utility.CreateResponse(req, new
                        {
                            data.Title,
                            data.SiteUrl,
                            SiteId = newSite.Site.Id.ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                return Utility.CreateResponse(req, ex);
            }
        }
    }
}
