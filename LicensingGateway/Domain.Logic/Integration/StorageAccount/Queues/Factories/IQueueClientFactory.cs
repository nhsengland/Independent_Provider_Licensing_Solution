using Azure.Storage.Queues;

namespace Domain.Logic.Integration.StorageAccount.Queues.Factories;
public interface IQueueClientFactory
{
    Task<QueueClient> Create(string queueName);
}
