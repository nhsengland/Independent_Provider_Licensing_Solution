using Domain.Logic.Features.Messages.Requests;
using Domain.Objects;
using System.Text;

namespace Domain.Logic.Features.Messages.Factories;

public class MessageBodyFactory : IMessageBodyFactory
{
    public string Create(ChangeRequestForCompanyAddress request)
    {
        var stringBuilder = new StringBuilder();

        stringBuilder.Append("This is to inform you that the registered address for ");
        stringBuilder.Append(request.CompanyName);
        stringBuilder.Append("/");
        stringBuilder.Append(request.LicenseNumber);
        stringBuilder.Append(" on the NHS England Independent Provider Licensing Portal has been changed.");
        stringBuilder.AppendLine();
        stringBuilder.AppendLine();
        stringBuilder.Append("Details of the address change:");
        stringBuilder.AppendLine();
        stringBuilder.Append("Changed by: ");
        stringBuilder.Append(request.RequestorName);
        stringBuilder.AppendLine();
        stringBuilder.Append("Previous Address: ");
        stringBuilder.Append(request.PreviousAddress);
        stringBuilder.AppendLine();
        stringBuilder.Append("New Address: ");
        stringBuilder.Append(request.NewAddress);
        stringBuilder.AppendLine();
        stringBuilder.Append("Request submitted: ");
        stringBuilder.Append(request.RequestedOn.ToString("dd/MM/yyyy"));
        stringBuilder.AppendLine();
        stringBuilder.AppendLine();
        stringBuilder.Append("If you have any questions or concerns regarding this change, please send us a message through the portal.");
        stringBuilder.AppendLine();
        stringBuilder.AppendLine();
        stringBuilder.Append("Kind regards,");
        stringBuilder.AppendLine();
        stringBuilder.Append(NotificationConstants.From);

        return stringBuilder.ToString();
    }

    public string Create(ChangeRequestForCompanyName request)
    {
        var stringBuilder = new StringBuilder();

        stringBuilder.Append("This is to inform you that the company name for ");
        stringBuilder.Append(request.PreviousCompanyName);
        stringBuilder.Append("/ Licence number: ");
        stringBuilder.Append(request.LicenseNumber);
        stringBuilder.Append(" on the NHS England Independent Provider Licensing Portal has been requested.");
        stringBuilder.AppendLine();
        stringBuilder.AppendLine();
        stringBuilder.Append("Details of the change:");
        stringBuilder.AppendLine();
        stringBuilder.Append("Changed by: ");
        stringBuilder.Append(request.RequestorName);
        stringBuilder.AppendLine();
        stringBuilder.Append("Previous Company Name: ");
        stringBuilder.Append(request.PreviousCompanyName);
        stringBuilder.AppendLine();
        stringBuilder.Append("New Company Name: ");
        stringBuilder.Append(request.NewCompanyName);
        stringBuilder.AppendLine();
        stringBuilder.Append("Request submitted: ");
        stringBuilder.Append(request.RequestedOn.ToString("dd/MM/yyyy"));
        stringBuilder.AppendLine();
        stringBuilder.AppendLine();
        stringBuilder.Append("If you have any questions or concerns regarding this change, please send us a message through the portal.");
        stringBuilder.AppendLine();
        stringBuilder.AppendLine();
        stringBuilder.Append("Kind regards,");
        stringBuilder.AppendLine();
        stringBuilder.Append(NotificationConstants.From);

        return stringBuilder.ToString();
    }

    public string Create(ChangeRequestForFYE request)
    {
        var stringBuilder = new StringBuilder();

        stringBuilder.Append("This is to inform you that the financial year-end date for ");
        stringBuilder.Append(request.CompanyName);
        stringBuilder.Append("/");
        stringBuilder.Append(request.LicenseNumber);
        stringBuilder.Append(" on the NHS England Independent Provider Licensing Portal has been changed.");
        stringBuilder.AppendLine();
        stringBuilder.AppendLine();
        stringBuilder.Append("Details of the change:");
        stringBuilder.AppendLine();
        stringBuilder.Append("Changed by: ");
        stringBuilder.Append(request.RequestorName);
        stringBuilder.AppendLine();
        stringBuilder.Append("Previous Financial Year-End: ");
        stringBuilder.Append(request.PreviousFinancialYearEnd.ToString("dd/MM/yyyy"));
        stringBuilder.AppendLine();
        stringBuilder.Append("New Financial Year-End: ");
        stringBuilder.Append(request.NewFinancialYearEnd.ToString("dd/MM/yyyy"));
        stringBuilder.AppendLine();
        stringBuilder.Append("Request submitted: ");
        stringBuilder.Append(request.RequestedOn.ToString("dd/MM/yyyy"));
        stringBuilder.AppendLine();
        stringBuilder.AppendLine();
        stringBuilder.Append("If you have any questions or concerns regarding this change, please send us a message through the portal.");
        stringBuilder.AppendLine();
        stringBuilder.AppendLine();
        stringBuilder.Append("Kind regards,");
        stringBuilder.AppendLine();
        stringBuilder.Append(NotificationConstants.From);


        return stringBuilder.ToString();
    }

    public string Create(InvitationToJoinPortalRequest request)
    {
        var stringBuilder = new StringBuilder();

        stringBuilder.Append("This is to notify you that a new invitation to join the NHS England Independent Provider Licensing Portal has been sent.");
        stringBuilder.AppendLine();
        stringBuilder.AppendLine();
        stringBuilder.Append("Details of the invitation:");
        stringBuilder.AppendLine();
        stringBuilder.Append("Invited by: ");
        stringBuilder.Append(request.RequestorName);
        stringBuilder.AppendLine();
        stringBuilder.Append("Invited Person's Name: ");
        stringBuilder.Append(request.InviteeName);
        stringBuilder.AppendLine();
        stringBuilder.Append("Invitee's Email: ");
        stringBuilder.Append(request.InviteeEmail);
        stringBuilder.AppendLine();
        stringBuilder.Append("Date of Invitation: ");
        stringBuilder.Append(request.RequestedOn.ToString("dd/MM/yyyy"));
        stringBuilder.AppendLine();
        stringBuilder.AppendLine();
        stringBuilder.Append("If there are any questions or concerns regarding this invitation, please send us a message through the portal.");
        stringBuilder.AppendLine();
        stringBuilder.AppendLine();
        stringBuilder.Append("Best regards,");
        stringBuilder.AppendLine();
        stringBuilder.Append(NotificationConstants.From);

        return stringBuilder.ToString();
    }

    public string Create(PendingUserDeletedMessageRequest request)
    {
        var stringBuilder = new StringBuilder();

        stringBuilder.Append("This is to inform you that a pending invitation to join the NHS England Independent Provider Licensing Portal has been deleted.");
        stringBuilder.AppendLine();
        stringBuilder.AppendLine();
        stringBuilder.Append("Details of the deleted invitation:");
        stringBuilder.AppendLine();
        stringBuilder.Append("Deleted by: ");
        stringBuilder.Append(request.RequestorName);
        stringBuilder.AppendLine();
        stringBuilder.Append("Invitee's Name: ");
        stringBuilder.Append(request.InviteeName);
        stringBuilder.AppendLine();
        stringBuilder.Append("Invitee's Email: ");
        stringBuilder.Append(request.InviteeEmail);
        stringBuilder.AppendLine();
        stringBuilder.Append("Date of Deletion: ");
        stringBuilder.Append(request.RequestedOn.ToString("dd/MM/yyyy"));
        stringBuilder.AppendLine();
        stringBuilder.AppendLine();
        stringBuilder.Append("If you have any questions or concerns regarding this action, please send us a message through the portal.");
        stringBuilder.AppendLine();
        stringBuilder.AppendLine();
        stringBuilder.Append("Best regards,");
        stringBuilder.AppendLine();
        stringBuilder.Append(NotificationConstants.From);

        return stringBuilder.ToString();
    }

    public string Create(ReIssureInvitationToJoinPortalRequest request)
    {
        var stringBuilder = new StringBuilder();

        stringBuilder.Append("This is to notify you that an invitation to join the NHS England Independent Provider Licensing Portal has been re-issued to the same invitee.");
        stringBuilder.AppendLine();
        stringBuilder.AppendLine();
        stringBuilder.Append("Details of the re-issued invitation:");
        stringBuilder.AppendLine();
        stringBuilder.Append("Re-issued by: ");
        stringBuilder.Append(request.RequestorName);
        stringBuilder.AppendLine();
        stringBuilder.Append("Invitee's Name: ");
        stringBuilder.Append(request.InviteeName);
        stringBuilder.AppendLine();
        stringBuilder.Append("Invitee's Email: ");
        stringBuilder.Append(request.InviteeEmail);
        stringBuilder.AppendLine();
        stringBuilder.Append("Date of Re-issue: ");
        stringBuilder.Append(request.RequestedOn.ToString("dd/MM/yyyy"));
        stringBuilder.AppendLine();
        stringBuilder.AppendLine();
        stringBuilder.Append("If there are any questions or concerns regarding this invitation, please send us a message through the portal.");
        stringBuilder.AppendLine();
        stringBuilder.AppendLine();
        stringBuilder.Append("Best regards,");
        stringBuilder.AppendLine();
        stringBuilder.Append(NotificationConstants.From);

        return stringBuilder.ToString();
    }

    public string Create(UserDeletedMessageRequest request)
    {
        var stringBuilder = new StringBuilder();

        stringBuilder.Append("This is to inform you that an existing user account has been deleted from the NHS England Independent Provider Licensing Portal.");
        stringBuilder.AppendLine();
        stringBuilder.AppendLine();
        stringBuilder.Append("Details of the deleted account:");
        stringBuilder.AppendLine();
        stringBuilder.Append("Deleted by: ");
        stringBuilder.Append(request.RequestorName);
        stringBuilder.AppendLine();
        stringBuilder.Append("User's Name: ");
        stringBuilder.Append(request.InviteeName);
        stringBuilder.AppendLine();
        stringBuilder.Append("User's Email: ");
        stringBuilder.Append(request.InviteeEmail);
        stringBuilder.AppendLine();
        stringBuilder.Append("Date of Deletion: ");
        stringBuilder.Append(request.RequestedOn.ToString("dd/MM/yyyy"));
        stringBuilder.AppendLine();
        stringBuilder.AppendLine();
        stringBuilder.Append("If you have any questions or concerns regarding this action, please send us a message through the portal.");
        stringBuilder.AppendLine();
        stringBuilder.AppendLine();
        stringBuilder.Append("Best regards,");
        stringBuilder.AppendLine();
        stringBuilder.Append(NotificationConstants.From);

        return stringBuilder.ToString();
    }
    public string Create(UpdateAnnualCertificateTaskStatusRequest request)
    {
        var stringBuilder = new StringBuilder();

        stringBuilder.Append("This is to confirm that a request to change the annual certificate task status for the licence ");
        stringBuilder.Append(request.LicenceName);
        stringBuilder.Append(" has been made via the NHS England Independent Provider Licensing Portal.");
        stringBuilder.AppendLine();
        stringBuilder.AppendLine();
        stringBuilder.Append("Details of the request:");
        stringBuilder.AppendLine();
        stringBuilder.Append("Requested by: ");
        stringBuilder.Append(request.RequestorName);
        stringBuilder.AppendLine();
        stringBuilder.Append("Date of Request: ");
        stringBuilder.Append(request.RequestedOn.ToString("dd/MM/yyyy"));
        stringBuilder.AppendLine();
        stringBuilder.AppendLine();
        stringBuilder.Append("If there are any questions or concerns regarding this invitation, please send us a message through the portal.");
        stringBuilder.AppendLine();
        stringBuilder.AppendLine();
        stringBuilder.Append("Best regards,");
        stringBuilder.AppendLine();
        stringBuilder.Append(NotificationConstants.From);

        return stringBuilder.ToString();
    }

    public string Create(UpdateFinancialMonitorngTaskStatusRequest request)
    {
        var stringBuilder = new StringBuilder();

        stringBuilder.Append("This is to confirm that a request to change the financial monitoring task status was made via the NHS England Independent Provider Licensing Portal.");
        stringBuilder.AppendLine();
        stringBuilder.AppendLine();
        stringBuilder.Append("Details of the request:");
        stringBuilder.AppendLine();
        stringBuilder.Append("Requested by: ");
        stringBuilder.Append(request.RequestorName);
        stringBuilder.AppendLine();
        stringBuilder.Append("Date of Request: ");
        stringBuilder.Append(request.RequestedOn.ToString("dd/MM/yyyy"));
        stringBuilder.AppendLine();
        stringBuilder.AppendLine();
        stringBuilder.Append("If there are any questions or concerns regarding this invitation, please send us a message through the portal.");
        stringBuilder.AppendLine();
        stringBuilder.AppendLine();
        stringBuilder.Append("Best regards,");
        stringBuilder.AppendLine();
        stringBuilder.Append(NotificationConstants.From);

        return stringBuilder.ToString();
    }
}
