using Domain.Objects.Integrations.StorageAccounts.Queues;

namespace Domain.Logic.Integrations.StorageAccount.Queues;
public interface IStorageAccountQueueWrapper
{
    Task PutMessageOntoCreateNewUserQueue(
        CreateOktaUserInputModel input,
        CancellationToken cancellationToken);

    Task PutMessageOntoAutomationQueueToRefreshSharePointPermissions(
        RefreshOktaUserPermissions payload,
        CancellationToken cancellationToken);

    Task PutMessageOntoRemoveUserFromOktaGroupQueue(
        int userId,
        CancellationToken cancellationToken);
}
