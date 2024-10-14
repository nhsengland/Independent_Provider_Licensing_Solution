namespace Domain.Logic.Integrations.StorageAccount.Queues.Factories;
public interface IQueueMessageEncoder
{
    string ToBase64String(string message);
}
