using Pulumi;
using Pulumi.Azure.Core;
using Pulumi.Azure.Storage;

class MyStack : Stack
{
    public MyStack()
    {
        // Create an Azure Resource Group
        var resourceGroup = new ResourceGroup("resourceGroup", new ResourceGroupArgs
        {
            // adding arguments to control my resource group
            // i do not use location, as this is defined in Pulumi.dev.yaml
            Name = "rg-PulumiStorage"
        });

        // Create an Azure Storage Account
        var storageAccount = new Account("storage", new AccountArgs
        {
            // when not specifying name. Pulumi will generate a random name for us.
            ResourceGroupName = resourceGroup.Name, // use outputs from the resource group
            AccountReplicationType = "LRS",
            AccountTier = "Standard",
            EnableHttpsTrafficOnly = true
        });

        // create a storage account container
        var strContainer = new Container("container", new ContainerArgs
        {
            StorageAccountName = storageAccount.Name,
            Name = "images"
        });

        // Export the connection string for the storage account
        this.ConnectionString = storageAccount.PrimaryConnectionString;
    }

    [Output]
    public Output<string> ConnectionString { get; set; }
}
