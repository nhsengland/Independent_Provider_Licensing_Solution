using Domain.Logic.Integrations.Session;
using Domain.Objects;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Licence.Holder.Application.Controllers;

public class BaseController(
    ISessionOrchestration sessionOrchestration) : Controller
{
    private readonly ISessionOrchestration sessionOrchestration = sessionOrchestration;

    protected string GetOktaUserId()
    {
        return User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
    }

    protected bool GetFromSession_FormValidationFailure_ThenReset()
    {
        var currentValue = sessionOrchestration.Get<bool>(PageConstants.Session_FormValidationFailure);
        
        Set_Session_FormValidationFailure(false);

        return currentValue;
    }

    protected void Set_Session_FormValidationFailure(bool value)
    {
        sessionOrchestration.Set(PageConstants.Session_FormValidationFailure, value);
    }

    protected void Remove_Session_FormValidationFailure()
    {
        sessionOrchestration.Remove(PageConstants.Session_FormValidationFailure);
    }

    protected (int taskId, int organisationOrlicenseId) Get_Session_UpdateSubmissionStatus()
    {
        return (sessionOrchestration.Get<int>(PageConstants.Session_UpdateSubmissionStatus_TaskId), sessionOrchestration.Get<int>(PageConstants.Session_UpdateSubmissionStatus_LicenceId));
    }

    protected void Set_Session_UpdateSubmissionStatus(int taskId, int organisationOrlicenseId)
    {
        Set_Session_FormValidationFailure(true);
        sessionOrchestration.Set(PageConstants.Session_UpdateSubmissionStatus_TaskId, taskId);
        sessionOrchestration.Set(PageConstants.Session_UpdateSubmissionStatus_LicenceId, organisationOrlicenseId);
    }

    protected void Remove_Session_UpdateSubmissionStatus()
    {
        Remove_Session_FormValidationFailure();
        sessionOrchestration.Remove(PageConstants.Session_UpdateSubmissionStatus_TaskId);
        sessionOrchestration.Remove(PageConstants.Session_UpdateSubmissionStatus_LicenceId);
    }

    protected (string line1, string line2, string townOrCiy, string county, string postcode) Get_Session_Address()
    {
        return (sessionOrchestration.Get<string>(PageConstants.Session_Company_Address_Line_1) ?? string.Empty, sessionOrchestration.Get<string>(PageConstants.Session_Company_Address_Line_2) ?? string.Empty, sessionOrchestration.Get<string>(PageConstants.Session_Company_Address_TownOrCity) ?? string.Empty, sessionOrchestration.Get<string>(PageConstants.Session_Company_Address_County) ?? string.Empty, sessionOrchestration.Get<string>(PageConstants.Session_Company_Address_Postcode) ?? string.Empty);
    }

    protected void Set_Session_Address(
        string addressLine1,
        string addressLine2,
        string townOrCity,
        string county,
        string postcode)
    {
        sessionOrchestration.Set(PageConstants.Session_Company_Address_Line_1, addressLine1);
        sessionOrchestration.Set(PageConstants.Session_Company_Address_Line_2, addressLine2);
        sessionOrchestration.Set(PageConstants.Session_Company_Address_TownOrCity, townOrCity);
        sessionOrchestration.Set(PageConstants.Session_Company_Address_County, county);
        sessionOrchestration.Set(PageConstants.Session_Company_Address_Postcode, postcode);
    }

    protected void Remove_Session_Address()
    {
        Remove_Session_FormValidationFailure();
        sessionOrchestration.Remove(PageConstants.Session_Company_Address_Line_1);
        sessionOrchestration.Remove(PageConstants.Session_Company_Address_Line_2);
        sessionOrchestration.Remove(PageConstants.Session_Company_Address_TownOrCity);
        sessionOrchestration.Remove(PageConstants.Session_Company_Address_County);
        sessionOrchestration.Remove(PageConstants.Session_Company_Address_Postcode);
    }

    protected void Set_Session_FeedbackType(Domain.Objects.Database.FeedbackType feedbackType)
    {
        sessionOrchestration.Set(FeedbackFormConstants.SessionFeedbackTypeId, feedbackType);
    }
}
