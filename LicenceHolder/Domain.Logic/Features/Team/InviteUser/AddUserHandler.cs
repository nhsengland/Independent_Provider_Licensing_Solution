using Azure.Core;
using Database.Entites;
using Database.Entites.Factories;
using Database.Repositories;
using Database.Repositories.EmailNotification;
using Database.Repositories.User;
using Domain.Logic.Factories;
using Domain.Logic.Features.Messages;
using Domain.Logic.Features.Messages.Requests;
using Domain.Logic.Integrations.Automation;
using Domain.Logic.Integrations.Automation.Factories;
using Domain.Logic.Integrations.StorageAccount.Queues;
using Domain.Logic.Integrations.StorageAccount.Queues.Factories;
using Domain.Objects.Exceptions;
using Domain.Objects.Integrations.StorageAccounts.Queues;
using Microsoft.Extensions.Logging;

namespace Domain.Logic.Features.Team.InviteUser;

public class AddUserHandler(
    ILogger<AddUserHandler> logger,
    IRepositoryForUser repositoryForUser,
    IOrganisationRepository organisationRepository,
    IStorageAccountQueueWrapper storageAccountQueueWrapper,
    ICreateOktaUserFactory createOktaUserFactory,
    IAutomationAPIWrapper automationAPIWrapper,
    IEmailBodyTemplateFactory emailBodyTemplateFactory,
    AddUserHandlerConfiguration configuration,
    IRepositoryForEmailNotification repositoryForEmailNotification,
    IEmailNotificationFactory emailNotificationFactory,
    IQueueMessageFactory queueMessageFactory,
    IMessagesHandler messagesHandler,
    IDateTimeFactory dateTimeFactory) : IAddUserHandler
{
    private readonly ILogger<AddUserHandler> logger = logger;
    private readonly IRepositoryForUser repositoryForUser = repositoryForUser;
    private readonly IOrganisationRepository organisationRepository = organisationRepository;
    private readonly IStorageAccountQueueWrapper storageAccountQueueWrapper = storageAccountQueueWrapper;
    private readonly ICreateOktaUserFactory createOktaUserFactory = createOktaUserFactory;
    private readonly IAutomationAPIWrapper automationAPIWrapper = automationAPIWrapper;
    private readonly IEmailBodyTemplateFactory emailBodyTemplateFactory = emailBodyTemplateFactory;
    private readonly AddUserHandlerConfiguration configuration = configuration;
    private readonly IRepositoryForEmailNotification repositoryForEmailNotification = repositoryForEmailNotification;
    private readonly IEmailNotificationFactory emailNotificationFactory = emailNotificationFactory;
    private readonly IQueueMessageFactory queueMessageFactory = queueMessageFactory;
    private readonly IMessagesHandler messagesHandler = messagesHandler;
    private readonly IDateTimeFactory dateTimeFactory = dateTimeFactory;

    public async Task Create(
        NewUserRequest newUserRequest,
        CancellationToken cancellationToken)
    {

        var requestor = await GetRequestor(newUserRequest.RequestedById, cancellationToken);

        var user = new Database.Entites.User()
        {
            OktaId = null,
            Firstname = newUserRequest.Firstname,
            Lastname = newUserRequest.Lastname,
            Email = newUserRequest.Email,
            EmailIsVerified = false,
            UserRoleId = (int)newUserRequest.UserRole,
            OrganisationId = newUserRequest.OrganisationId,
        };

        await repositoryForUser.AddAsync(user, cancellationToken);

        var emailNotification = emailNotificationFactory.Create(user.Id, (int)Objects.Database.EmailNotificationType.InviteUser, newUserRequest.RequestedById);

        await repositoryForEmailNotification.AddAsync(emailNotification, cancellationToken);

        await storageAccountQueueWrapper.PutMessageOntoCreateNewUserQueue(queueMessageFactory.Create(user.Id, emailNotification.Id), cancellationToken);

        await storageAccountQueueWrapper.PutMessageOntoAutomationQueueToRefreshSharePointPermissions(
            new RefreshOktaUserPermissions() { ObjectId = user.Id },
            cancellationToken);

        await messagesHandler.SendAsync(
                       new InvitationToJoinPortalRequest
                       {
                           RequestorName = $"{requestor.Firstname} {requestor.Lastname}",
                           OrganisationId = newUserRequest.OrganisationId,
                           InviteeEmail = newUserRequest.Email,
                           InviteeName = $"{newUserRequest.Firstname} {newUserRequest.Lastname}",
                           RequestedOn = dateTimeFactory.Create(),
                       }, cancellationToken);
    }

    public async Task CreateOktaUser(
        CreateOktaUserInputModel input, CancellationToken cancellationToken)
    {
        var user = await repositoryForUser.GetByIdAsync(input.UserId, cancellationToken) ?? throw new Exception($"User not found: {input.UserId}");

        if (user.IsDeleted)
        {
            logger.LogInformation("User is set as deleted so we will not create it: {userId}", input.UserId);
            return;
        }

        var organisation = await organisationRepository.GetByUserIdAsync(input.UserId, cancellationToken);

        var createOktaUser = createOktaUserFactory.Create(user.Firstname, user.Lastname, user.Email, organisation.Name);

        var result = await automationAPIWrapper.CreateOktaUser(createOktaUser);

        user.OktaId = result.User.Id;

        await repositoryForUser.SaveChangesAsync(cancellationToken);

        if (!await repositoryForEmailNotification.HasBeenSent(input.EmailNotificationId, cancellationToken))
        {
            var requestorName = await repositoryForEmailNotification.RequestedByFullName(input.EmailNotificationId, cancellationToken);

            var emailBodyTemplate = string.IsNullOrWhiteSpace(result.ActivationUrl) switch
            {
                false => emailBodyTemplateFactory.CreateForNewOktaUser(
                    user.Email,
                    $"{user.Firstname} {user.Lastname}",
                    result.ActivationUrl,
                    configuration.LicenseHolderApplicationURL,
                    requestorName,
                    organisation.Name),

                true => emailBodyTemplateFactory.CreateForExistingOktaUser(
                    user.Email,
                    $"{user.Firstname} {user.Lastname}",
                    configuration.LicenseHolderApplicationURL,
                    requestorName,
                    organisation.Name),
            };

            await automationAPIWrapper.SendEmail(emailBodyTemplate);

            await repositoryForEmailNotification.MarkAsSent(input.EmailNotificationId, cancellationToken);
        }
    }

    public Task<bool> IsEmailInUse(string email, CancellationToken cancellationToken)
    {
        return repositoryForUser.IsEmailInUse(email, cancellationToken);
    }

    public bool IsEmailDomainBlackListed(string email, CancellationToken cancellationToken)
    {
        string[] blackListOfEmailDomains = ["gmail.com", "yahoo.com", "outlook.com", "protonmail.com", "hotmail.com", "hotmail.co.uk", "aol.com", "icloud.com", "mail.com", "zoho.com", "yahoo.co.uk", "live.com", "ymail.com"];

        if (blackListOfEmailDomains.Any(email.Contains))
        {
            return true;
        }

        return false;
    }

    public async Task ReInviteUser(int userId, string requestorOktaUserId, CancellationToken cancellationToken)
    {
        /* 
         * to re-invite a user we will use the same flow as inviting a new user
         * as the underlying logic has been designed to handle this scenario
         */

        var requestedById = await repositoryForUser.GetIdAsync(requestorOktaUserId, cancellationToken);

        var requestor = await GetRequestor(requestedById, cancellationToken);

        var invitee = await GetRequestor(requestedById, cancellationToken);

        var emailNotification = emailNotificationFactory.Create(userId, (int)Objects.Database.EmailNotificationType.ReSendInvite, requestedById);

        await repositoryForEmailNotification.AddAsync(emailNotification, cancellationToken);

        await storageAccountQueueWrapper.PutMessageOntoCreateNewUserQueue(queueMessageFactory.Create(userId, emailNotification.Id), cancellationToken);

        await messagesHandler.SendAsync(
                       new ReIssureInvitationToJoinPortalRequest
                       {
                           RequestorName = $"{requestor.Firstname} {requestor.Lastname}",
                           OrganisationId = invitee.OrganisationId,
                           InviteeEmail = invitee.Email,
                           InviteeName = $"{invitee.Firstname} {invitee.Lastname}",
                           RequestedOn = dateTimeFactory.Create()
                       }, cancellationToken);
    }

    private async Task<User> GetRequestor(int id, CancellationToken cancellationToken)
    {
        return await repositoryForUser.GetByIdAsync(id, cancellationToken) ?? throw new NotFoundException($"User with id of: {id}");
    }
}
