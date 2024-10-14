$ErrorActionPreference = "Stop"

$scriptPath = Split-Path -Parent $PSCommandPath
Import-Module (Join-Path -Path $scriptPath -ChildPath "common.psm1")

$rootPath = Split-Path -Path $scriptPath -Parent

$deploymentConfig = Get-DeploymentConfig

Test-DeploymentPrerequisites
Connect-AzureSubscription

$resourceGroup = Get-ResourceGroup -CreateIfNotExists

New-AzResourceGroupDeployment -Name "AutoKeyVaultDeployment_$((Get-Date).tostring('yyyy-MM-dd-HHmmss'))" `
	-ResourceGroupName $resourceGroup.ResourceGroupName `
	-TemplateFile "$rootPath\.iac\keyvault.bicep" `
	-TemplateParameterFile "D:\source\Licensing.Automation.Functions\.iac\parameters\keyvault.parameters.$($deploymentConfig.bicepParams).json"

