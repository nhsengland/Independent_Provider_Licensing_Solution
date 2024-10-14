using Database.Repositories;
using Database.Repositories.User;
using Domain.Logic.Factories;
using Domain.Logic.Features.Messages.Factories;
using Domain.Logic.Features.Messages.Requests;
using Domain.Objects;
using Domain.Objects.ViewModels.Messages;

namespace Domain.Logic.Features.Messages;

public class MessagesHandler(
    IMessageRepository messageRepository,
    IMessageDateFactory notificationDateFactory,
    IRepositoryForUser repositoryForUser,
    IMessagePropertyFactory notificationPropertyFactory,
    MessagesConfiguration configuration,
    IPageCountFactory pageCountFactory,
    ISkipFactory skipFactory,
    IMessageBodyFactory notificationBodyFactory,
    IMessageTitleFactory notificationTitleFactory,
    IOrganisationRepository organisationRepository,
    IDateTimeFactory dateTimeFactory) : IMessagesHandler
{
    private readonly IMessageRepository messageRepository = messageRepository;
    private readonly IMessageDateFactory notificationDateFactory = notificationDateFactory;
    private readonly IRepositoryForUser repositoryForUser = repositoryForUser;
    private readonly IMessagePropertyFactory notificationPropertyFactory = notificationPropertyFactory;
    private readonly MessagesConfiguration configuration = configuration;
    private readonly IPageCountFactory pageCountFactory = pageCountFactory;
    private readonly ISkipFactory skipFactory = skipFactory;
    private readonly IMessageBodyFactory notificationBodyFactory = notificationBodyFactory;
    private readonly IMessageTitleFactory notificationTitleFactory = notificationTitleFactory;
    private readonly IOrganisationRepository organisationRepository = organisationRepository;
    private readonly IDateTimeFactory dateTimeFactory = dateTimeFactory;

    public async Task<MessageDTO> GetAsync(
    GetMessageViewModelRequest request,
    CancellationToken cancellationToken)
    {
        request.OrganisationId = await repositoryForUser.GetOrganisationId(request.UserOktaId, cancellationToken);

        await messageRepository.MarkAsRead(request.OrganisationId, request.NotificationId);

        var notification = await messageRepository.GetMessage(request.OrganisationId, request.NotificationId);

        return new MessageDTO
        {
            Id = notification.Id,
            Title = notification.Title,
            Body = notification.Body,
            From = notification.From,
            IsRead = notification.IsRead,
            DateSent = notificationDateFactory.CreateDate(notification.SendDateTime),
            ActualDateSent = notification.SendDateTime
        };
    }

    public async Task<MessagesDashboardViewModel> GetAsync(
        GetMessagesDashboardViewModelRequest request,
        CancellationToken cancellationToken)
    {
        /* Page number starts at zero */
        if (request.PageNumber < 0)
        {
            /* a negitive number will cause a runtime exception */
            request.PageNumber = 1;
        }

        request.OrganisationId = await repositoryForUser.GetOrganisationId(request.UserOktaId, cancellationToken);

        /* page number is zero based */
        var skip = skipFactory.Create(request.PageNumber, configuration.PageSize);

        var notifications = await messageRepository.GetMesages(request.OrganisationId, skip, configuration.PageSize, request.Type);

        var viewModel = new List<MessageDTO>();

        foreach (var notification in notifications)
        {
            viewModel.Add(new MessageDTO
            {
                Id = notification.Id,
                IsRead = notification.IsRead,
                From = notification.From,
                Title = notificationPropertyFactory.CreateShortendVersion(notification.Title),
                Body = notificationPropertyFactory.CreateShortendVersion(notification.Body),
                DateSent = notificationDateFactory.CreateDate(notification.SendDateTime),
                ActualDateSent = notification.SendDateTime
            });
        }

        var totalNumberOfNotifications = await messageRepository.NumberOfMessages(request.OrganisationId, request.Type);

        var totalNumberOfPages = pageCountFactory.Create(totalNumberOfNotifications, configuration.PageSize);

        return new MessagesDashboardViewModel()
        {
            TotalNumberOfPages = totalNumberOfPages,
            CurrentPageNumber = request.PageNumber,
            Messages = viewModel,
            NumberOfUnreadMessages = await messageRepository.NumberOfUnReadMessages(request.OrganisationId, request.Type),
            OrganisationName = await organisationRepository.GetOrganisationNameAsync(request.OrganisationId, cancellationToken)
        };
    }

    public async Task SendAsync(PendingUserDeletedMessageRequest request, CancellationToken cancellationToken)
    {
        await messageRepository.AddAsync(new Database.Entites.Message()
        {
            OrganisationId = request.OrganisationId,
            Title = notificationTitleFactory.Create(request),
            Body = notificationBodyFactory.Create(request),
            From = NotificationConstants.From,
            IsRead = false,
            MessageTypeId = (int)Objects.Database.MessageType.Inbound,
            SendDateTime = dateTimeFactory.Create()
        }, cancellationToken);
    }

    public async Task SendAsync(ChangeRequestForCompanyAddress request, CancellationToken cancellationToken)
    {
        await messageRepository.AddAsync(new Database.Entites.Message()
        {
            OrganisationId = request.OrganisationId,
            Title = notificationTitleFactory.Create(
                Objects.Database.ChangeRequestType.Address,
                request.CompanyName,
                request.LicenseNumber),
            Body = notificationBodyFactory.Create(request),
            From = NotificationConstants.From,
            IsRead = false,
            MessageTypeId = (int)Objects.Database.MessageType.Inbound,
            SendDateTime = dateTimeFactory.Create()
        }, cancellationToken);
    }

    public async Task SendAsync(ChangeRequestForCompanyName request, CancellationToken cancellationToken)
    {
        await messageRepository.AddAsync(new Database.Entites.Message()
        {
            OrganisationId = request.OrganisationId,
            Title = notificationTitleFactory.Create(
                Objects.Database.ChangeRequestType.Name,
                request.PreviousCompanyName,
                request.LicenseNumber),
            Body = notificationBodyFactory.Create(request),
            From = NotificationConstants.From,
            IsRead = false,
            MessageTypeId = (int)Objects.Database.MessageType.Inbound,
            SendDateTime = dateTimeFactory.Create()
        }, cancellationToken);
    }

    public async Task SendAsync(
        ChangeRequestForFYE request, CancellationToken cancellationToken)
    {
        await messageRepository.AddAsync(new Database.Entites.Message()
        {
            OrganisationId = request.OrganisationId,
            Title = notificationTitleFactory.Create(
                Objects.Database.ChangeRequestType.FinancialYearEnd,
                request.CompanyName,
                request.LicenseNumber),
            Body = notificationBodyFactory.Create(request),
            From = NotificationConstants.From,
            IsRead = false,
            MessageTypeId = (int)Objects.Database.MessageType.Inbound,
            SendDateTime = dateTimeFactory.Create()
        }, cancellationToken);
    }

    public async Task SendAsync(
        SendNotificationQuery request,
        CancellationToken cancellationToken)
    {
        var user = await repositoryForUser.GetDetails(request.UserOktaId, cancellationToken);

        await messageRepository.SendMessage(
            await repositoryForUser.GetOrganisationId(request.UserOktaId, cancellationToken),
            request.Model.Body.Trim(),
            request.Model.Title,
            $"{user.Forename} {user.Surname}",
            dateTimeFactory.Create(),
            Objects.Database.MessageType.Outbound);
    }

    public async Task SendAsync(
        InvitationToJoinPortalRequest request, CancellationToken cancellationToken)
    {
        await messageRepository.AddAsync(new Database.Entites.Message()
        {
            OrganisationId = request.OrganisationId,
            Title = notificationTitleFactory.Create(request),
            Body = notificationBodyFactory.Create(request),
            From = NotificationConstants.From,
            IsRead = false,
            MessageTypeId = (int)Objects.Database.MessageType.Inbound,
            SendDateTime = dateTimeFactory.Create()
        }, cancellationToken);
    }

    public async Task SendAsync(
        ReIssureInvitationToJoinPortalRequest request, CancellationToken cancellationToken)
    {
        await messageRepository.AddAsync(new Database.Entites.Message()
        {
            OrganisationId = request.OrganisationId,
            Title = notificationTitleFactory.Create(request),
            Body = notificationBodyFactory.Create(request),
            From = NotificationConstants.From,
            IsRead = false,
            MessageTypeId = (int)Objects.Database.MessageType.Inbound,
            SendDateTime = dateTimeFactory.Create()
        }, cancellationToken);
    }

    public async Task SendAsync(UserDeletedMessageRequest request, CancellationToken cancellationToken)
    {
        await messageRepository.AddAsync(new Database.Entites.Message()
        {
            OrganisationId = request.OrganisationId,
            Title = notificationTitleFactory.Create(request),
            Body = notificationBodyFactory.Create(request),
            From = NotificationConstants.From,
            IsRead = false,
            MessageTypeId = (int)Objects.Database.MessageType.Inbound,
            SendDateTime = dateTimeFactory.Create()
        }, cancellationToken);
    }

    public async Task SendAsync(
        UpdateAnnualCertificateTaskStatusRequest request,
        CancellationToken cancellationToken)
    {
        await messageRepository.AddAsync(new Database.Entites.Message()
        {
            OrganisationId = request.OrganisationId,
            Title = notificationTitleFactory.Create(request),
            Body = notificationBodyFactory.Create(request),
            From = NotificationConstants.From,
            IsRead = false,
            MessageTypeId = (int)Objects.Database.MessageType.Inbound,
            SendDateTime = dateTimeFactory.Create()
        }, cancellationToken);
    }

    public async Task SendAsync(UpdateFinancialMonitorngTaskStatusRequest request, CancellationToken cancellationToken)
    {
        await messageRepository.AddAsync(new Database.Entites.Message()
        {
            OrganisationId = request.OrganisationId,
            Title = notificationTitleFactory.Create(request),
            Body = notificationBodyFactory.Create(request),
            From = NotificationConstants.From,
            IsRead = false,
            MessageTypeId = (int)Objects.Database.MessageType.Inbound,
            SendDateTime = dateTimeFactory.Create()
        }, cancellationToken);
    }
}
