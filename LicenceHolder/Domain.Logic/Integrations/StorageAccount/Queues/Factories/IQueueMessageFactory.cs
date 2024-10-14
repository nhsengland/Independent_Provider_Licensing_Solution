using Domain.Objects.Integrations.StorageAccounts.Queues;

namespace Domain.Logic.Integrations.StorageAccount.Queues.Factories;

public interface IQueueMessageFactory
{
    CreateOktaUserInputModel Create(int userId, int emailNotificationId);
}
