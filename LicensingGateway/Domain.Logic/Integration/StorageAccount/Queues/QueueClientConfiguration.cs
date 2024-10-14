namespace Domain.Logic.Integration.StorageAccount.Queues;
public class QueueClientConfiguration
{
    public string StorageAccountName { get; set; } = default!;

    public string EmailNotificationQueueName { get; set; } = default!;
}
