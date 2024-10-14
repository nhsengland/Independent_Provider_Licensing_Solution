using Domain.Logic.Integrations.StorageAccount.Queues.Factories;
using Domain.Objects.Integrations.StorageAccounts.Queues;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Domain.Logic.Integrations.StorageAccount.Queues;
public class StorageAccountQueueWrapper(
    QueueClientConfiguration configuration,
    IQueueClientFactory queueClientFactory,
    IQueueMessageEncoder queueMessageEncoder) : IStorageAccountQueueWrapper
{
    private readonly QueueClientConfiguration configuration = configuration;
    private readonly IQueueClientFactory queueClientFactory = queueClientFactory;
    private readonly IQueueMessageEncoder queueMessageEncoder = queueMessageEncoder;

    public async Task PutMessageOntoAutomationQueueToRefreshSharePointPermissions(RefreshOktaUserPermissions payload, CancellationToken cancellationToken)
    {
        await PutMessageOntoAutomationQueue(JsonSerializer.Serialize(payload), configuration.AutomationRefreshSharepointPermissionQueueName, cancellationToken);
    }

    public async Task PutMessageOntoCreateNewUserQueue(
        CreateOktaUserInputModel input,
        CancellationToken cancellationToken)
    {
        await PutMessageOntoQueue(JsonSerializer.Serialize(input), configuration.CreateOktaUserQueueName, cancellationToken);
    }

    public async Task PutMessageOntoRemoveUserFromOktaGroupQueue(
        int userId,
        CancellationToken cancellationToken)
    {
        await PutMessageOntoQueue(userId.ToString(), configuration.RemoveOktaUserFromGroupQueueName, cancellationToken);
    }

    private async Task PutMessageOntoAutomationQueue(
        string message,
        string queueName,
        CancellationToken cancellationToken)
    {
        var queueClient = await queueClientFactory.CreateOnAutomationStorageAccount(queueName);

        await queueClient.SendMessageAsync(queueMessageEncoder.ToBase64String(message), cancellationToken);
    }

    private async Task PutMessageOntoQueue(
        string message,
        string queueName,
        CancellationToken cancellationToken)
    {
        var queueClient = await queueClientFactory.Create(queueName);

        await queueClient.SendMessageAsync(queueMessageEncoder.ToBase64String(message), cancellationToken);
    }
}
