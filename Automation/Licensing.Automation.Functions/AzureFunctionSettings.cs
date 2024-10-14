using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licensing.Automation.Functions
{
    public class AzureFunctionSettings
    {
        public string? M365LocalDevTenantId { get; set; }
        public string? M365LocalDevClientId { get; set; }
        public string? M365LocalDevCertificateThumbprint { get; set; }
        public string? M365RootSiteUrl { get; set; }
        public string? OktaDomain {  get; set; }
        public string? OktaClientId { get; set; }
        public string? OktaPrivateKey {  get; set; }
        public string? OktaLicensingGroupId { get; set; }
        public string? OktaApplicationGroupId {  get; set; }
        public string? M365InviteRedirectUrl {  get; set; }
        public string? M365SiteOwner {  get; set; }
        public string? M365SiteAdminsEntraIDGroup { get; set; }
        public string? M365IndependentProviderTeamEntraIDGroup { get; set; }
        public string? M365SitePrefix {  get; set; }
        public string? GovUKNotifyAPIKey {  get; set; }
    }
}
