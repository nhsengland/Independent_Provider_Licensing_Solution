namespace Domain.Logic.Integrations.StorageAccount.Queues;
public class QueueClientConfiguration
{
    public string StorageAccountName { get; set; } = default!;

    public string AutomationStorageAccountName { get; set; } = default!;

    public string AutomationRefreshSharepointPermissionQueueName { get; set; } = default!;

    public string CreateOktaUserQueueName { get; set; } = default!;

    public string RemoveOktaUserFromGroupQueueName { get; set; } = default!;
}
