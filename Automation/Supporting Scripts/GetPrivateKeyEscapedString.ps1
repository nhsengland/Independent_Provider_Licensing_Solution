# File that contains the JWT Private Key
$jsonFile = "PrivateKey.json"

$json = Get-Content -Path (Join-Path -Path $PSScriptRoot -ChildPath $jsonFile)

# for pasting in to azure functions
$json | ConvertFrom-Json | ConvertTo-Json -Compress | clip

# for pasting in to local.settings.json, including leading and trailing quotes
#"`"" + (($json | ConvertFrom-Json | ConvertTo-Json -Compress) -Replace "`"", "\`"") + "`"" | clip