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
using PnP.Core.QueryModel;
using PnP.Framework.Provisioning.Model;
using PnP.Core;
using System.Text.RegularExpressions;

namespace Licensing.Automation.Functions
{
    public record SPOMaintainManagedMembersGroupData(string SiteUrl, bool IsListed, string[] UserIds);

    public class SPOMaintainManagedMembersGroup
    {
        private readonly ILogger _logger;
        private readonly IPnPContextFactory _contextFactory;
        private readonly AzureFunctionSettings _azureFunctionSettings;
        
        public SPOMaintainManagedMembersGroup(IPnPContextFactory pnpContextFactory, ILoggerFactory loggerFactory, AzureFunctionSettings azureFunctionSettings)
        {
            _logger = loggerFactory.CreateLogger<SPOMaintainManagedMembersGroup>();
            _azureFunctionSettings = azureFunctionSettings;
            _contextFactory = pnpContextFactory;
        }

        [Function("SPOMaintainManagedMembersGroup")]
        public async Task<HttpResponseData> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req, [FromBody] SPOMaintainManagedMembersGroupData data)
        {
            try
            {
                var userIds = data.UserIds.Where(x => !string.IsNullOrEmpty(x)).Distinct().Select(x => x.ToLower()).ToArray();

                using (var pnpContext = await _contextFactory.CreateAsync(new Uri(data.SiteUrl)))
                {
                    // Get existing group members
                    var group = await ((IQueryable<ISharePointGroup>)pnpContext.Web.SiteGroups).FirstOrDefaultAsync(g => g.Title == Utility.SPO_MANAGED_MEMBERS_GROUP) ?? throw new PnP.Core.SharePointRestServiceException(PnP.Core.ErrorType.SharePointRestServiceError, 404, Utility.SPO_ERRORSTRING_GROUP_NOT_FOUND);
                    var existingUsers = await group.Users.QueryProperties(p => p.AadObjectId, p => p.Title, p => p.UserPrincipalName, p => p.LoginName).ToListAsync();
                                        
                    // add users
                    var usersToAdd = userIds.Where(p => !existingUsers.Select(e => e.AadObjectId.ToLower()).Contains(p));

                    if (usersToAdd.Any())
                    {
                        // initialise graph client
                        var graphServiceClient = new GraphServiceClient(new DelegateAuthenticationProvider((requestMessage) =>
                        {
                            return pnpContext.AuthenticationProvider.AuthenticateRequestAsync(new Uri("https://graph.microsoft.com"), requestMessage);
                        }));

                        foreach(var userToAdd in usersToAdd)
                        {
                            try
                            {
                                // get user from graph api
                                var graphUser = await graphServiceClient.Users[userToAdd].Request().Select("Id,DisplayName,UserPrincipalName,AccountEnabled").GetAsync();
                                
                                // check user is enabled before adding (NB. Sharepoint will throw an error if the user's account is disabled)
                                if (graphUser.AccountEnabled == true) {
                                    
                                    // ensure user in Sharepoint site
                                    var spoUser = await pnpContext.Web.EnsureUserAsync(graphUser.UserPrincipalName);

                                    // add user to group
                                    await group.Users.AddAsync(spoUser.LoginName);
                                }
                            }
                            catch (Microsoft.Graph.ServiceException ex)
                            {
                                if (ex.StatusCode != System.Net.HttpStatusCode.NotFound)
                                {
                                    // rethrow exception if anything other than user not found
                                    throw;
                                }
                            }
                            catch (PnP.Core.SharePointRestServiceException ex)
                            {
                                if (ex.Error is SharePointRestError sharePointRestError)
                                {
                                    if (sharePointRestError.HttpResponseCode != 400 || !Regex.IsMatch(sharePointRestError.Message,"could not be found.$")) { 
                                        // Ignore edge case where user account has just been reenabled in Entra ID, but Sharepoint still thinks the user
                                        // doesn't exist.
                                        throw;
                                    }
                                }
                                else { throw; }
                            }
                        }
                    }

                    // remove users
                    if (existingUsers.Any())
                    {
                        foreach (var userToRemove in existingUsers.Where(p => !userIds.Contains(p.AadObjectId.ToLower())))
                        {
                            await userToRemove.DeleteAsync();
                        }
                    }

                    if(!string.IsNullOrEmpty(_azureFunctionSettings.M365IndependentProviderTeamEntraIDGroup))
                    {
                        await pnpContext.Web.LoadAsync(p => p.AssociatedMemberGroup);
                        
                        if (!data.IsListed)
                        {
                            // ensure IP Licensing Team group in Sharepoint site
                            var spoIPTeamGroupUser = await pnpContext.Web.EnsureUserAsync(_azureFunctionSettings.M365IndependentProviderTeamEntraIDGroup);

                            // add IP Licensing Team group to 'Members' SharePoint group
                            
                            await pnpContext.Web.AssociatedMemberGroup.Users.AddAsync(spoIPTeamGroupUser.LoginName);
                        }
                        else
                        {
                            // remove IP Licensing Team group from 'Members' SharePoint group
                            var spoIPTeamGroupUser = await ((IQueryable<ISharePointUser>)pnpContext.Web.AssociatedMemberGroup.Users).FirstOrDefaultAsync(p => p.Title == _azureFunctionSettings.M365IndependentProviderTeamEntraIDGroup);
                            if (spoIPTeamGroupUser != null)
                            {
                                await spoIPTeamGroupUser.DeleteAsync();
                            }
                        }
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
