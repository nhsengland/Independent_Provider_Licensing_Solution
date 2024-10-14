namespace Domain.Logic.Integration.StorageAccount.Queues;
public interface IStorageAccountQueueWrapper
{
    Task PutMessageOntoEmailNotificationQueue(int id);
}
