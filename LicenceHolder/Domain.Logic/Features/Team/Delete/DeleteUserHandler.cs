using Database.Entites;
using Database.Repositories.EmailNotification;
using Database.Repositories.User;
using Domain.Logic.Factories;
using Domain.Logic.Features.Messages;
using Domain.Logic.Features.Messages.Requests;
using Domain.Logic.Features.Team.Delete.Requests;
using Domain.Logic.Features.Team.ManageUsers;
using Domain.Logic.Features.Team.ManageUsers.Queries;
using Domain.Logic.Integrations.StorageAccount.Queues;
using Domain.Objects.Integrations.StorageAccounts.Queues;
using Licence.Holder.Application.Models.Team;

namespace Domain.Logic.Features.Team.Delete;

public class DeleteUserHandler(
    IManageUsersHandler manageUsersHandler,
    IRepositoryForEmailNotification repositoryForEmailNotification,
    IRepositoryForUser repositoryForUser,
    IStorageAccountQueueWrapper storageAccountQueueWrapper,
    IMessagesHandler messagesHandler,
    IDateTimeFactory dateTimeFactory) : IDeleteUserHandler
{
    private readonly IManageUsersHandler manageUsersHandler = manageUsersHandler;
    private readonly IRepositoryForEmailNotification repositoryForEmailNotification = repositoryForEmailNotification;
    private readonly IRepositoryForUser repositoryForUser = repositoryForUser;
    private readonly IStorageAccountQueueWrapper storageAccountQueueWrapper = storageAccountQueueWrapper;
    private readonly IMessagesHandler messagesHandler = messagesHandler;
    private readonly IDateTimeFactory dateTimeFactory = dateTimeFactory;

    public async Task<DeleteUserDetailsDTO> Get(GetUserDetailsForDeleteUserRequest request, CancellationToken cancellationToken)
    {
        await ThrowIfUsersNotInSameOrganisation(request.UserID, request.OktaUserId, cancellationToken);

        var userDetails = await manageUsersHandler.Get(new GetManageUserViewModelQuery() { OktaUserId = request.OktaUserId, UserId = request.UserID }, cancellationToken);

        return new DeleteUserDetailsDTO()
        {
            FirstName = userDetails.FirstName,
            LastName = userDetails.LastName,
            Email = userDetails.Email,
            IsVerified = userDetails.IsVerified,
            DateLastEmailNotificationWasCreated = await repositoryForEmailNotification.GetDateOfLatestEmailNotification(request.UserID, cancellationToken),
        };
    }

    public async Task SoftDeleteUser(
        string oktaId,
        int userId, CancellationToken cancellationToken)
    {
        if (await UsersAreInTheSameOrganisation(userId, oktaId, cancellationToken))
        {
            var requestorDetails = await repositoryForUser.GetDetails(oktaId, cancellationToken) ?? throw new Exception($"User not found: {userId}");

            var user = await repositoryForUser.GetByIdAsync(userId, cancellationToken) ?? throw new Exception($"User not found: {userId}");

            user.IsDeleted = true;

            await repositoryForUser.SaveChangesAsync(cancellationToken);

            await storageAccountQueueWrapper.PutMessageOntoRemoveUserFromOktaGroupQueue(user.Id, cancellationToken);

            await storageAccountQueueWrapper.PutMessageOntoAutomationQueueToRefreshSharePointPermissions(new RefreshOktaUserPermissions() { ObjectId = user.Id }, cancellationToken);

            if (user.EmailIsVerified)
            {
                await messagesHandler.SendAsync(new UserDeletedMessageRequest() {
                    OrganisationId = user.OrganisationId,
                    RequestorName = $"{requestorDetails.Forename} {requestorDetails.Surname}",
                    InviteeEmail = user.Email,
                    InviteeName = $"{user.Firstname} {user.Lastname}",
                    RequestedOn = dateTimeFactory.Create()
                }, cancellationToken);
            }
            else
            {
                await messagesHandler.SendAsync(new PendingUserDeletedMessageRequest() {
                    OrganisationId = user.OrganisationId,
                    RequestorName = $"{requestorDetails.Forename} {requestorDetails.Surname}",
                    InviteeEmail = user.Email,
                    InviteeName = $"{user.Firstname} {user.Lastname}",
                    RequestedOn = dateTimeFactory.Create()
                }, cancellationToken);
            }
        }
    }

    public async Task<bool> UserEmailIsVerified(int userId, CancellationToken cancellationToken)
    {
        var user = await repositoryForUser.GetByIdAsync(userId, cancellationToken) ?? throw new Exception($"User not found: {userId}");

        return user.EmailIsVerified;
    }

    public async Task<bool> UsersAreInTheSameOrganisation(int userId, string requestorOktaId, CancellationToken cancellationToken)
    {
        return await manageUsersHandler.UsersAreInTheSameOrganisation(new AllowedToEditUserQuery() { OktaUserId = requestorOktaId, UserId = userId }, cancellationToken);
    }

    private async Task ThrowIfUsersNotInSameOrganisation(int id, string oktaId, CancellationToken cancellationToken)
    {
        var inSameOrg = await UsersAreInTheSameOrganisation(id, oktaId, cancellationToken);

        if (!inSameOrg)
        {
            throw new Exception($"User {id} not in same organisation: {oktaId}");
        }

        return;
    }
}
