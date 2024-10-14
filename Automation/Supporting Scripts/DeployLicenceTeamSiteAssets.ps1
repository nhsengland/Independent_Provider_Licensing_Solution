$siteUrl = "*"

$annualCertificatesTemplatesListTitle = "Annual Certificates Templates"
$annualCertificatesTemplatesListUrl = "AnnualCertificatesTemplates"

Connect-PnPOnline -Url $siteUrl -Interactive

# Create 'Annual Certificates Templates' Document Library
$annualCertificatesTemplatesList = Get-PnPList -Identity $annualCertificatesTemplatesListTitle -ErrorAction SilentlyContinue
if ($null -eq $annualCertificatesTemplatesList) {
    $annualCertificatesTemplatesList = New-PnPList -Title $annualCertificatesTemplatesListTitle -Url $annualCertificatesTemplatesListUrl -Template DocumentLibrary
}

# Create top level folders in 'Annual Certificates Templates' Document Library
Resolve-PnPFolder -SiteRelativePath "$annualCertificatesTemplatesListUrl/Default" | Out-Null
Resolve-PnPFolder -SiteRelativePath "$annualCertificatesTemplatesListUrl/CRS" | Out-Null
