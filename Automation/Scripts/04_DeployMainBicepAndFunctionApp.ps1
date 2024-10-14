$ErrorActionPreference = "Stop"

$scriptPath = Split-Path -Parent $PSCommandPath
Import-Module (Join-Path -Path $scriptPath -ChildPath "common.psm1") -Force

$rootPath = Split-Path -Path $scriptPath -Parent

$deploymentConfig = Get-DeploymentConfig

Test-DeploymentPrerequisites
Connect-AzureSubscription

$resourceGroup = Get-ResourceGroup

# Build Function App
Set-Location $rootPath
dotnet build Licensing.Automation.Functions.sln --configuration Release
dotnet publish Licensing.Automation.Functions/Licensing.Automation.Functions.csproj --output publish_output_automation --configuration Release --framework net8.0 --runtime win-x64 --self-contained false
Compress-Archive -Path "publish_output_automation\*" -DestinationPath Licensing.Automation.Functions.zip -Force

# Deploy Main Bicep
New-AzResourceGroupDeployment -Name "Deployment_$((Get-Date).tostring('yyyy-MM-dd-HHmmss'))" `
	-ResourceGroupName $resourceGroup.ResourceGroupName `
	-TemplateFile "$rootPath\.iac\main.bicep" `
	-TemplateParameterFile "$rootPath\.iac\parameters\main.parameters.$($deploymentConfig.bicepParams).json"

# Publish Function App code
$functionApp = Get-FunctionApp
Publish-AzWebApp -ResourceGroupName $resourceGroup.ResourceGroupName -Name $functionApp.Name -ArchivePath Licensing.Automation.Functions.zip -Force
Update-AzFunctionAppSetting -ResourceGroupName $resourceGroup.ResourceGroupName -Name $functionApp.Name -AppSetting @{"WEBSITE_RUN_FROM_PACKAGE" = "1"}

# Write Function App Uri to App Registration
$app = Get-AuthenticationAppRegistration
$webApplication = [Microsoft.Azure.PowerShell.Cmdlets.Resources.MSGraph.Models.ApiV10.IMicrosoftGraphWebApplication]@{
    HomePageUrl = "https://$($functionApp.HostName)"
    RedirectUri = "https://$($functionApp.HostName)/.auth/login/aad/callback"
    ImplicitGrantSetting = @{
        EnableIdTokenIssuance = $true
    }
}
$app | Update-AzADApplication -Web $webApplication
