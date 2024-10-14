# Introduction 
TODO: Give a short introduction of your project. Let this section explain the objectives or the motivation behind this project. 

## Migrations
To create a new migration, run the following command in the package manager console (in visual studio), targeting the Database project:

``` nuget
Add-Migration InitialCreate
```

## Infrastructure as code (IAC)
We are creating the infrastructure in Azure, using BICEP [templates](/.iac/).

We are using a shared SQL instance, that has been created in the licensing gateway application.

### Deployment to an environment/resource group

``` powershell
az login --tenant #tenant/directory ID#
```

``` powershell
az bicep build --file main.bicep
```
``` powershell
az deployment group validate --resource-group $resourceGroupName --template-file main.bicep --parameters parameters/main.parameters.dev.json
```
``` powershell
az deployment group what-if --resource-group $resourceGroupName --template-file main.bicep --parameters parameters/main.parameters.dev.json
```
``` powershell
az deployment group create --resource-group $resourceGroupName --template-file main.bicep --parameters parameters/main.parameters.dev.json
```