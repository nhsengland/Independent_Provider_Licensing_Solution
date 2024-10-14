using Azure.Identity;
using Azure.Storage.Queues;

namespace Domain.Logic.Integrations.StorageAccount.Queues.Factories;
public class QueueClientFactory(
    QueueClientConfiguration configuration) : IQueueClientFactory
{
    private readonly QueueClientConfiguration configuration = configuration;

    public async Task<QueueClient> Create(string queueName)
    {
        return await CreateQueue(configuration.StorageAccountName, queueName);
    }

    public async Task<QueueClient> CreateOnAutomationStorageAccount(string queueName)
    {
        return await CreateQueue(configuration.AutomationStorageAccountName, queueName);
    }

    private async Task<QueueClient> CreateQueue(
        string storageAccountName,
        string queueName)
    {
        QueueClient queueClient;

        if (configuration.StorageAccountName == "UseDevelopmentStorage=true")
        {
            queueClient = new QueueClient(storageAccountName, queueName);
        }
        else
        {
            queueClient = new QueueClient(
                new Uri($"https://{storageAccountName}.queue.core.windows.net/{queueName}"),
                new DefaultAzureCredential());
        }

        await queueClient.CreateIfNotExistsAsync();

        return queueClient;
    }
}
