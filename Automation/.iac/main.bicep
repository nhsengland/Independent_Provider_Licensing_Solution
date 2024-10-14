@description('Location for all resources.')
param location string = resourceGroup().location

@description('Unique Suffix')
param uniqueSuffix string

@description('Storage Account SKU')
param storageAccountSku string

@description('Authentication App Registration Client ID')
param authenticationAppClientId string

@description('Microsoft 365 SharePoint Root Site URL')
param m365RootSiteUrl string

@description('Microsoft 365 Invite Redirect URL')
param m365InviteRedirectUrl string

@description('Microsoft 365 Site Owner Email/UPN')
param m365SiteOwner string

@description('Microsoft 365 Entra ID Admin Group')
param m365SiteAdminsEntraIDGroup string

@description('Microsoft 365 Entra ID IP Licensing Team Group')
param m365IndependentProviderTeamEntraIDGroup string

@description('Microsoft 365 SharePoint Licence Site Prefix')
param m365SitePrefix string

@description('Okta Domain')
param oktaDomain string

@description('Okta Client Id')
param oktaClientId string

@description('Okta Licensing Group Id')
param oktaLicensingGroupId string

@description('Okta Application Group Id')
param oktaApplicationGroupId string

@description('Service Account Principal ID')
param serviceAccountPrincipalID string

@description('SQL Server Name')
param sqlServerName string

@description('SQL Application Database Name')
param sqlApplicationDatabaseName string

@description('SQL Licensing Database Name')
param sqlLicensingDatabaseName string

@description('GOV.UK Notify Pre-application Approval With Activation Template ID')
param notifyPreApplicationApprovalWithActivationTemplateID string

@description('GOV.UK Notify No Pre-application Approval With Activation Template ID')
param notifyNoPreApplicationApprovalWithActivationTemplateID string

@description('GOV.UK Notify Pre-application Approval Without Activation Template ID')
param notifyPreApplicationApprovalWithoutActivationTemplateID string

@description('GOV.UK Notify No Pre-application Approval Without Activation Template ID')
param notifyNoPreApplicationApprovalWithoutActivationTemplateID string

@description('GOV.UK Notify Licensing User With Activation Template ID')
param notifyLicensingUserWithActivationTemplateID string

@description('GOV.UK Notify Licensing User Without Activation Template ID')
param notifyLicensingUserWithoutActivationTemplateID string

@description('License Holder Web App URL')
param licenseHolderUrl string

@description('In-Portal Notification From')
param inPortalNotificationFrom string

@description('In-portal Notification Annual Certificate Task Title')
param inPortalNotificationAnnualCertificateTaskTitle string

@description('In-portal Notification Annual Certificate Task Body')
param inPortalNotificationAnnualCertificateTaskBody string

@description('Part 2 Application Form Link')
param part2ApplicationFormLink string

@description('Contact Email')
param contactEmail string

@description('GOV.UK Notify Annual Certificate Task Template ID')
param notifyAnnualCertificateTaskTemplateID string

@description('GOV.UK Notify Financial Monitoring Task Template ID')
param notifyFinancialMonitoringTaskTemplateID string

@description('GOV.UK Notify In-Portal Notification Template ID')
param notifyInPortalNotificationTemplateID string

@description('In-portal Notification Financial Monitoring Task Title')
param inPortalNotificationFinancialMonitoringTaskTitle string

@description('In-portal Notification Financial Monitoring Task Body')
param inPortalNotificationFinancialMonitoringTaskBody string

@description('SPO Annual Certificate Document Template Path')
param spoAnnualCertificateDocumentTemplatePath string

@description('SPO Template Site Url')
param spoTemplateSiteUrl string

var namePrefix = 'auto'
var functionAppName = '${namePrefix}-func-${uniqueSuffix}'
var hostingPlanName = '${namePrefix}-plan-${uniqueSuffix}'
var applicationInsightsName = '${namePrefix}-appinsights-${uniqueSuffix}'
var keyVaultName = '${namePrefix}-vault-${uniqueSuffix}'
var publicStorageAccountName = '${namePrefix}pub${replace(uniqueSuffix,'-','')}'
var storageAccountName = '${namePrefix}${replace(uniqueSuffix,'-','')}'
var KeyVaultSecretsUserRoleGuid = subscriptionResourceId('Microsoft.Authorization/roleDefinitions', '4633458b-17de-408a-b874-0445c86b69e6')
var logicAppPrefix = '${namePrefix}-logicapp-${uniqueSuffix}-'

resource vault 'Microsoft.KeyVault/vaults@2021-11-01-preview' existing = {
  name: keyVaultName
}

resource storageAccount 'Microsoft.Storage/storageAccounts@2022-05-01' = {
  name: storageAccountName
  location: location
  sku: {
    name: storageAccountSku
  }
  kind: 'StorageV2'
  properties: {
    supportsHttpsTrafficOnly: true
    defaultToOAuthAuthentication: true
  }
}

resource hostingPlan 'Microsoft.Web/serverfarms@2021-03-01' = {
  name: hostingPlanName
  location: location
  sku: {
    name: 'Y1'
    tier: 'Dynamic'
  }
  properties: {}
}

resource functionApp 'Microsoft.Web/sites@2021-03-01' = {
  name: functionAppName
  location: location
  kind: 'functionapp'
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    serverFarmId: hostingPlan.id
    siteConfig: {
      appSettings: [
        {
          name: 'AzureWebJobsStorage'
          value: 'DefaultEndpointsProtocol=https;AccountName=${storageAccountName};EndpointSuffix=${environment().suffixes.storage};AccountKey=${storageAccount.listKeys().keys[0].value}'
        }
        {
          name: 'WEBSITE_CONTENTAZUREFILECONNECTIONSTRING'
          value: 'DefaultEndpointsProtocol=https;AccountName=${storageAccountName};EndpointSuffix=${environment().suffixes.storage};AccountKey=${storageAccount.listKeys().keys[0].value}'
        }
        {
          name: 'WEBSITE_CONTENTSHARE'
          value: toLower(functionAppName)
        }
        {
          name: 'FUNCTIONS_EXTENSION_VERSION'
          value: '~4'
        }
        {
          name: 'WEBSITE_NODE_DEFAULT_VERSION'
          value: '~14'
        }
        {
          name: 'APPINSIGHTS_INSTRUMENTATIONKEY'
          value: applicationInsights.properties.InstrumentationKey
        }
        {
          name: 'FUNCTIONS_WORKER_RUNTIME'
          value: 'dotnet-isolated'
        }
        {
          name: 'MICROSOFT_PROVIDER_AUTHENTICATION_SECRET'
          value: '@Microsoft.KeyVault(VaultName=${vault.name};SecretName=AuthenticationAppSecret)'
        }
        {
          name: 'M365RootSiteUrl'
          value: m365RootSiteUrl
        }
        {
          name: 'M365InviteRedirectUrl'
          value: m365InviteRedirectUrl
        }
        {
          name: 'M365SiteOwner'
          value: m365SiteOwner
        }
        {
          name: 'M365SiteAdminsEntraIDGroup'
          value: m365SiteAdminsEntraIDGroup
        }
        {
          name: 'M365IndependentProviderTeamEntraIDGroup'
          value: m365IndependentProviderTeamEntraIDGroup
        }
        {
          name: 'M365SitePrefix'
          value: m365SitePrefix
        }
        {
          name: 'OktaDomain'
          value: oktaDomain
        }
        {
          name: 'OktaClientId'
          value: oktaClientId
        }
        {
          name: 'OktaLicensingGroupId'
          value: oktaLicensingGroupId
        }
        {
          name: 'OktaApplicationGroupId'
          value: oktaApplicationGroupId
        }
        {
          name: 'OktaPrivateKey'
          value: '@Microsoft.KeyVault(VaultName=${vault.name};SecretName=OktaPrivateKey)'
        }
        {
          name: 'GovUKNotifyAPIKey'
          value: '@Microsoft.KeyVault(VaultName=${vault.name};SecretName=GovUKNotifyAPIKey)'
        }
      ]
      ftpsState: 'Disabled'
      minTlsVersion: '1.2'
      netFrameworkVersion: 'v8.0'
      use32BitWorkerProcess: false
    }
    httpsOnly: true
  }
}

resource ftpPublishingCredentialsPolicy 'Microsoft.Web/sites/basicPublishingCredentialsPolicies@2022-09-01' = {
  name: 'ftp'
  kind: 'string'
  parent: functionApp
  properties: {
    allow: false
  }
}

resource functionAppAuthSettings 'Microsoft.Web/sites/config@2022-03-01' = {
  parent: functionApp
  name:'authsettingsV2'
  properties: {
    globalValidation: {
      requireAuthentication: true
      unauthenticatedClientAction: 'Return401'
    }
    identityProviders: {
      azureActiveDirectory: {
        enabled: true
        registration: {
          clientId: authenticationAppClientId
          clientSecretSettingName: 'MICROSOFT_PROVIDER_AUTHENTICATION_SECRET'
          openIdIssuer: 'https://sts.windows.net/${tenant().tenantId}/v2.0'
        }
        validation: {
          allowedAudiences: [
            'api://${authenticationAppClientId}'
          ]
        }
      }
    }
  }
}

resource roleAssignment 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  scope: vault
  name: guid(vault.id, functionApp.id, KeyVaultSecretsUserRoleGuid)
  properties: {
    principalId: functionApp.identity.principalId
    roleDefinitionId: KeyVaultSecretsUserRoleGuid
  }
}

resource applicationInsights 'Microsoft.Insights/components@2020-02-02' = {
  name: applicationInsightsName
  location: location
  kind: 'web'
  properties: {
    Application_Type: 'web'
    Request_Source: 'rest'
  }
}

resource queueServices 'Microsoft.Storage/storageAccounts/queueServices@2023-01-01' = {
  name: 'default'
  parent: storageAccount
}

module connections 'modules/connections.bicep' = {
  name: 'connectionsDeploy'
  params: {
    location: location
    namePrefix: namePrefix
  }
}

module logicapp01 'modules/logicapp01.bicep' = {
  name: 'logicapp01Deploy'
  params: {
    name: '${logicAppPrefix}01'
    location: location
    queuesConnectionId: connections.outputs.queuesId
    sqlConnectionId: connections.outputs.sqlId
    sharePointConnectionId: connections.outputs.sharePointId
    storageAccountName: storageAccountName
    functionAppHostName: functionApp.properties.defaultHostName
    functionAppAudience: functionAppAuthSettings.properties.identityProviders.azureActiveDirectory.validation.allowedAudiences[0]
    sqlServerName: sqlServerName
    sqlLicensingDatabaseName: sqlLicensingDatabaseName
    logicApp05Url: logicapp05.outputs.logicApp05Url
    spoAnnualCertificateDocumentTemplatePath: spoAnnualCertificateDocumentTemplatePath
    spoTemplateSiteUrl: spoTemplateSiteUrl
  }
  dependsOn: [
    queueServices
  ]
}

module logicapp02 'modules/logicapp02.bicep' = {
  name: 'logicapp02Deploy'
  params: {
    name: '${logicAppPrefix}02'
    location: location
    storageAccountName: storageAccountName
    functionAppHostName: functionApp.properties.defaultHostName
    functionAppAudience: functionAppAuthSettings.properties.identityProviders.azureActiveDirectory.validation.allowedAudiences[0]
    queuesConnectionId: connections.outputs.queuesId
    sqlConnectionId: connections.outputs.sqlId
    sqlServerName: sqlServerName
    sqlApplicationDatabaseName: sqlApplicationDatabaseName
    sqlLicensingDatabaseName: sqlLicensingDatabaseName
    notifyPreApplicationApprovalWithActivationTemplateID: notifyPreApplicationApprovalWithActivationTemplateID
    notifyNoPreApplicationApprovalWithActivationTemplateID: notifyNoPreApplicationApprovalWithActivationTemplateID
    notifyPreApplicationApprovalWithoutActivationTemplateID: notifyPreApplicationApprovalWithoutActivationTemplateID
    notifyNoPreApplicationApprovalWithoutActivationTemplateID: notifyNoPreApplicationApprovalWithoutActivationTemplateID
    notifyLicensingUserWithActivationTemplateID: notifyLicensingUserWithActivationTemplateID
    notifyLicensingUserWithoutActivationTemplateID: notifyLicensingUserWithoutActivationTemplateID
    licenseHolderUrl: licenseHolderUrl
    part2ApplicationFormLink: part2ApplicationFormLink
    contactEmail: contactEmail
  }
  dependsOn: [
    queueServices
  ]
}

module logicapp03 'modules/logicapp03.bicep' = {
  name: 'logicapp03Deploy'
  params: {
    name: '${logicAppPrefix}03'
    location: location
    storageAccountName: storageAccountName
    sqlConnectionId: connections.outputs.sqlId
    sqlServerName: sqlServerName
    sqlApplicationDatabaseName: sqlApplicationDatabaseName
    sqlLicensingDatabaseName: sqlLicensingDatabaseName
    queuesConnectionId: connections.outputs.queuesId
  }
  /*
  dependsOn: [
    queueServices
  ]
  */
}

module logicapp04 'modules/logicapp04.bicep' = {
  name: 'logicapp04Deploy'
  params: {
    name: '${logicAppPrefix}04'
    location: location
    storageAccountName: storageAccountName
    functionAppHostName: functionApp.properties.defaultHostName
    functionAppAudience: functionAppAuthSettings.properties.identityProviders.azureActiveDirectory.validation.allowedAudiences[0]
    queuesConnectionId: connections.outputs.queuesId
    sqlConnectionId: connections.outputs.sqlId
    sqlServerName: sqlServerName
    sqlApplicationDatabaseName: sqlApplicationDatabaseName
    sqlLicensingDatabaseName: sqlLicensingDatabaseName
  }
  dependsOn: [
    queueServices
  ]
}

module logicapp05 'modules/logicapp05.bicep' = {
  name: 'logicapp05Deploy'
  params: {
    name: '${logicAppPrefix}05'
    location: location
    storageAccountName: storageAccountName
    functionAppHostName: functionApp.properties.defaultHostName
    functionAppAudience: functionAppAuthSettings.properties.identityProviders.azureActiveDirectory.validation.allowedAudiences[0]
    queuesConnectionId: connections.outputs.queuesId
    sqlConnectionId: connections.outputs.sqlId
    sqlServerName: sqlServerName
    sqlLicensingDatabaseName: sqlLicensingDatabaseName
  }
  dependsOn: [
    queueServices
  ]
}

module logicapp07 'modules/logicapp07.bicep' = {
  name: 'logicapp07Deploy'
  params: {
    name: '${logicAppPrefix}07'
    location: location
    storageAccountName: storageAccountName
    functionAppHostName: functionApp.properties.defaultHostName
    functionAppAudience: functionAppAuthSettings.properties.identityProviders.azureActiveDirectory.validation.allowedAudiences[0]
    queuesConnectionId: connections.outputs.queuesId
    sqlConnectionId: connections.outputs.sqlId
    sqlServerName: sqlServerName
    sqlLicensingDatabaseName: sqlLicensingDatabaseName
    logicApp05PrincipalId: logicapp05.outputs.logicApp05PrincipalId
  }
  dependsOn: [
    queueServices
  ]
}

module logicapp08 'modules/logicapp08.bicep' = {
  name: 'logicapp08Deploy'
  params: {
    name: '${logicAppPrefix}08'
    location: location
    storageAccountName: storageAccountName
    functionAppHostName: functionApp.properties.defaultHostName
    functionAppAudience: functionAppAuthSettings.properties.identityProviders.azureActiveDirectory.validation.allowedAudiences[0]
    queuesConnectionId: connections.outputs.queuesId
    sqlConnectionId: connections.outputs.sqlId
    sqlServerName: sqlServerName
    sqlLicensingDatabaseName: sqlLicensingDatabaseName
    licenseHolderUrl: licenseHolderUrl
    notifyAnnualCertificateTaskTemplateID: notifyAnnualCertificateTaskTemplateID
    notifyFinancialMonitoringTaskTemplateID: notifyFinancialMonitoringTaskTemplateID
    notifyInPortalNotificationTemplateID: notifyInPortalNotificationTemplateID
    inPortalNotificationFrom: inPortalNotificationFrom
    inPortalNotificationAnnualCertificateTaskTitle: inPortalNotificationAnnualCertificateTaskTitle
    inPortalNotificationAnnualCertificateTaskBody: inPortalNotificationAnnualCertificateTaskBody
    inPortalNotificationFinancialMonitoringTaskTitle: inPortalNotificationFinancialMonitoringTaskTitle
    inPortalNotificationFinancialMonitoringTaskBody: inPortalNotificationFinancialMonitoringTaskBody
    logicApp01PrincipalId: logicapp01.outputs.logicApp01PrincipalId
  }
  dependsOn: [
    queueServices
  ]
}

module logicapp09 'modules/logicapp09.bicep' = {
  name: 'logicapp09Deploy'
  params: {
    name: '${logicAppPrefix}09'
    location: location
    queuesConnectionId: connections.outputs.queuesId
    storageAccountName: storageAccountName
    functionAppHostName: functionApp.properties.defaultHostName
    functionAppAudience: functionAppAuthSettings.properties.identityProviders.azureActiveDirectory.validation.allowedAudiences[0]
    logicApp08PrincipalId: logicapp08.outputs.logicApp08PrincipalId
    logicApp02PrincipalId: logicapp02.outputs.logicApp02PrincipalId
    sharePointConnectionId: connections.outputs.sharePointId
    oktaLicensingGroupId: oktaLicensingGroupId
    sqlServerName: sqlServerName
    sqlLicensingDatabaseName: sqlLicensingDatabaseName
    sqlConnectionId: connections.outputs.sqlId
  }
  dependsOn: [
    queueServices
  ]
}

module logicapp10 'modules/logicapp10.bicep' = {
  name: 'logicapp10Deploy'
  params: {
    name: '${logicAppPrefix}10'
    location: location
    queuesConnectionId: connections.outputs.queuesId
    storageAccountName: storageAccountName
    functionAppHostName: functionApp.properties.defaultHostName
    functionAppAudience: functionAppAuthSettings.properties.identityProviders.azureActiveDirectory.validation.allowedAudiences[0]
    logicApp03PrincipalId: logicapp03.outputs.logicApp03PrincipalId
    sqlServerName: sqlServerName
    sqlLicensingDatabaseName: sqlLicensingDatabaseName
    sqlConnectionId: connections.outputs.sqlId
    sqlApplicationDatabaseName: sqlApplicationDatabaseName
  }
  dependsOn: [
    queueServices
  ]
}

module logicapp11 'modules/logicapp11.bicep' = {
  name: 'logicapp11Deploy'
  params: {
    name: '${logicAppPrefix}11'
    location: location
    storageAccountName: storageAccountName
    functionAppHostName: functionApp.properties.defaultHostName
    functionAppAudience: functionAppAuthSettings.properties.identityProviders.azureActiveDirectory.validation.allowedAudiences[0]
    queuesConnectionId: connections.outputs.queuesId
    sqlConnectionId: connections.outputs.sqlId
    sqlServerName: sqlServerName
    sqlLicensingDatabaseName: sqlLicensingDatabaseName
  }
  dependsOn: [
    queueServices
  ]
}

module dataverseInterop 'modules/dataverseInterop.bicep' = {
  name: 'dataverseInteropDeploy'
  params: {
    storageAccountName: storageAccountName
    serviceAccountPrincipalID: serviceAccountPrincipalID
    logicApp01PrincipalId: logicapp01.outputs.logicApp01PrincipalId
    logicApp02PrincipalId: logicapp02.outputs.logicApp02PrincipalId
    logicApp04PrincipalId: logicapp04.outputs.logicApp04PrincipalId
    logicApp05PrincipalId: logicapp05.outputs.logicApp05PrincipalId
    logicApp07PrincipalId: logicapp07.outputs.logicApp07PrincipalId
    logicApp08PrincipalId: logicapp08.outputs.logicApp08PrincipalId
    logicApp09PrincipalId: logicapp09.outputs.logicApp09PrincipalId
  }
}

module publicStorageAccount 'modules/publicStorageAccount.bicep' = {
  name: 'publicStorageAccountDeploy'
  params: {
    name: publicStorageAccountName
    location: location
    serviceAccountPrincipalID: serviceAccountPrincipalID
    storageAccountType: storageAccountSku
  }
}
