# Introduction 
TODO: Give a short introduction of your project. Let this section explain the objectives or the motivation behind this project. 

## Database
### Setup
Before you run this application on a local machine. The local machine will need to have SQL server development edition installed. After installation, you will need to create the database. Run the following command in the package manager console (in visual studio).

``` nuget
Update-database
```

### Migrations
To create a new migration, run the following command in the package manager console (in visual studio), targeting the Database project. As we have more than one database context in the solution we need to include the context name in the command. 

``` nuget
Add-Migration MigrationName -OutputDir "Migrations" -context "LicensingGatewayDbContext"
```

## Infrastructure as code (IAC)
We are creating the infrastructure in Azure, using BICEP [templates](/.iac/).

We have created the key-vault manually:
 - setting the resource access to checked (Azure Resource Manager for template deployment).
 - grant developers Key Vault Administrator role to resource 

### Manual Setup
Before starting the setup, there are a few steps that need to be performed:
 - Create all the resource groups, with the same location UK SOUTH
 - Within each resource group we will need to setup a key-vault
    - Azure role based access control
    - We need to grant the developers the role: Key Vault Administrator acce
 - Create an Entra Security Group. This security group is used to hold a collection of SQL server admins:
   - Navigate to Microsoft Entra in the azure portal
   - Create a new Security Group Called: *
   - Add Transparity developers to this group
   - Take a note of the Object ID

For the deployments to deploy the infrastructure, we need to setup a service principle between azure devops and the azure portal. We can do this from the devops portal and we want to create one for each environment:
 - Navigate to the project in azure devops
 - View the project settings
 - Click on service connections
 - New service connections
 - Select Azure Resource Manager using Workload Identity federation with OpenID Connect (automatic)
 - scope level: Subscription
    - Select subscription
    - Select Resource group for the environment you are creating for
    - Provide name
        

We then need to add all three of these service principles to the entra security group we created earlier in the process.

And lastly, grant the service connections the following roles:
- On the key-vault, we need to assign the following role to access/create secrets: Key Vault Administrator
- At the resource-group level, we want to grant the following role: Owner, but we want to constrain the role:
    - Allow user to only assign roles you select
        - Key Vault Secrets User
        - Storage Blob Data Contributor
        - Storage Queue Data Contributor
        - Storage Table Data Contributor
        - Storage File Data Privileged Contributor

### Deployments to an environment/resource group

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