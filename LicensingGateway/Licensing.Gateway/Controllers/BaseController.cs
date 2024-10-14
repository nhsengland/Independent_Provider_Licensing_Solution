using System.Security.Claims;
using Domain.Logic.Forms.Application;
using Domain.Logic.Forms.Application.Page;
using Domain.Logic.Forms.Helpers.Session;
using Domain.Models.Database;
using Domain.Models.Database.DTO;
using Domain.Models.Forms.Application;
using Licensing.Gateway.Models.Directors;
using Microsoft.AspNetCore.Mvc;

namespace Licensing.Gateway.Controllers;

public class BaseController
    (ILogger logger,
    ISessionOrchestration sessionOrchestration,
    IApplicationOrchestration applicationOrchestration,
    IDirectorOrchestration directorOrchestration,
    IPageToControllerMapper pageToControllerMapper) : Controller
{
    protected async Task<RedirectToActionResult> RedirectToReviewOrPage(
    int applicationId,
        Domain.Models.Database.ApplicationPage page)
    {
        var review = GetReview();

        if (review)
        {
            page = ApplicationPage.Review;
        }

        return await SetPageAndRedirectToAction(applicationId, page);
    }

    protected async Task<RedirectToActionResult> SetPageAndRedirectPostDirectors(
    int applicationId,
        Domain.Models.Database.ApplicationPage page)
    {
        var review = GetReview();

        if (review)
        {
            await applicationOrchestration.SetCurrentPage(applicationId, Domain.Models.Database.ApplicationPage.Review);

            return RedirectToAction(
                ApplicationControllerConstants.Controller_Director_Method_G3,
                ApplicationControllerConstants.Controller_Name_Director);
        }

        return await SetPageAndRedirectToAction(applicationId, page);
    }

    protected bool GetReview()
    {
        return sessionOrchestration.Get<bool>(ApplicationFormConstants.SessionApplicationReview);
    }

    protected async Task<RedirectToActionResult> SetPageAndRedirectToAction(
        int applicationId,
        Domain.Models.Database.ApplicationPage page)
    {
        await applicationOrchestration.SetCurrentPage(applicationId, page);

        return RedirectToAction(page.ToString(), pageToControllerMapper.Map(page));
    }

    protected RedirectToActionResult ReturnToStart()
    {
        logger.LogWarning("We have a submit attempt that hasn't got a valid application id");

        return RedirectToAction(
            ApplicationControllerConstants.Controller_Name_Application,
            ApplicationControllerConstants.Controller_Application_Method_ApplicationCode);
    }

    protected (int applicationID, int ultimateControllerId) GetUltimateControllerIds()
    {
        var applicationID = GetApplicationIdFromSession();

        var ultimateControllerId = sessionOrchestration.Get<int>(ApplicationFormConstants.SessionUltimateControllerId);

        return (applicationID, ultimateControllerId);
    }

    protected int GetApplicationIdFromSession()
    {
        return sessionOrchestration.Get<int>(ApplicationFormConstants.SessionApplicationDatabaseId);
    }

    protected bool GetThenResetFormValidationErrorFromSession()
    {
        var currentValue = sessionOrchestration.Get<bool>(ApplicationFormConstants.SessionFormValidationError);

        SetSessionFormValidationError(false);

        return currentValue;
    }

    protected void SetSessionFormValidationError(bool value)
    {
        sessionOrchestration.Set(ApplicationFormConstants.SessionFormValidationError, value);
    }

    protected (int applicationID, int directorID) GetDirectorIds()
    {
        var applicationID = GetApplicationIdFromSession();

        var directorID = sessionOrchestration.Get<int>(ApplicationFormConstants.SessionDirectorDatabaseId);

        return (applicationID, directorID);
    }

    protected int GetDirectorGroupIdFromSession()
    {
        return sessionOrchestration.Get<int>(ApplicationFormConstants.SessionDirectorGroupId);
    }

    protected DirectorNameDTO GetDirectorNamesFromSession()
    {
        return new DirectorNameDTO()
        {
            Forename = sessionOrchestration.Get<string>(ApplicationFormConstants.SessionDirectorForename) ?? string.Empty,
            Surname = sessionOrchestration.Get<string>(ApplicationFormConstants.SessionDirectorSurname) ?? string.Empty
        };
    }

    protected void ResetGroupSessionVariables()
    {
        sessionOrchestration.Set(ApplicationFormConstants.SessionDirectorGroupId, 0);
        sessionOrchestration.Set(ApplicationFormConstants.SessionDirectorGroupName, string.Empty);
    }

    protected void ResetSesssionDirectorDatabaseId()
    {
        sessionOrchestration.Set(ApplicationFormConstants.SessionDirectorDatabaseId, 0);
    }

    protected void ResetDirectorSessionVariables()
    {
        ResetSesssionDirectorDatabaseId();
        sessionOrchestration.Set(ApplicationFormConstants.SessionDirectorForename, string.Empty);
        sessionOrchestration.Set(ApplicationFormConstants.SessionDirectorSurname, string.Empty);
    }

    protected string GetDirectorGroupNameFromSession()
    {
        return sessionOrchestration.Get<string>(ApplicationFormConstants.SessionDirectorGroupName) ?? string.Empty;
    }

    protected async Task<IActionResult> GetDirectorDetailsFromSessionOrDatabase()
    {
        var (applicationID, directorID) = GetDirectorIds();

        if (applicationID == 0)
        {
            return RedirectToAction(nameof(ApplicationControllerConstants.Controller_Application_Method_ApplicationCode), nameof(ApplicationControllerConstants.Controller_Name_Application));
        }

        var directorName = GetDirectorNamesFromSession();

        if (directorID > 0)
        {
            directorName = await directorOrchestration.GetDirectorName(applicationID, directorID);
        }

        var model = new DirectorNameViewModel()
        {
            Forename = directorName.Forename,
            Surname = directorName.Surname,
            ValidationFailure = GetThenResetFormValidationErrorFromSession()
        };

        return View(model);
    }

    protected async Task<IActionResult> SetupDirectorDateOfBirth()
    {
        var (applicationID, directorID) = GetDirectorIds();

        if (applicationID == 0)
        {
            return RedirectToAction(nameof(ApplicationControllerConstants.Controller_Application_Method_ApplicationCode), nameof(ApplicationControllerConstants.Controller_Name_Application));
        }

        var dateDto = await directorOrchestration.GetDirectorDateOfBirthFromSessionOrDatabase(applicationID, directorID);

        var model = new DirectorDateOfBirthViewModel()
        {
            Day = dateDto.Day,
            Month = dateDto.Month,
            Year = dateDto.Year,
            ValidationFailure = GetThenResetFormValidationErrorFromSession()
        };

        if (model.ValidationFailure)
        {
            var outcome = directorOrchestration.EvaluateDirectorDateOfBirth(
                dateDto.Day,
                dateDto.Month,
                dateDto.Year);

            model.FailureMessages = outcome.ErrorMessages;
            model.IsValidDate = dateDto.IsValidDate;
        }

        return View(model);
    }

    protected async Task<IActionResult> ProcessDirectorDateOfBirth(
        DirectorDateOfBirthViewModel model,
        string actionNameToDirectToOnValidatioFailure,
        string actionNameToDirectToOnValidatioSuccess,
        int groupId)
    {
        var (applicationID, directorID) = GetDirectorIds();

        if (applicationID == 0)
        {
            return RedirectToAction(nameof(ApplicationControllerConstants.Controller_Application_Method_ApplicationCode), nameof(ApplicationControllerConstants.Controller_Name_Application));
        }

        SetSessionFormValidationError(false);

        var outcome = await directorOrchestration.OrchestratePersistanceOfDirectorDetails(
            applicationID,
            directorID,
            GetDirectorNamesFromSession(),
            new ApplicationDateDTO() { Day = model.Day, Month = model.Month, Year = model.Year },
            groupId);

        if (outcome.IsSuccess)
        {
            ResetDirectorSessionVariables();
            ResetSesssionDirectorDatabaseId();

            return RedirectToAction(actionNameToDirectToOnValidatioSuccess);
        }

        SetSessionFormValidationError(true);

        return RedirectToAction(actionNameToDirectToOnValidatioFailure);
    }

    protected string GetOktaUserId()
    {
        return User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
    }

    protected void SetupDirectorGroupInSession(int groupId, string groupName)
    {
        sessionOrchestration.Set(ApplicationFormConstants.SessionDirectorGroupName, groupName);
        sessionOrchestration.Set(ApplicationFormConstants.SessionDirectorGroupId, groupId);
    }

    protected async Task<bool> SetupGroupSessionDetails(int groupId)
    {
        if(groupId == 0)
        {
            // Its possible that a redirect back to the page when a submission has failed validation checks, we need to get grop id from session
            groupId = GetDirectorGroupIdFromSession();
        }

        var belongs = await GroupBelongsToApplication(groupId);

        if (belongs)
        {
            var groupName = await directorOrchestration.GetGroupName(groupId);

            SetupDirectorGroupInSession(groupId, groupName);
        }
        else
        {
            logger.LogWarning("Group {groupId} does not belong: userId: {oktaId}", groupId, GetOktaUserId());
        }

        return belongs;
    }

    protected async Task<bool> GroupBelongsToApplication(int groupId)
    {
        var id = GetApplicationIdFromSession();

        if (id == 0)
        {
            logger.LogWarning("Application {id}: userId: {oktaId}", id, GetOktaUserId());
            return false;
        }

        return await directorOrchestration.GroupBelongsToApplication(id, groupId);
    }

    protected async Task AquireDirectorGroup(int applicationId, int groupId, string groupName, DirectorType directorType)
    {
        if (groupId == 0)
        {
            groupId = await directorOrchestration.CreateGroup(applicationId, groupName, directorType);
        }
        else
        {
            await directorOrchestration.SetGroupName(groupId, groupName);
        }

        SetupDirectorGroupInSession(groupId, groupName);
    }

    protected async Task<IActionResult> ProcessDirectorName(
        DirectorNameViewModel model,
        string actionNameToDirectToOnValidatioFailure,
        string actionNameToDirectToOnValidationSuccess)
    {
        var (applicationID, directorID) = GetDirectorIds();

        if (applicationID == 0)
        {
            return RedirectToAction(nameof(ApplicationControllerConstants.Controller_Application_Method_ApplicationCode), nameof(ApplicationControllerConstants.Controller_Name_Application));
        }

        SetSessionFormValidationError(false);

        if (directorID == 0)
        {
            sessionOrchestration.Set(ApplicationFormConstants.SessionDirectorForename, model.Forename?.Trim());
            sessionOrchestration.Set(ApplicationFormConstants.SessionDirectorSurname, model.Surname?.Trim());
        }
        else
        {
            await directorOrchestration.SetDirectorName(applicationID, directorID, model.Forename?.Trim() ?? string.Empty, model.Surname?.Trim() ?? string.Empty);
        }

        if (string.IsNullOrWhiteSpace(model.Forename) || string.IsNullOrWhiteSpace(model.Surname))
        {
            SetSessionFormValidationError(true);

            return RedirectToAction(actionNameToDirectToOnValidatioFailure);
        }

        return RedirectToAction(actionNameToDirectToOnValidationSuccess);
    }
}
