using System.Text.RegularExpressions;
using Domain.Logic.Forms.Application;
using Domain.Logic.Forms.Application.Page;
using Domain.Logic.Forms.Helpers;
using Domain.Logic.Forms.Helpers.Session;
using Domain.Models.Database;
using Domain.Models.Exceptions;
using Licensing.Gateway.Models.Directors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace Licensing.Gateway.Controllers;

[Authorize]
public class DirectorController(
    ILogger<DirectorController> logger,
    IApplicationOrchestration applicationOrchestration,
    IResponseConverter responseConverter,
    INextPageOrchestor nextPageOrchestor,
    IDirectorOrchestration directorOrchestration,
    ISessionOrchestration sessionOrchestration,
    ISessionDateHandler sessionDateHandler,
    IPageToControllerMapper pageToControllerMapper) : BaseController(
        logger,
        sessionOrchestration,
        applicationOrchestration,
        directorOrchestration,
        pageToControllerMapper)
{
    private readonly ILogger<DirectorController> logger = logger;
    private readonly IApplicationOrchestration applicationOrchestration = applicationOrchestration;
    private readonly IResponseConverter responseConverter = responseConverter;
    private readonly INextPageOrchestor nextPageOrchestor = nextPageOrchestor;
    private readonly IDirectorOrchestration directorOrchestration = directorOrchestration;
    private readonly ISessionOrchestration sessionOrchestration = sessionOrchestration;
    private readonly ISessionDateHandler sessionDateHandler = sessionDateHandler;
    private readonly IPageToControllerMapper pageToControllerMapper = pageToControllerMapper;

    [Route("licence-conditions/directors-check")]
    public async Task<IActionResult> DirectorCheck()
    {
        var id = GetApplicationIdFromSession();

        if (id == 0)
        {
            return RedirectToAction(nameof(ApplicationControllerConstants.Controller_Application_Method_SessionTimeout), nameof(ApplicationControllerConstants.Controller_Name_Application));
        }

        return View(new DirectorCheck()
        {
            DirectorsCheck = await applicationOrchestration.GetDirectorsCheck(id),
            ProviderName = await applicationOrchestration.GetCQCProviderName(id),
            ValidationFailure = GetThenResetFormValidationErrorFromSession()
        });
    }

    [HttpPost]
    [Route("licence-conditions/directors-check")]
    public async Task<IActionResult> DirectorCheck(DirectorCheck model)
    {
        var id = GetApplicationIdFromSession();

        if (id == 0)
        {
            return RedirectToAction(nameof(ApplicationControllerConstants.Controller_Application_Method_SessionTimeout), nameof(ApplicationControllerConstants.Controller_Name_Application));
        }

        SetSessionFormValidationError(false);

        var response = model.DirectorsCheck?.Trim() ?? string.Empty;

        await applicationOrchestration.SetDirectorsCheck(id, response);

        if (string.IsNullOrWhiteSpace(model.DirectorsCheck))
        {
            SetSessionFormValidationError(true);

            return RedirectToAction();
        }

        var nextPage = await applicationOrchestration.OrchestrateNextPagePostDirectorCheck(id, response, GetReview());

        return RedirectToAction(nextPage.ToString(), pageToControllerMapper.Map(nextPage));
    }

    [Route("licence-conditions/directors")]
    public async Task<IActionResult> Directors()
    {
        var id = GetApplicationIdFromSession();

        if (id == 0)
        {
            return RedirectToAction(nameof(ApplicationControllerConstants.Controller_Application_Method_SessionTimeout), nameof(ApplicationControllerConstants.Controller_Name_Application));
        }

        return View(new DirectorsViewModel()
        {
            Directors = await directorOrchestration.GetDirectors(id, DirectorType.Board),
            ProviderName = await applicationOrchestration.GetCQCProviderName(id),
            ValidationFailure = GetThenResetFormValidationErrorFromSession()
        });
    }

    [HttpPost]
    [Route("licence-conditions/directors")]
    public async Task<IActionResult> Directors(DirectorsViewModel model)
    {
        var id = GetApplicationIdFromSession();

        if (id == 0)
        {
            return RedirectToAction(nameof(ApplicationControllerConstants.Controller_Application_Method_SessionTimeout), nameof(ApplicationControllerConstants.Controller_Name_Application));
        }

        SetSessionFormValidationError(false);

        var numberOfDirectors = await directorOrchestration.CountDirectorsOfGroupType(id, DirectorType.Board);

        if (numberOfDirectors == 0)
        {
            return RedirectToAction(nameof(DirectorCheck));
        }

        return await SetPageAndRedirectPostDirectors(id, Domain.Models.Database.ApplicationPage.CorporateDirectorCheck);
    }

    [HttpPost]
    [Route("licence-conditions/add-director")]
    public IActionResult DirectorAdd(DirectorsViewModel _)
    {
        var applicationID = GetApplicationIdFromSession();

        if (applicationID == 0)
        {
            return RedirectToAction(nameof(ApplicationControllerConstants.Controller_Application_Method_SessionTimeout), nameof(ApplicationControllerConstants.Controller_Name_Application));
        }

        ResetSesssionDirectorDatabaseId();
        ResetDirectorSessionVariables();

        /* WE ARE NOT SETTING THE PAGE HERE AS IT WOULD BE TRICKIER TO IMPLEMENT */
        return RedirectToAction(nameof(DirectorName));
    }

    [HttpPost]
    [Route("licence-conditions/director-edit")]
    public async Task<IActionResult> DirectorEdit(DirectorsEditViewModel model)
    {
        var applicationID = GetApplicationIdFromSession();

        if (applicationID == 0)
        {
            return RedirectToAction(nameof(ApplicationControllerConstants.Controller_Application_Method_SessionTimeout), nameof(ApplicationControllerConstants.Controller_Name_Application));
        }

        var exists = await directorOrchestration.DirectorExists(applicationID, model.Id);

        if (exists)
        {
            var directorDetails = await directorOrchestration.GetDirectorDetails(applicationID, model.Id);

            sessionOrchestration.Set(ApplicationFormConstants.SessionDirectorDatabaseId, model.Id);

            /* WE ARE NOT SETTING THE PAGE HERE AS IT WOULD BE TRICKIER TO IMPLEMENT */
            return RedirectToAction(nameof(DirectorName));
        }

        throw new NotFoundException($"Director with the id of {model.Id} doesn't exist for user {GetOktaUserId()}");
    }

    [HttpPost]
    [Route("licence-conditions/director-remove")]
    public async Task<IActionResult> DirectorRemove(DirectorsEditViewModel model)
    {
        var (applicationID, _) = GetDirectorIds();

        if (applicationID == 0)
        {
            return RedirectToAction(nameof(ApplicationControllerConstants.Controller_Application_Method_SessionTimeout), nameof(ApplicationControllerConstants.Controller_Name_Application));
        }

        var exists = await directorOrchestration.DirectorExists(applicationID, model.Id);

        if (exists)
        {
            sessionOrchestration.Set(ApplicationFormConstants.SessionDirectorDatabaseId, model.Id);

            /* WE ARE NOT SETTING THE PAGE HERE AS IT WOULD BE TRICKIER TO IMPLEMENT */
            return RedirectToAction(nameof(DirectorRemoveConfirm));
        }

        throw new NotFoundException($"Director with the id of {model.Id} doesn't exist for user {GetOktaUserId()}");
    }

    [Route("licence-conditions/director-remove-confirm")]
    public async Task<IActionResult> DirectorRemoveConfirm()
    {
        var (applicationID, directorID) = GetDirectorIds();

        if (applicationID == 0)
        {
            return RedirectToAction(nameof(ApplicationControllerConstants.Controller_Application_Method_SessionTimeout), nameof(ApplicationControllerConstants.Controller_Name_Application));
        }

        if(directorID == 0)
        {
            // If someone has deleted a director and then tried to go back from the directors page
            return RedirectToAction(nameof(Directors));
        }

        return View(new DirectorRemoveConfirmViewModel()
        {
            Director = await directorOrchestration.GetDirectorDetails(applicationID, directorID),
            ValidationFailure = GetThenResetFormValidationErrorFromSession()
        });
    }

    [HttpPost]
    [Route("licence-conditions/director-remove-confirm")]
    public async Task<IActionResult> DirectorRemoveConfirm(DirectorRemoveConfirmViewModel model)
    {
        var (applicationID, directorID) = GetDirectorIds();

        if (applicationID == 0 || directorID == 0)
        {
            return RedirectToAction(nameof(ApplicationControllerConstants.Controller_Application_Method_SessionTimeout), nameof(ApplicationControllerConstants.Controller_Name_Application));
        }

        SetSessionFormValidationError(false);

        if (string.IsNullOrWhiteSpace(model.Confirmation))
        {
            SetSessionFormValidationError(true);

            return RedirectToAction();
        }

        if (model.Confirmation == ApplicationFormConstants.Yes)
        {
            await directorOrchestration.DeleteDirector(applicationID, directorID);
            ResetSesssionDirectorDatabaseId();
        }

        var numberOfDirectors = await directorOrchestration.CountDirectorsOfGroupType(applicationID, DirectorType.Board);

        if (numberOfDirectors == 0)
        {
            return RedirectToAction(nameof(Directors));
        }

        return await RedirectToReviewOrPage(applicationID, Domain.Models.Database.ApplicationPage.Directors);
    }

    [Route("licence-conditions/add-director-name")]
    public async Task<IActionResult> DirectorName()
    {
        return await GetDirectorDetailsFromSessionOrDatabase();
    }

    [HttpPost]
    [Route("licence-conditions/add-director-name")]
    public async Task<IActionResult> DirectorName(DirectorNameViewModel model)
    {
        return await ProcessDirectorName(model, nameof(DirectorName), nameof(DirectorDateOfBirth));
    }

    [Route("licence-conditions/add-director-dob")]
    public async Task<IActionResult> DirectorDateOfBirth()
    {
        var applicationID = GetApplicationIdFromSession();

        if (applicationID == 0)
        {
            return RedirectToAction(nameof(ApplicationControllerConstants.Controller_Application_Method_SessionTimeout), nameof(ApplicationControllerConstants.Controller_Name_Application));
        }

        return await SetupDirectorDateOfBirth();
    }

    [HttpPost]
    [Route("licence-conditions/add-director-dob")]
    public async Task<IActionResult> DirectorDateOfBirth(DirectorDateOfBirthViewModel model)
    {
        var applicationID = GetApplicationIdFromSession();

        if (applicationID == 0)
        {
            return RedirectToAction(nameof(ApplicationControllerConstants.Controller_Application_Method_SessionTimeout), nameof(ApplicationControllerConstants.Controller_Name_Application));
        }

        var groupId = await directorOrchestration.AquireGroupForBoard(applicationID);

        return await ProcessDirectorDateOfBirth(
            model,
            nameof(DirectorDateOfBirth),
            nameof(Directors),
            groupId);
    }

    [Route("licence-conditions/directors-satisfy-g3-fit-and-proper")]
    public async Task<IActionResult> DirectorsSatisfyG3FitAndProperRequirements()
    {
        var id = GetApplicationIdFromSession();

        if (id == 0)
        {
            return RedirectToAction(nameof(ApplicationControllerConstants.Controller_Application_Method_SessionTimeout), nameof(ApplicationControllerConstants.Controller_Name_Application));
        }

        return View(new DirectorsSatisfyG3FitAndProperRequirementsViewModel()
        {
            IfNoWhy = await applicationOrchestration.GetDirectorsSatisfyG3FitAndProperRequirementsIfNoWhy(id),
            DirectorsFitAndProper = await applicationOrchestration.GetDirectorsSatisfyG3FitAndProperRequirements(id),
            Directors = await directorOrchestration.GetDirectors(id),
            ValidationFailure = GetThenResetFormValidationErrorFromSession()
        });
    }

    [HttpPost]
    [Route("licence-conditions/directors-satisfy-g3-fit-and-proper")]
    public async Task<IActionResult> DirectorsSatisfyG3FitAndProperRequirements(DirectorsSatisfyG3FitAndProperRequirementsViewModel model)
    {
        var id = GetApplicationIdFromSession();

        if (id == 0)
        {
            return RedirectToAction(nameof(ApplicationControllerConstants.Controller_Application_Method_SessionTimeout), nameof(ApplicationControllerConstants.Controller_Name_Application));
        }

        SetSessionFormValidationError(false);

        await applicationOrchestration.SetDirectorsSatisfyG3FitAndProperRequirements(id, model.DirectorsFitAndProper?.Trim() ?? string.Empty);

        await applicationOrchestration.SetDirectorsSatisfyG3FitAndProperRequirementsIfNoWhy(id, model.IfNoWhy ?? string.Empty);

        if (string.IsNullOrWhiteSpace(model.DirectorsFitAndProper) || model.DirectorsFitAndProper == ApplicationFormConstants.No && string.IsNullOrWhiteSpace(model.IfNoWhy))
        {
            SetSessionFormValidationError(true);

            return RedirectToAction();
        }

        if (model.DirectorsFitAndProper == ApplicationFormConstants.Yes)
        {
            /* reset value to nothing just in case an end user has set the value but has changed their mind */
            model.IfNoWhy = string.Empty;
        }

        var nextPage = await applicationOrchestration.DetermineNextPageAfterDirectorsSection(id);

        return await RedirectToReviewOrPage(id, nextPage);
    }

    [HttpPost]
    [Route("licence-conditions/directors-satisfy-g3-fit-and-proper/edit")]
    public async Task<IActionResult> DirectorsSatisfyG3FitAndProperRequirementsEdit(int directorId)
    {
        var id = GetApplicationIdFromSession();

        if (id == 0)
        {
            return RedirectToAction(nameof(ApplicationControllerConstants.Controller_Application_Method_SessionTimeout), nameof(ApplicationControllerConstants.Controller_Name_Application));
        }

        var exits = await directorOrchestration.DirectorExists(id, directorId);

        if (exits)
        {
            var details = await directorOrchestration.GetDirectorDetails(id, directorId);

            await SetupGroupSessionDetails(details.GroupId);

            sessionOrchestration.Set(ApplicationFormConstants.SessionDirectorDatabaseId, directorId);

            switch (details.DirectorType)
            {
                case DirectorType.Board:

                    return RedirectToAction(ApplicationControllerConstants.Controller_Director_Method_DirectorName, ApplicationControllerConstants.Controller_Name_Director);

                case DirectorType.Corporate:

                    return RedirectToAction(ApplicationControllerConstants.Controller_CoporateDirector_Method_CorporateDirectorName, ApplicationControllerConstants.Controller_Name_CoporateDirector);

                case DirectorType.ParentCompany:

                    return RedirectToAction(ApplicationControllerConstants.Controller_ParentCompanyDirector_Method_ParentCompanyDirectorName, ApplicationControllerConstants.Controller_Name_ParentCompanyDirector);

                default:
                    throw new NotImplementedException($"Director group id not caterred for. Director id: {directorId}, groupId: {details.GroupId}, type: {details.DirectorType}");
            }
        }

        return RedirectToAction(nameof(DirectorsSatisfyG3FitAndProperRequirements));
    }

    [HttpPost]
    [Route("licence-conditions/directors-satisfy-g3-fit-and-proper/remove")]
    public async Task<IActionResult> DirectorsSatisfyG3FitAndProperRequirementsRemove(int directorId)
    {
        var id = GetApplicationIdFromSession();

        if (id == 0)
        {
            return RedirectToAction(nameof(ApplicationControllerConstants.Controller_Application_Method_SessionTimeout), nameof(ApplicationControllerConstants.Controller_Name_Application));
        }

        var exits = await directorOrchestration.DirectorExists(id, directorId);

        if (exits)
        {
            var details = await directorOrchestration.GetDirectorDetails(id, directorId);

            await SetupGroupSessionDetails(details.GroupId);

            sessionOrchestration.Set(ApplicationFormConstants.SessionDirectorDatabaseId, directorId);

            switch (details.DirectorType)
            {
                case DirectorType.Board:

                    return RedirectToAction(ApplicationControllerConstants.Controller_Director_Method_DirectorRemoveConfirm, ApplicationControllerConstants.Controller_Name_Director);

                case DirectorType.Corporate:

                    return RedirectToAction(ApplicationControllerConstants.Controller_CoporateDirector_Method_CorporateDirectorRemoveConfirm, ApplicationControllerConstants.Controller_Name_CoporateDirector);

                case DirectorType.ParentCompany:

                    return RedirectToAction(ApplicationControllerConstants.Controller_CoporateDirector_Method_ParentCompanyDirectorRemoveConfirm, ApplicationControllerConstants.Controller_Name_ParentCompanyDirector);

                default:
                    throw new NotImplementedException($"Director group id not caterred for. Director id: {directorId}, groupId: {details.GroupId}, type: {details.DirectorType}");
            }
        }

        return RedirectToAction(nameof(DirectorsSatisfyG3FitAndProperRequirements));
    }
}
