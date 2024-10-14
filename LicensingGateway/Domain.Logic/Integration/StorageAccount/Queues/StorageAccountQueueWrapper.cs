using Domain.Logic.Integration.StorageAccount.Queues.Factories;
using Microsoft.Extensions.Logging;

namespace Domain.Logic.Integration.StorageAccount.Queues;
public class StorageAccountQueueWrapper(
    ILogger<StorageAccountQueueWrapper> logger,
    QueueClientConfiguration configuration,
    IQueueClientFactory queueClientFactory,
    IQueueMessageEncoder queueMessageEncoder) : IStorageAccountQueueWrapper
{
    private readonly ILogger<StorageAccountQueueWrapper> logger = logger;
    private readonly QueueClientConfiguration configuration = configuration;
    private readonly IQueueClientFactory queueClientFactory = queueClientFactory;
    private readonly IQueueMessageEncoder queueMessageEncoder = queueMessageEncoder;

    public async Task PutMessageOntoEmailNotificationQueue(int id)
    {
        var queueClient = await queueClientFactory.Create(configuration.EmailNotificationQueueName);

        await queueClient.SendMessageAsync(queueMessageEncoder.ToBase64String(id.ToString()));
    }
}
