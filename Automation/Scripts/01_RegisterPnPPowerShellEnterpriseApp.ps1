$scriptPath = Split-Path -Parent $PSCommandPath
Import-Module (Join-Path -Path $scriptPath -ChildPath "common.psm1") -Force

Test-DeploymentPrerequisites

# See https://pnp.github.io/powershell/articles/authentication.html
Register-PnPManagementShellAccess
