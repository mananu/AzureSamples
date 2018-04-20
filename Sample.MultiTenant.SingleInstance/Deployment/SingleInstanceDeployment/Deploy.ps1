<#
	.Synopsis
	Multi-Tenant Sample Demo Environment.
	.DESCRIPTION
	This script is used to create a new multi-tenant sample Demo Environment.
	Update the paramenters in the Tenant1Params.json file.
	.EXAMPLE
	Deploy -resourceGroupLocation <Azure Location>
#>

[CmdletBinding()]
param(
	
	[string]
	$resourceGroupLocation,

	[string]
	$user
)
## This function initializes the subscription and lists available subscriptions to select from

## Variables
$user = $user.ToLower() -replace '\s',''
$path = (Get-Item -Path ".\" -Verbose).FullName + "\Templates"
$resourceGroupName = "saas-sample-"+$user
$DeploySaaSTemplateFile = "$path\SingleTenantDeploy.json"
$DeploySaaSParameterFile = "$path\Tenant1.parameters.json"

## Register needed Azure Resource Providers
$resourceProviders = @("microsoft.sql", "microsoft.web");
foreach($resourceProvider in $resourceProviders) {
	$registered = (Get-AzureRmResourceProvider -ProviderNamespace $resourceProvider).registrationstate
	If(!$registered)
	{
		Write-Host "Registering resource providers"
		Register-AzureRmResourceProvider -ProviderNamespace $resourceProvider;
    }
}

#Create or check for existing resource group
$resourceGroup = Get-AzureRmResourceGroup -Name $resourceGroupName -ErrorAction SilentlyContinue
if(!$resourceGroup)
{
    Write-Host "Resource group '$resourceGroupName' does not exist. To create a new resource group, please enter a location.";
    if(!$resourceGroupLocation) {
        $resourceGroupLocation = Read-Host "resourceGroupLocation";
    }
    Write-Host "Creating resource group '$resourceGroupName' in location '$resourceGroupLocation'";
    New-AzureRmResourceGroup -Name $resourceGroupName -Location $resourceGroupLocation
}
else{
    Write-Host "Using existing resource group '$resourceGroupName'";
}

# create tenant database by template
$starttime = $(Get-Date)
if(Test-Path $DeploySaaSTemplateFile)
{
	New-AzureRmResourceGroupDeployment -TemplateFile $DeploySaaSTemplateFile -user $user -ResourceGroupName $ResourceGroupName
}
else
{
	Write-Host "Unable to locate $DeploySaaSTemplateFile"
}
Write-Output (New-TimeSpan $starttime $(Get-Date)).TotalSeconds