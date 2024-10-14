using Domain.Logic.Features.Messages.Requests;
using Domain.Objects.Database;

namespace Domain.Logic.Features.Messages.Factories;

public class MessageTitleFactory : IMessageTitleFactory
{
    public string Create(ChangeRequestType request, string companyName, string licenceNumber)
    {
        return request switch
        {
            ChangeRequestType.Address => $"Notification of registered address change for {companyName}/{licenceNumber}",
            ChangeRequestType.Name => $"Notification of licensee name change request for {companyName}/{licenceNumber}",
            ChangeRequestType.FinancialYearEnd => $"Notification of financial year-end change request for {companyName}/{licenceNumber}",
            _ => throw new ArgumentOutOfRangeException(nameof(request), request, null)
        };
    }

    public string Create(InvitationToJoinPortalRequest request)
    {
        return "Invitation Sent to join NHS England Independent Provider Licensing Portal";
    }

    public string Create(PendingUserDeletedMessageRequest request)
    {
        return "Notification of deleted pending invitation";
    }

    public string Create(ReIssureInvitationToJoinPortalRequest request)
    {
        return "Re-issued Invitation to join NHS England Independent Provider Licensing Portal";
    }

    public string Create(UserDeletedMessageRequest request)
    {
        return "Notification of deleted user account";
    }

    public string Create(UpdateAnnualCertificateTaskStatusRequest request)
    {
        return "Annual certificate task status has been changed";
    }

    public string Create(UpdateFinancialMonitorngTaskStatusRequest request)
    {
        return "Financial monitoring task status has been changed";
    }
}
