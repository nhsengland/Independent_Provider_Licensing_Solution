using Domain.Logic.Features.Messages.Requests;
using Domain.Objects.Database;

namespace Domain.Logic.Features.Messages.Factories;

public interface IMessageTitleFactory
{
    string Create(ChangeRequestType request, string companyName, string licenceName);

    string Create(InvitationToJoinPortalRequest request);

    string Create(PendingUserDeletedMessageRequest request);

    string Create(ReIssureInvitationToJoinPortalRequest request);

    string Create(UserDeletedMessageRequest request);

    string Create(UpdateAnnualCertificateTaskStatusRequest request);

    string Create(UpdateFinancialMonitorngTaskStatusRequest request);
}
