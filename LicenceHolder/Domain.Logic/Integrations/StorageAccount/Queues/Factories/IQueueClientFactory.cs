using Azure.Storage.Queues;

namespace Domain.Logic.Integrations.StorageAccount.Queues.Factories;
public interface IQueueClientFactory
{
    Task<QueueClient> Create(string queueName);

    Task<QueueClient> CreateOnAutomationStorageAccount(string queueName);
}
