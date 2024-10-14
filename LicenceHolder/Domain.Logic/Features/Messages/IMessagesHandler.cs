using Domain.Logic.Features.Messages.Requests;
using Domain.Objects.ViewModels.Messages;

namespace Domain.Logic.Features.Messages;

public interface IMessagesHandler
{
    Task<MessagesDashboardViewModel> GetAsync(
        GetMessagesDashboardViewModelRequest request,
        CancellationToken cancellationToken);

    Task<MessageDTO> GetAsync(
        GetMessageViewModelRequest request,
        CancellationToken cancellationToken);

    Task SendAsync(
        ChangeRequestForCompanyAddress request,
        CancellationToken cancellationToken);

    Task SendAsync(
        ChangeRequestForCompanyName request,
        CancellationToken cancellationToken);

    Task SendAsync(
        ChangeRequestForFYE request,
        CancellationToken cancellationToken);

    Task SendAsync(
        InvitationToJoinPortalRequest request,
        CancellationToken cancellationToken);

    Task SendAsync(
        PendingUserDeletedMessageRequest request,
        CancellationToken cancellationToken);

    Task SendAsync(
        ReIssureInvitationToJoinPortalRequest request,
        CancellationToken cancellationToken);

    Task SendAsync(
        SendNotificationQuery request,
        CancellationToken cancellationToken);

    Task SendAsync(
        UserDeletedMessageRequest request,
        CancellationToken cancellationToken);

    Task SendAsync(
        UpdateAnnualCertificateTaskStatusRequest request,
        CancellationToken cancellationToken);

    Task SendAsync(
        UpdateFinancialMonitorngTaskStatusRequest request,
        CancellationToken cancellationToken);
}
