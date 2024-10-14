$scriptPath = Split-Path -Parent $PSCommandPath
$bicepParamsPath = Join-Path -Path (Split-Path -Path $scriptPath -Parent) -ChildPath ".iac/parameters"

function Get-DeploymentConfig {
    Get-Content -Path (Join-Path -Path $scriptPath -ChildPath "config.json") -Raw | ConvertFrom-Json
}

function Get-MainBicepConfig {
    $config = Get-DeploymentConfig
    Get-Content -Path (Join-Path -Path $bicepParamsPath -ChildPath "main.parameters.$($config.bicepParams).json") -Raw | ConvertFrom-Json
}

function Set-MainBicepConfig {
    [cmdletbinding()]
    Param(
        [Parameter(ValueFromPipeline, Mandatory = $true)]
        [object]$BicepConfig
    )
    $config = Get-DeploymentConfig
    $bicepConfig | ConvertTo-Json -depth 5 | Set-Content (Join-Path -Path $bicepParamsPath -ChildPath "main.parameters.$($config.bicepParams).json")
}

function Connect-AzureSubscription {
    
    try {
        $config = Get-DeploymentConfig
        Write-Host "Connecting to subscription $($config.subscriptionId) in tenant $($config.tenantId)"
        Connect-AzAccount -Tenant $config.tenantId -WarningAction Ignore | Out-Null
        Set-AzContext -Subscription $config.subscriptionId | Out-Null
    }
    catch {
        throw
    }
}

function Test-DeploymentPrerequisites {

    Write-Host "Checking deployment prerequisites"

    # Check for PnP.PowerShell module
    $pnpModule = Get-InstalledModule -Name "PnP.PowerShell" -ErrorAction SilentlyContinue
    if ($null -eq $pnpModule -or $pnpModule.Version -lt "2.3.0") {
        throw "PowerShell Module 'PnP.PowerShell' v2.3.0 or later must be installed (Use Install-Module PnP.PowerShell -AllowPrerelease)"
    }

    # Check for Az module
    $azModule = Get-InstalledModule -Name "Az" -ErrorAction SilentlyContinue
    if ($null -eq $azModule -or $azModule.Version -lt "11.4.0") {
        throw "PowerShell Module 'Az' v11.4.0 or later must be installed (Use: Install-Module Az)"
    }

    # Check for Bicep
    if ($env:PATH -notlike "*\Bicep CLI*") {
        throw "Bicep path variable not found. (Use: winget install -e --id Microsoft.Bicep)"
    }
}

function Get-ResourceGroup {
    Param(
        [switch]$CreateIfNotExists = $false
    )

    try {
        $config = Get-DeploymentConfig
        $resourceGroup = Get-AzResourceGroup -Name $config.resourceGroup.name -ErrorAction SilentlyContinue
        if ($null -eq $resourceGroup) {
            if (!$CreateIfNotExists) {
                throw "Resource Group $($config.resourceGroup.name) does not exist"
            }
            else {
                Write-Host "Creating Resource Group $($config.resourceGroup.name) in the $($config.resourceGroup.region) region"
                New-AzResourceGroup -Name $config.resourceGroup.name -Location $config.resourceGroup.region
            }
        }
        else {
            $resourceGroup
        }
    }
    catch {
        throw
    }
}

function Get-AuthenticationAppRegistration {
    
    Write-Host "Fetching Authentication App registration"

    $config = Get-DeploymentConfig
    $app = Get-AzADApplication -Filter "DisplayName eq '$($config.authenticationAppRegistration.name)'"
    if ($null -eq $app) {
        Write-Host "Authentication App registration with a name of '$($config.authenticationAppRegistration.name)' not found"
    }
    else {
        $app
    }
}

function Get-FunctionApp {
    Param(
        [switch]$AsWebApp = $false
    )
    
    Write-Host "Fetching Function App"

    $config = Get-DeploymentConfig

    if ($AsWebApp) {
        $functionApp = Get-AzWebApp -ResourceGroupName $config.resourceGroup.name | Where-Object { $_.Name -like "auto-*" }
    }
    else {
        $functionApp = Get-AzFunctionApp -ResourceGroupName $config.resourceGroup.name | Where-Object { $_.Name -like "auto-*" }
    }
    
    if ($null -eq $functionApp) {
        throw "Unable to locate Function App"
    }
    else {
        $functionApp
    }
}
