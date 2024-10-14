@description('Location for all resources.')
param location string = resourceGroup().location

@description('Unique Suffix')
param uniqueSuffix string

@secure()
@description('Authentication App Registration Secret')
param authenticationAppSecret string

@secure()
@description('Okta Private Key')
param oktaPrivateKey string

@secure()
@description('GOV.UK Notify API Key')
param govUKNotifyAPIKey string

var namePrefix = 'auto'
var keyVaultName = '${namePrefix}-vault-${uniqueSuffix}'

resource vault 'Microsoft.KeyVault/vaults@2021-11-01-preview' = {
  name: keyVaultName
  location: location
  properties: {
    accessPolicies:[]
    enableRbacAuthorization: true
    enableSoftDelete: true
    softDeleteRetentionInDays: 90
    enabledForDeployment: false
    enabledForDiskEncryption: false
    enabledForTemplateDeployment: true
    tenantId: subscription().tenantId
    sku: {
      name: 'standard'
      family: 'A'
    }
    networkAcls: {
      defaultAction: 'Allow'
      bypass: 'AzureServices'
    }
  }
}

resource oktaSecret 'Microsoft.KeyVault/vaults/secrets@2021-11-01-preview' = {
  parent: vault
  name: 'OktaPrivateKey'
  properties: {
    value: oktaPrivateKey
  }
}

resource govUkNotifySecret 'Microsoft.KeyVault/vaults/secrets@2021-11-01-preview' = {
  parent: vault
  name: 'GovUKNotifyAPIKey'
  properties: {
    value: govUKNotifyAPIKey
  }
}

resource authenticationAppRegistrationSecret 'Microsoft.KeyVault/vaults/secrets@2021-11-01-preview' = {
  parent: vault
  name: 'AuthenticationAppSecret'
  properties: {
    value: authenticationAppSecret
  }
}
