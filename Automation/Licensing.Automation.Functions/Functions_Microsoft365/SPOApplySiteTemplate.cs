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
    public record ApplySiteTemplateData(string SiteUrl, bool PopulateDefaultMemberGroup = true, bool HideNewButton = true);

    public class SPOApplySiteTemplate
    {
        private readonly ILogger _logger;
        private readonly IPnPContextFactory _contextFactory;
        private readonly AzureFunctionSettings _azureFunctionSettings;
        private readonly string _templateXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
    <pnp:Provisioning xmlns:pnp=""http://schemas.dev.office.com/PnP/2022/09/ProvisioningSchema"">
    <pnp:Preferences Generator=""PnP.Framework, Version=1.11.69.0, Culture=neutral, PublicKeyToken=0d501f89f11b748c"" >
        <pnp:Parameters>
            <pnp:Parameter Key=""M365SiteAdminsEntraIDGroup""/>
            <pnp:Parameter Key=""M365IndependentProviderTeamEntraIDGroup""/>
            <pnp:Parameter Key=""ManagedMembersGroupName""/>
            <pnp:Parameter Key=""HideNewButton""/>
        </pnp:Parameters>
    </pnp:Preferences>
    <pnp:Templates ID=""CONTAINER-TEMPLATE-020B87BD5D0C47189C21F84BDBCFBAAA"">
        <pnp:ProvisioningTemplate ID=""TEMPLATE-020B87BD5D0C47189C21F84BDBCFBAAA"" Version=""1"" BaseSiteTemplate=""STS#3"" Scope=""RootSite"">
        <pnp:WebSettings RequestAccessEmail="""" NoCrawl=""true"" WelcomePage=""Shared Documents/Forms/AllItems.aspx"" SiteLogo="""" AlternateCSS="""" MasterPageUrl=""{masterpagecatalog}/seattle.master"" CustomMasterPageUrl=""{masterpagecatalog}/seattle.master"" CommentsOnSitePagesDisabled=""true"" QuickLaunchEnabled=""false"" SearchScope=""DefaultScope"" ExcludeFromOfflineClient=""true"" MembersCanShare=""false"" DisableFlows=""false"" DisableAppViews=""true"" SearchBoxInNavBar=""Hidden""/>
        <pnp:SiteSettings AllowDesigner=""false"" AllowCreateDeclarativeWorkflow=""false"" AllowSaveDeclarativeWorkflowAsTemplate=""false"" AllowSavePublishDeclarativeWorkflow=""false"" SearchBoxInNavBar=""Hidden"" SearchCenterUrl="""" SocialBarOnSitePagesDisabled=""true"" />
        <pnp:RegionalSettings AdjustHijriDays=""0"" AlternateCalendarType=""None"" CalendarType=""Gregorian"" Collation=""25"" FirstDayOfWeek=""Sunday"" FirstWeekOfYear=""0"" LocaleId=""2057"" ShowWeeks=""false"" Time24=""false"" TimeZone=""2"" WorkDayEndHour=""5:00PM"" WorkDays=""62"" WorkDayStartHour=""8:00AM"" />
        <pnp:Security AssociatedOwnerGroup=""{groupsitetitle} Owners"" AssociatedMemberGroup=""{groupsitetitle} Members"" AssociatedVisitorGroup=""{groupsitetitle} Visitors"">
            <pnp:AdditionalAdministrators>
                <pnp:User Name=""{parameter:M365SiteAdminsEntraIDGroup}"" />
            </pnp:AdditionalAdministrators>
            <pnp:AdditionalMembers>
                <pnp:User Name=""{parameter:M365IndependentProviderTeamEntraIDGroup}"" />
            </pnp:AdditionalMembers>
            <pnp:SiteGroups>
                <pnp:SiteGroup Title=""{parameter:ManagedMembersGroupName}"" Description=""This group is managed by the Provider Licensing Power Platform Solution.  Please do not manually add users to this group. "" />
            </pnp:SiteGroups>
              <pnp:Permissions>
                <pnp:RoleDefinitions>
                    <pnp:RoleDefinition Name=""Guest Read"" Description="""">
                      <pnp:Permissions>
                        <pnp:Permission>EmptyMask</pnp:Permission>
                        <pnp:Permission>ViewListItems</pnp:Permission>
                        <pnp:Permission>OpenItems</pnp:Permission>
                        <pnp:Permission>ViewFormPages</pnp:Permission>
                        <pnp:Permission>Open</pnp:Permission>
                        <pnp:Permission>ViewPages</pnp:Permission>
                        <pnp:Permission>UseClientIntegration</pnp:Permission>
                        <pnp:Permission>UseRemoteAPIs</pnp:Permission>
                      </pnp:Permissions>
                    </pnp:RoleDefinition>
                    <pnp:RoleDefinition Name=""Guest Contribute"" Description="""">
                      <pnp:Permissions>
                        <pnp:Permission>EmptyMask</pnp:Permission>
                        <pnp:Permission>ViewListItems</pnp:Permission>
                        <pnp:Permission>AddListItems</pnp:Permission>
                        <pnp:Permission>EditListItems</pnp:Permission>
                        <pnp:Permission>DeleteListItems</pnp:Permission>
                        <pnp:Permission>OpenItems</pnp:Permission>
                        <pnp:Permission>ViewFormPages</pnp:Permission>
                        <pnp:Permission>Open</pnp:Permission>
                        <pnp:Permission>ViewPages</pnp:Permission>
                        <pnp:Permission>BrowseDirectories</pnp:Permission>
                        <pnp:Permission>UseClientIntegration</pnp:Permission>
                        <pnp:Permission>UseRemoteAPIs</pnp:Permission>
                      </pnp:Permissions>
                    </pnp:RoleDefinition>
                </pnp:RoleDefinitions>
                <pnp:RoleAssignments>
                    <pnp:RoleAssignment Principal=""{associatedmembergroup}"" RoleDefinition=""{roledefinition:Editor}"" Remove=""true"" />
                    <pnp:RoleAssignment Principal=""{associatedvisitorgroup}"" RoleDefinition=""{roledefinition:Reader}"" Remove=""true"" />
                    <pnp:RoleAssignment Principal=""{associatedmembergroup}"" RoleDefinition=""{roledefinition:Contributor}"" />
                    <pnp:RoleAssignment Principal=""{associatedvisitorgroup}"" RoleDefinition=""Guest Read"" />
                    <pnp:RoleAssignment Principal=""{parameter:ManagedMembersGroupName}"" RoleDefinition=""{roledefinition:Contributor}"" />
                </pnp:RoleAssignments>
            </pnp:Permissions>
        </pnp:Security>
        <pnp:Navigation AddNewPagesToNavigation=""true"" CreateFriendlyUrlsForNewPages=""true"">
            <pnp:GlobalNavigation NavigationType=""Structural"">
            <pnp:StructuralNavigation RemoveExistingNodes=""false"" />
            </pnp:GlobalNavigation>
            <pnp:CurrentNavigation NavigationType=""StructuralLocal"">
            <pnp:StructuralNavigation RemoveExistingNodes=""true"">
                <pnp:NavigationNode Title=""Documents"" Url=""{site}/Shared Documents/Forms/AllItems.aspx"" />
            </pnp:StructuralNavigation>
            </pnp:CurrentNavigation>
        </pnp:Navigation>
        <pnp:Lists>
            <pnp:ListInstance Title=""Documents"" Description="""" DocumentTemplate=""{site}/Shared Documents/Forms/template.dotx"" OnQuickLaunch=""true"" TemplateType=""101"" Url=""Shared Documents"" EnableVersioning=""true"" MinorVersionLimit=""0"" MaxVersionLimit=""500"" DraftVersionVisibility=""0"" TemplateFeatureID=""00bfea71-e717-4e80-aa17-d0c71b360101"" EnableAttachments=""false"" DefaultDisplayFormUrl=""{site}/Shared Documents/Forms/DispForm.aspx"" DefaultEditFormUrl=""{site}/Shared Documents/Forms/EditForm.aspx"" DefaultNewFormUrl=""{site}/Shared Documents/Forms/Upload.aspx"" ImageUrl=""/_layouts/15/images/itdl.png?rev=47"" IrmExpire=""false"" IrmReject=""false"" IsApplicationList=""false"" ValidationFormula="""" ValidationMessage="""">
            <pnp:Views>
                <View Name=""{A7B07D08-6E4F-460A-A7C9-202F1F7C55A3}"" DefaultView=""TRUE"" MobileView=""TRUE"" MobileDefaultView=""TRUE"" Type=""HTML"" DisplayName=""All Documents"" Url=""{site}/Shared Documents/Forms/AllItems.aspx"" Level=""1"" BaseViewID=""1"" ContentTypeID=""0x"" ImageUrl=""/_layouts/15/images/dlicon.png?rev=47"">
                  <Query>
                    <OrderBy>
                      <FieldRef Name=""FileLeafRef"" />
                    </OrderBy>
                  </Query>
                  <ViewFields>
                    <FieldRef Name=""DocIcon"" />
                    <FieldRef Name=""LinkFilename"" />
                    <FieldRef Name=""Modified"" />
                  </ViewFields>
                  <RowLimit Paged=""TRUE"">30</RowLimit>
                  <Aggregations Value=""Off"" />
                  <JSLink>clienttemplates.js</JSLink>
                  <CustomFormatter><![CDATA[{""$schema"":""https://developer.microsoft.com/json-schemas/sp/v2/row-formatting.schema.json"",""commandBarProps"":{""commands"":[{""key"":""export"",""hide"":true},{""key"":""pinToQuickAccess"",""hide"":true},{""key"":""automate"",""hide"":true},{""key"":""integrate"",""hide"":true},{""key"":""editInGridView"",""hide"":true},{""key"":""sync"",""hide"":true},{""key"":""new"",""hide"":{parameter:HideNewButton}},{""key"":""newWordDocument"",""hide"":true},{""key"":""newExcelWorkbook"",""hide"":true},{""key"":""newPowerPointPresentation"",""hide"":true},{""key"":""newOneNoteNotebook"",""hide"":true},{""key"":""newFormsForExcel"",""hide"":true},{""key"":""newVisioDrawing"",""hide"":true},{""key"":""newLink"",""hide"":true},{""key"":""newVideo"",""hide"":true},{""key"":""addTemplate"",""hide"":true},{""key"":""editNewMenu"",""hide"":true},{""key"":""addShortcut"",""hide"":true},{""key"":""alertMe"",""hide"":true},{""key"":""manageAlert"",""hide"":true},{""key"":""uploadTemplate"",""hide"":true},{""key"":""copyLink"",""hide"":true},{""key"":""pinItem"",""hide"":true},{""key"":""moveTo"",""hide"":true},{""key"":""copyTo"",""hide"":true},{""key"":""share"",""hide"":true},{""key"":""properties"",""hide"":true},{""key"":""open"",""hide"":true},{""key"":""checkOut"",""hide"":true}]}}]]></CustomFormatter>
                  <ColumnWidth>
                    <FieldRef Name=""Name"" width=""300"" />
                  </ColumnWidth>
                  <NewDocumentTemplates>[{""templateId"":""NewFolder"",""title"":""Folder"",""visible"":true},{""templateId"":""NewDOC"",""title"":""Word document"",""visible"":true},{""templateId"":""NewXSL"",""title"":""Excel workbook"",""visible"":true},{""templateId"":""NewPPT"",""title"":""PowerPoint presentation"",""visible"":true},{""templateId"":""NewONE"",""title"":""OneNote notebook"",""visible"":true},{""templateId"":""NewXSLSurvey"",""title"":""Excel survey"",""visible"":true},{""templateId"":""NewXSLForm"",""title"":""Forms for Excel"",""visible"":true},{""templateId"":""NewVSDX"",""title"":""Visio drawing"",""visible"":true},{""templateId"":""NewClipchamp"",""title"":""Clipchamp video"",""visible"":false}]</NewDocumentTemplates>
                  <ViewData />
                </View>
            </pnp:Views>
            <pnp:Fields>
                <Field ID=""{5cc6dc79-3710-4374-b433-61cb4a686c12}"" ReadOnly=""TRUE"" Type=""Computed"" Name=""LinkFilename"" DisplayName=""Name"" DisplayNameSrcField=""FileLeafRef"" Filterable=""FALSE"" ClassInfo=""Menu"" AuthoringInfo=""(linked to document with edit menu)"" ListItemMenuAllowed=""Required"" LinkToItemAllowed=""Prohibited"" NoCustomize=""TRUE"" SourceID=""http://schemas.microsoft.com/sharepoint/v3"" StaticName=""LinkFilename"" FromBaseType=""TRUE"" CustomFormatter=""{&quot;$schema&quot;:&quot;https://developer.microsoft.com/json-schemas/sp/v2/column-formatting.schema.json&quot;,&quot;elmType&quot;:&quot;div&quot;,&quot;txtContent&quot;:&quot;@currentField&quot;,&quot;customRowAction&quot;:{&quot;action&quot;:&quot;defaultClick&quot;},&quot;style&quot;:{&quot;cursor&quot;:&quot;pointer&quot;},&quot;attributes&quot;:{&quot;class&quot;:&quot;sp-field-fontSizeMedium&quot;}}""/>
            </pnp:Fields>
            </pnp:ListInstance>
        </pnp:Lists>
        <pnp:Header Layout=""Compact"" ShowSiteNavigation=""false"" />
        </pnp:ProvisioningTemplate>
    </pnp:Templates>
    </pnp:Provisioning>";

        public SPOApplySiteTemplate(IPnPContextFactory pnpContextFactory, ILoggerFactory loggerFactory, AzureFunctionSettings azureFunctionSettings)
        {
            _logger = loggerFactory.CreateLogger<SPOApplySiteTemplate>();
            _azureFunctionSettings = azureFunctionSettings;
            _contextFactory = pnpContextFactory;
        }

        [Function("SPOApplySiteTemplate")]
        public async Task<HttpResponseData> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req, [FromBody] ApplySiteTemplateData data)
        {
            try
            {
                var hideNewButton = "true"; // needs to be a string value as it'll be inserted in to xml string
                if (!data.HideNewButton)
                {
                    hideNewButton = "false";
                }

                using (var pnpContext = await _contextFactory.CreateAsync("Default"))
                {
                    // Get a CSOM context object for the new site and apply template
                    using (var csomAuth = PnP.Framework.AuthenticationManager.CreateWithPnPCoreSdk(pnpContext.AuthenticationProvider))
                    using (var csomContext = csomAuth.GetContext(data.SiteUrl))
                    using (var templateStream = new MemoryStream(Encoding.UTF8.GetBytes(_templateXml)))
                    {
                        XMLTemplateProvider provider = new XMLStreamTemplateProvider();
                        PnP.Framework.Provisioning.Model.ProvisioningTemplate template = provider.GetTemplate(templateStream);
                        template.Parameters.Clear();
                        template.Parameters.Add("ManagedMembersGroupName", Utility.SPO_MANAGED_MEMBERS_GROUP);
                        template.Parameters.Add("M365SiteAdminsEntraIDGroup", _azureFunctionSettings.M365SiteAdminsEntraIDGroup);
                        template.Parameters.Add("HideNewButton", hideNewButton);

                        if (data.PopulateDefaultMemberGroup)
                        {
                            template.Parameters.Add("M365IndependentProviderTeamEntraIDGroup", _azureFunctionSettings.M365IndependentProviderTeamEntraIDGroup);
                        }
                        csomContext.Web.ApplyProvisioningTemplate(template);

                        // disable access requests
                        csomContext.Web.DisableRequestAccess();
                    }

                    // Set Sharing Capability to ExistingExternalUserSharingOnly
                    var siteProperties = await pnpContext.GetSiteCollectionManager().GetSiteCollectionPropertiesAsync(new Uri(data.SiteUrl));
                    siteProperties.SharingCapability = SharingCapabilities.ExistingExternalUserSharingOnly;
                    await siteProperties.UpdateAsync();

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
