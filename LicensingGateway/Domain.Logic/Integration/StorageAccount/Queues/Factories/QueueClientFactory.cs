using Azure.Identity;
using Azure.Storage.Queues;

namespace Domain.Logic.Integration.StorageAccount.Queues.Factories;
public class QueueClientFactory(
    QueueClientConfiguration configuration) : IQueueClientFactory
{
    private readonly QueueClientConfiguration configuration = configuration;

    public async Task<QueueClient> Create(string queueName)
    {
        QueueClient queueClient;

        if(configuration.StorageAccountName == "UseDevelopmentStorage=true")
        {
            queueClient = new QueueClient(configuration.StorageAccountName, queueName);
        }
        else
        {
            queueClient = new QueueClient(
                new Uri($"https://{configuration.StorageAccountName}.queue.core.windows.net/{queueName}"),
                new DefaultAzureCredential());
        }

        await queueClient.CreateIfNotExistsAsync();

        return queueClient;
    }
}
