
$ErrorActionPreference = "Continue"

$scriptPath = Split-Path -Parent $PSCommandPath
Import-Module (Join-Path -Path $scriptPath -ChildPath "common.psm1")

Test-DeploymentPrerequisites

# Open config file
$config = Get-DeploymentConfig

Connect-PnPOnline -Url $config.sharePointUrl -Interactive

# get service principal for our authentication app
$authenticationAppServicePrincipal = Get-PnPAzureADServicePrincipal -AppName $config.authenticationAppRegistration.name

# get all managed identity service principals in our resource group
$managedIdentityServicePrincipals = Get-PnPAzureADServicePrincipal -Filter "alternativeNames/any(a:startsWith(a,'/subscriptions/$($config.subscriptionId)/resourcegroups/$($config.resourceGroup.name)/')) and servicePrincipalType eq 'ManagedIdentity'"

# Grant permissions Function App Managed Identity Service Principal
# NOTE: At present, the the Managed Identity Service Principal requires the Sites.FullControl.All role in order to create new site collections.
# This is more permission than desirable, but is the only option as of 14/03/2024.  Microsoft are planning to implement a new 'Sites.Create.All' role, at
# which point it should be possible to remove the 'Sites.FullControl.All' role and replace it with 'Sites.Create.All' and 'Sites.Selected' roles.  Code changes
# to the Function App are also likely to be necessary.  The corresponding roadmap entry can be found here:
# https://www.microsoft.com/en-gb/microsoft-365/roadmap?filters=&searchterms=124843
$functionAppManagedIdentityPrincipal = $managedIdentityServicePrincipals | Where-Object { $_.AlternativeNames -like '*/Microsoft.Web/sites/auto-*' }
Write-Host "Granting permissions to '$($functionAppManagedIdentityPrincipal.DisplayName)' Function App" -f Yellow
Add-PnPAzureADServicePrincipalAppRole -Principal $functionAppManagedIdentityPrincipal.Id -AppRole "User.Invite.All" -BuiltInType MicrosoftGraph | Out-Null
Add-PnPAzureADServicePrincipalAppRole -Principal $functionAppManagedIdentityPrincipal.Id -AppRole "Sites.FullControl.All" -BuiltInType SharePointOnline | Out-Null
Add-PnPAzureADServicePrincipalAppRole -Principal $functionAppManagedIdentityPrincipal.Id -AppRole "User.Read.All" -BuiltInType MicrosoftGraph | Out-Null

# Grant all Logic App Managed Identities access to the automation Function App
$managedIdentityServicePrincipals | Where-Object { $_.AlternativeNames -like '*/Microsoft.Logic/workflows/*' } | ForEach-Object {
    Write-Host "Graning permission to '$($_.DisplayName)' Logic App" -f Yellow
    Add-PnPAzureADServicePrincipalAppRole -Principal $_.Id -AppRole "Function.InvokeAll" -Resource $authenticationAppServicePrincipal.Id | Out-Null
}

# Grant all Web App Managed Identities access to the automation Function App
$managedIdentityServicePrincipals | Where-Object { $_.AlternativeNames -like '*/Microsoft.Web/sites/*' -and $_.DisplayName -notlike 'auto-*' } | ForEach-Object {
    Write-Host "Graning permission to '$($_.DisplayName)' Web or Function App" -f Yellow
    Add-PnPAzureADServicePrincipalAppRole -Principal $_.Id -AppRole "Function.InvokeAll" -Resource $authenticationAppServicePrincipal.Id | Out-Null
}

