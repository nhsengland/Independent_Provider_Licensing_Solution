using Domain.Logic.Features.Messages.Requests;

namespace Domain.Logic.Features.Messages.Factories;

public interface IMessageBodyFactory
{
    string Create(ChangeRequestForCompanyAddress request);

    string Create(ChangeRequestForCompanyName request);

    string Create(ChangeRequestForFYE request);

    string Create(InvitationToJoinPortalRequest request);

    string Create(PendingUserDeletedMessageRequest request);

    string Create(ReIssureInvitationToJoinPortalRequest request);

    string Create(UserDeletedMessageRequest request);

    string Create(UpdateAnnualCertificateTaskStatusRequest request);

    string Create(UpdateFinancialMonitorngTaskStatusRequest request);
}
