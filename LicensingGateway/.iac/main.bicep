param storageAccountName string
param storageAccountSkuName string
param applicationInsightsName string
param webAppName string
param webAppSkuName string
param webAppSkuTier string
param webAppSkuSize string
param webAppSkuFamily string
param webAppSkuCapacity int
param webAppServicePlanName string
param keyVaultName string
param logAnalyticsWorkspaceName string
param location string = resourceGroup().location
param sqlInstanceName string
param sqlAdGroupName string
param sqlAdGroupId string
param sqlDatabaseName string
param functionAppName string
param functionServicePlanName string
param automationFunctionBaseURL string
param licenceHolderApplicationURL string
param sqlDatabaseRequestedBackupStorageRedundancy string
param sqlDatabaseNameForLicenseHolder string


resource logAnalytics 'Microsoft.OperationalInsights/workspaces@2021-12-01-preview' = {
  name: logAnalyticsWorkspaceName
  location: location
}

module application_insights 'modules/application.insights.bicep' = {
  name: applicationInsightsName
  params: {
    name: applicationInsightsName
    location: location
  }
}

module storage_account 'modules/storage.account.bicep' = {
  name: storageAccountName
  params: {
    name: storageAccountName
    sku: storageAccountSkuName
    location: location
    keyVaultName: keyVaultName
  }
}

module sql 'modules/sql.bicep' = {
  name: 'sql'
  params: {
    location: location
    sqlInstanceName: sqlInstanceName
    sqlAdGroupName: sqlAdGroupName
    sqlAdGroupId: sqlAdGroupId
    sqlDatabaseName: sqlDatabaseName
    sqlDatabaseRequestedBackupStorageRedundancy: sqlDatabaseRequestedBackupStorageRedundancy
  }
}

module webApp_licensing_gateway 'modules/web.app.bicep' = {
  name: webAppName
  params: {
    applicationInsightsConnectionString: application_insights.outputs.connectionString
    appServicePlanName: webAppServicePlanName
    location: location
    logAnalyticsId: logAnalytics.id
    skuName: webAppSkuName
    skuCapacity: webAppSkuCapacity
    skuFamily: webAppSkuFamily
    skuSize: webAppSkuSize
    skuTier: webAppSkuTier
    sqlDatabaseName: sqlDatabaseName
    sqlServerHostname: sql.outputs.sqlServerHostname
    sqlDatabaseNameForLicenseHolder: sqlDatabaseNameForLicenseHolder
    storageAccountName: storageAccountName
    storageAccountQueueEndPoint: storage_account.outputs.storageAccountQueueEndPoint
    storageAccountBlobEndPoint: storage_account.outputs.storageAccountBlobEndPoint
    storageAccountTableEndPoint: storage_account.outputs.storageAccountTableEndPoint
    webAppName: webAppName
    keyVaultName: keyVaultName
    licenceHolderApplicationURL: licenceHolderApplicationURL
  }
}

module function_app_main 'modules/function.app.bicep' = {
  name: functionAppName
  params: {
    functionAppName: functionAppName
    appServicePlanName: functionServicePlanName
    location: location
    logAnalyticsId: logAnalytics.id
    applicationInsightsInstrumentationKey: application_insights.outputs.instrumentationKey
    storageAccountName: storageAccountName
    storageAccountQueueEndPoint: storage_account.outputs.storageAccountQueueEndPoint
    storageAccountBlobEndPoint: storage_account.outputs.storageAccountBlobEndPoint
    storageAccountTableEndPoint: storage_account.outputs.storageAccountTableEndPoint
    sqlDatabaseName: sqlDatabaseName
    sqlDatabaseNameForLicenseHolder: sqlDatabaseNameForLicenseHolder
    sqlServerHostname: sql.outputs.sqlServerHostname
    automationFunctionBaseURL: automationFunctionBaseURL
    keyVaultName: keyVaultName
  }
}
