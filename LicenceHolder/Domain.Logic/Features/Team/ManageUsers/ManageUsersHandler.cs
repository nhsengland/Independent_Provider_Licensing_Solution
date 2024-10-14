using Database.Repositories.EmailNotification;
using Database.Repositories.Orchestrate;
using Database.Repositories.User;
using Domain.Logic.Features.Team.ManageUsers.Queries;
using Domain.Logic.Features.Team.Maps;
using Domain.Logic.Integrations.Automation;
using Domain.Logic.Integrations.Automation.Factories;
using Domain.Logic.Integrations.StorageAccount.Queues;
using Domain.Objects.Integrations.StorageAccounts.Queues;
using Domain.Objects.ViewModels.Team;
using Microsoft.Extensions.Logging;

namespace Domain.Logic.Features.Team.ManageUsers;

public class ManageUsersHandler(
    ILogger<ManageUsersHandler> logger,
    IRepositoryOrchestrator repositoryOrchestrator,
    IRepositoryForUser repositoryForUser,
    IAccessLevelMapper accessLevelMapper,
    IAutomationAPIWrapper automationAPIWrapper,
    IRemoveUserFromGroupFactory removeUserFromGroupFactory,
    IRepositoryForEmailNotification repositoryForEmailNotification,
    IStorageAccountQueueWrapper storageAccountQueueWrapper) : IManageUsersHandler
{
    private readonly ILogger<ManageUsersHandler> logger = logger;
    private readonly IRepositoryOrchestrator repositoryOrchestrator = repositoryOrchestrator;
    private readonly IRepositoryForUser repositoryForUser = repositoryForUser;
    private readonly IAccessLevelMapper accessLevelMapper = accessLevelMapper;
    private readonly IAutomationAPIWrapper automationAPIWrapper = automationAPIWrapper;
    private readonly IRemoveUserFromGroupFactory removeUserFromGroupFactory = removeUserFromGroupFactory;
    private readonly IRepositoryForEmailNotification repositoryForEmailNotification = repositoryForEmailNotification;
    private readonly IStorageAccountQueueWrapper storageAccountQueueWrapper = storageAccountQueueWrapper;

    public async Task<ManageUsersViewModel> Get(
        GetManageUsersViewModelQuery request,
        CancellationToken cancellationToken)
    {
        var users = await repositoryOrchestrator.GetUsersInMyOrganisation(request.OktaUserId, cancellationToken);

        var orgaisationDetails = await repositoryOrchestrator.GetOrganisationGetDetails(request.OktaUserId, cancellationToken);

        var model = new ManageUsersViewModel()
        {
            OrganisationName = orgaisationDetails.Name,
            IsCrsOrHaredToReplaceOrganisation = orgaisationDetails.HasCrsOrHardToReplaceCompanys,
        };

        foreach (var user in users)
        {
            model.Users.Add(new ManageUsersViewModel.User
            {
                Id = user.Id,
                Email = user.Email,
                Firstname = user.Firstname,
                Lastname = user.Lastname,
                IsVerified = user.EmailIsVerified,
                Level = user.UserRoleId.ToString(),
            });
        }

        return model;
    }

    public async Task<ManageUserViewModel> Get(GetManageUserViewModelQuery request, CancellationToken cancellationToken)
    {
        var user = await repositoryForUser.GetByIdAsync(request.UserId, cancellationToken) ?? throw new InvalidOperationException($"User doesn't exist: OktaUserId {request.OktaUserId}, user: {request.UserId}");

        return new ManageUserViewModel()
        {
            Id = user.Id,
            FirstName = user.Firstname,
            LastName = user.Lastname,
            Email = user.Email,
            DateLastEmailNotificationWasCreated = await repositoryForEmailNotification.GetDateOfLatestEmailNotification(user.Id, cancellationToken),
            IsVerified = user.EmailIsVerified,
            AccessLevel = accessLevelMapper.MapToAccessLevel(user.UserRoleId),
            ShowAccessLevel = await repositoryOrchestrator.UserAllowedToSetAccessLevel(request.OktaUserId, cancellationToken),
        };
    }

    public async Task<ManageUserChangeAccessLevelViewModel> Get(GetManageUserChangeAccessLevelViewModel request, CancellationToken cancellationToken)
    {
        var user = await repositoryForUser.GetByIdAsync(request.UserId, cancellationToken) ?? throw new InvalidOperationException($"User doesn't exist: OktaUserId {request.OktaUserId}, user: {request.UserId}");

        return new ManageUserChangeAccessLevelViewModel()
        {
            Id = user.Id,
            FirstName = user.Firstname,
            LastName = user.Lastname,
            Email = user.Email,
            AccessLevel = accessLevelMapper.MapToAccessLevel(user.UserRoleId),
        };
    }

    public async Task RemoveOktaUserFromGroup(int userId, CancellationToken cancellationToken)
    {
        var user = await repositoryForUser.GetByIdAsync(userId, cancellationToken) ?? throw new Exception($"User not found: {userId}");

        if (user.OktaId == null)
        {
            logger.LogError("User doesn't have an OktaId: {userId}", userId);
            throw new InvalidOperationException($"User doesn't have an OktaId: {userId}");
        }

        await automationAPIWrapper.RemoveOktaUserFromGroup(removeUserFromGroupFactory.Create(user.OktaId));
    }

    public async Task UpdateUserAccessLevel(
        int userId,
        Objects.Database.UserRole userRole,
        CancellationToken cancellationToken)
    {
        var user = await repositoryForUser.GetByIdAsync(userId, cancellationToken) ?? throw new Exception($"User not found: {userId}");

        var roleIsDifferent = user.UserRoleId != (int)userRole;

        user.UserRoleId = (int)userRole;

        await repositoryForUser.SaveChangesAsync(cancellationToken);

        if (roleIsDifferent)
        {
            await storageAccountQueueWrapper.PutMessageOntoAutomationQueueToRefreshSharePointPermissions(new RefreshOktaUserPermissions() { ObjectId = user.Id }, cancellationToken);
        }
    }

    public Task<bool> UsersAreInTheSameOrganisation(AllowedToEditUserQuery request, CancellationToken cancellationToken)
    {
        return repositoryOrchestrator.RequestingUsersOrgansiationContainsUserWithId(request.OktaUserId, request.UserId, cancellationToken);
    }
}
