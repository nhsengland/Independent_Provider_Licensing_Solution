$errorActionPreference = "Stop"

$scriptPath = Split-Path -Parent $PSCommandPath

Import-Module (Join-Path -Path $scriptPath -ChildPath "common.psm1") -Force

Test-DeploymentPrerequisites
Connect-AzureSubscription

# check for existing app registration
$app = Get-AuthenticationAppRegistration -ErrorAction SilentlyContinue
if ($null -ne $app) {
    throw "App registration with a name of '$($config.authenticationAppRegistration.name)' already exists"
}

$config = Get-DeploymentConfig

# Build App role hashtable
$appRole = [Microsoft.Azure.PowerShell.Cmdlets.Resources.MSGraph.Models.ApiV10.IMicrosoftGraphAppRole]@{
    Id = [Guid]::NewGuid().ToString()
    AllowedMemberType = "Application"
    DisplayName = $config.authenticationAppRegistration.customRoleName
    Description = $config.authenticationAppRegistration.customRoleDescription
    Value = $config.authenticationAppRegistration.customRoleName
    IsEnabled = $true
}

# Build required resource access hashtable (specifies User.Read delegated Microsoft Graph permission)
$requiredResourceAccess = [Microsoft.Azure.PowerShell.Cmdlets.Resources.MSGraph.Models.ApiV10.IMicrosoftGraphRequiredResourceAccess]@{
    ResourceAccess = @(@{
        Id = "{id}"
        Type = "Scope"
    })
    ResourceAppId = "00000003-0000-0000-c000-000000000000"
}

# Create application
Write-Host "Creating '$($config.authenticationAppRegistration.name)' app registration"
$app = New-AzADApplication -DisplayName $config.authenticationAppRegistration.name `
    -AppRole $appRole `
    -RequiredResourceAccess $requiredResourceAccess `
    -Description $config.authenticationAppRegistration.description `
    -SignInAudience AzureADMyOrg

# Add application id uri (api://<app id>)
$app | Update-AzADApplication -IdentifierUris @("api://$($app.AppId)")

# Create app registration secret
$secret = $app | New-AzADAppCredential -EndDate (Get-Date).AddYears(10)

# Create service principal
New-AzADServicePrincipal -ApplicationId $app.AppId -AppRoleAssignmentRequired -Tag "HideApp" | Out-Null

# Add app registration info to bicep file
$bicepConfig = Get-MainBicepConfig
$bicepConfig.parameters.authenticationAppClientId.value = $app.AppId
$bicepConfig | Set-MainBicepConfig

Write-Host "The following line contains the authentication app secret.  You will need this when deploying the keyvault bicep script"
Write-Host $secret.SecretText
