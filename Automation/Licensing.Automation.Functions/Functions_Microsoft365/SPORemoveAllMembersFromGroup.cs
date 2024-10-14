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
    public record SPORemoveAllMembersFromGroupData(string SiteUrl, string Group);

    public class SPORemoveAllMembersFromGroup
    {
        private readonly ILogger _logger;
        private readonly IPnPContextFactory _contextFactory;
        private readonly AzureFunctionSettings _azureFunctionSettings;

        public SPORemoveAllMembersFromGroup(IPnPContextFactory pnpContextFactory, ILoggerFactory loggerFactory, AzureFunctionSettings azureFunctionSettings)
        {
            _logger = loggerFactory.CreateLogger<SPORemoveAllMembersFromGroup>();
            _azureFunctionSettings = azureFunctionSettings;
            _contextFactory = pnpContextFactory;
        }

        [Function("SPORemoveAllMembersFromGroup")]
        public async Task<HttpResponseData> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req, [FromBody] SPORemoveAllMembersFromGroupData data)
        {
            try
            {
                using (var pnpContext = await _contextFactory.CreateAsync(new Uri(data.SiteUrl)))
                {
                    // get group
                    ISharePointGroup? group;

                    if (data.Group.StartsWith('{') && data.Group.EndsWith('}'))
                    {
                        // group name is tokenised
                        await pnpContext.Web.LoadAsync(p => p.AssociatedMemberGroup, p => p.AssociatedOwnerGroup, p => p.AssociatedVisitorGroup);
                        switch (data.Group.Trim(['{', '}']))
                        {
                            case Utility.SPO_OWNERGROUP_TOKEN:
                                group = pnpContext.Web.AssociatedOwnerGroup; break;
                            case Utility.SPO_MEMBERGROUP_TOKEN:
                                group = pnpContext.Web.AssociatedMemberGroup; break;
                            case Utility.SPO_VISITORGROUP_TOKEN:
                                group = pnpContext.Web.AssociatedVisitorGroup; break;
                            default:
                                return Utility.CreateResponse(req, HttpStatusCode.BadRequest, Utility.SPO_ERRORSTRING_UNEXPECTED_GROUP_TOKEN);
                        }
                    }
                    else
                    {
                        // use group name from request
                        group = await pnpContext.Web.SiteGroups.FirstOrDefaultAsync(g => g.Title == data.Group) ?? throw new PnP.Core.SharePointRestServiceException(PnP.Core.ErrorType.SharePointRestServiceError, 404, Utility.SPO_ERRORSTRING_GROUP_NOT_FOUND);
                    }
                    
                    foreach(var spoUser in group.Users)
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
