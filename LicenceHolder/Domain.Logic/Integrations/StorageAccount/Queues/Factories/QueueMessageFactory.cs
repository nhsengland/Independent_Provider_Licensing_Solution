using Domain.Objects.Integrations.StorageAccounts.Queues;

namespace Domain.Logic.Integrations.StorageAccount.Queues.Factories;

public class QueueMessageFactory : IQueueMessageFactory
{
    public CreateOktaUserInputModel Create(int userId, int emailNotificationId)
    {
        return new CreateOktaUserInputModel
        {
            UserId = userId,
            EmailNotificationId = emailNotificationId
        };
    }
}
