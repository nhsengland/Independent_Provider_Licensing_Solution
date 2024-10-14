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
using Okta.Sdk.Client;
using PnP.Core.Model.Security;
using Okta.Sdk.Abstractions;

namespace Licensing.Automation.Functions
{
    public record SPOAddUserToGroupData(string SiteUrl, string Group, string UserId);

    public class SPOAddUserToGroup
    {
        private readonly ILogger _logger;
        private readonly IPnPContextFactory _contextFactory;
        private readonly AzureFunctionSettings _azureFunctionSettings;
        
        public SPOAddUserToGroup(IPnPContextFactory pnpContextFactory, ILoggerFactory loggerFactory, AzureFunctionSettings azureFunctionSettings)
        {
            _logger = loggerFactory.CreateLogger<SPOAddUserToGroup>();
            _azureFunctionSettings = azureFunctionSettings;
            _contextFactory = pnpContextFactory;
        }

        [Function("SPOAddUserToGroup")]
        public async Task<HttpResponseData> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req, [FromBody] SPOAddUserToGroupData data)
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
                    
                    // ensure user exists in site
                    // NOTE: If we're adding a user that's only just been created, there can be a small delay before the user is 'seen' by
                    // SharePoint.  Therefore, some retry logic has been implemented (up to 2 minutes wait time)
                    ISharePointUser? spoUser = null;
                    PnP.Core.SharePointRestServiceException? currentException = null;
                    var startTime = DateTime.Now;
                    var retryCount = 1;
                                        
                    do
                    {
                        try
                        {
                            spoUser = await pnpContext.Web.EnsureUserAsync(graphUser.UserPrincipalName);
                            _logger.LogInformation($"Try {retryCount}. Principal {graphUser.UserPrincipalName} found.");
                            currentException = null;
                        }
                        catch (PnP.Core.SharePointRestServiceException ex)
                        {
                            currentException = ex;

                            _logger.LogInformation($"Try {retryCount}. Principal {graphUser.UserPrincipalName} not found.");

                            // sleep 5 seconds
                            Thread.Sleep(5000);
                        }
                        retryCount++;
                    } while (spoUser == null && (DateTime.Now - startTime).TotalMinutes < 2);
                    
                    if (null != currentException)
                    {
                        throw currentException;
                    }                    

                    // get group
                    ISharePointGroup? group;

                    if (data.Group.StartsWith('{') && data.Group.EndsWith('}'))
                    {
                        // group name is tokenised
                        await pnpContext.Web.LoadAsync(p => p.AssociatedMemberGroup, p => p.AssociatedOwnerGroup, p => p.AssociatedVisitorGroup);
                        switch (data.Group.Trim(['{','}']))
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


                    // add user to group
                    if (spoUser != null)
                    {
                        await group.Users.AddAsync(spoUser.LoginName);
                        return Utility.CreateResponse(req);
                    }
                    else
                    {
                        throw new Exception("User not found");
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
