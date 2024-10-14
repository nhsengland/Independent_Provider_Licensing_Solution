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
using PnP.Core.QueryModel;

namespace Licensing.Automation.Functions
{
    public record CreateFolderPathData(string SiteUrl, string FolderPath, bool CreateUploadAndDownloadFolders);

    public class SPOCreateFolderPath
    {
        private readonly ILogger _logger;
        private readonly IPnPContextFactory _contextFactory;
        private readonly AzureFunctionSettings _azureFunctionSettings;
        
        public SPOCreateFolderPath(IPnPContextFactory pnpContextFactory, ILoggerFactory loggerFactory, AzureFunctionSettings azureFunctionSettings)
        {
            _logger = loggerFactory.CreateLogger<SPOCreateFolderPath>();
            _azureFunctionSettings = azureFunctionSettings;
            _contextFactory = pnpContextFactory;
        }

        [Function("SPOCreateFolderPath")]
        public async Task<HttpResponseData> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req, [FromBody] CreateFolderPathData data)
        {
            try
            {
                using (var pnpContext = await _contextFactory.CreateAsync(new Uri(data.SiteUrl)))
                {
                    // get document library root folder
                    var folder = (await pnpContext.Web.Lists.GetByTitleAsync(Utility.SPO_LISTTITLE_DOCUMENTS, p => p.RootFolder)).RootFolder;

                    // create folder structure
                    var subFolder = await folder.EnsureFolderAsync(data.FolderPath);
                    
                    // create upload and download folders
                    if (data.CreateUploadAndDownloadFolders)
                    {
                        var uploadFolder = await subFolder.EnsureFolderAsync(Utility.SPO_FOLDER_UPLOAD);
                        var downloadFolder = await subFolder.EnsureFolderAsync(Utility.SPO_FOLDER_DOWNLOAD);

                        // grant visitors the 'Guest Contribute' role on the upload folder
                        await pnpContext.Web.LoadAsync(p => p.AssociatedVisitorGroup, p => p.RoleDefinitions);
                        var guestContributeRole = pnpContext.Web.RoleDefinitions.AsRequested().FirstOrDefault(p => p.Name == Utility.SPO_ROLE_GUEST_CONTRIBUTE);
                        var guestReadRole = pnpContext.Web.RoleDefinitions.AsRequested().FirstOrDefault(p => p.Name == Utility.SPO_ROLE_GUEST_READ);
                        await uploadFolder.ListItemAllFields.BreakRoleInheritanceAsync(true, true);
                        await uploadFolder.ListItemAllFields.RemoveRoleDefinitionAsync(pnpContext.Web.AssociatedVisitorGroup.Id, guestReadRole);
                        await uploadFolder.ListItemAllFields.AddRoleDefinitionAsync(pnpContext.Web.AssociatedVisitorGroup.Id, guestContributeRole);
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
