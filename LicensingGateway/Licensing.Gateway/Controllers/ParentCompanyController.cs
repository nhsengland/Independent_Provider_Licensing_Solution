using Domain.Logic.Forms.Application;
using Domain.Logic.Forms.Application.Page;
using Domain.Logic.Forms.Helpers;
using Domain.Logic.Forms.Helpers.Session;
using Domain.Models.Database;
using Domain.Models.Exceptions;
using Licensing.Gateway.Models.Directors;
using Licensing.Gateway.Models.Directors.Corporate;
using Licensing.Gateway.Models.Directors.ParentCompanies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Licensing.Gateway.Controllers;

[Authorize]
public class ParentCompanyController(
    ILogger<ParentCompanyController> logger,
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
    private readonly ILogger<ParentCompanyController> logger = logger;
    private readonly IApplicationOrchestration applicationOrchestration = applicationOrchestration;
    private readonly IResponseConverter responseConverter = responseConverter;
    private readonly INextPageOrchestor nextPageOrchestor = nextPageOrchestor;
    private readonly IDirectorOrchestration directorOrchestration = directorOrchestration;
    private readonly ISessionOrchestration sessionOrchestration = sessionOrchestration;
    private readonly ISessionDateHandler sessionDateHandler = sessionDateHandler;

    [Route("licence-conditions/parent-companies-check")]
    public async Task<IActionResult> ParentCompaniesCheck()
    {
        var id = GetApplicationIdFromSession();

        if (id == 0)
        {
            return RedirectToAction(nameof(ApplicationControllerConstants.Controller_Application_Method_SessionTimeout), nameof(ApplicationControllerConstants.Controller_Name_Application));
        }

        return View(new ParentCompaniesCheckViewModel()
        {
            Check = await applicationOrchestration.GetOneOrMoreParentCompanies(id),
            ProviderName = await applicationOrchestration.GetCQCProviderName(id),
            ValidationFailure = GetThenResetFormValidationErrorFromSession()
        });
    }

    [HttpPost]
    [Route("licence-conditions/parent-companies-check")]
    public async Task<IActionResult> ParentCompaniesCheck(ParentCompaniesCheckViewModel model)
    {
        var id = GetApplicationIdFromSession();

        if (id == 0)
        {
            return RedirectToAction(nameof(ApplicationControllerConstants.Controller_Application_Method_SessionTimeout), nameof(ApplicationControllerConstants.Controller_Name_Application));
        }

        SetSessionFormValidationError(false);

        await applicationOrchestration.SetOneOrMoreParentCompanies(id, model.Check?.Trim() ?? string.Empty);

        if (string.IsNullOrWhiteSpace(model.Check) || (model.Check != ApplicationFormConstants.Yes && model.Check != ApplicationFormConstants.No))
        {
            SetSessionFormValidationError(true);

            return RedirectToAction();
        }

        var whereNext = applicationOrchestration.DetermineNextPageAfterParentCompaniesCheck(model.Check, GetReview());

        return await SetPageAndRedirectToAction(id, whereNext);
    }

    [Route("licence-conditions/parent-companies")]
    public async Task<IActionResult> ParentCompanies()
    {
        var id = GetApplicationIdFromSession();

        if (id == 0)
        {
            return RedirectToAction(nameof(ApplicationControllerConstants.Controller_Application_Method_SessionTimeout), nameof(ApplicationControllerConstants.Controller_Name_Application));
        }

        return View(new ParentCompaniesViewModel()
        {
            ProviderName = await applicationOrchestration.GetCQCProviderName(id),
            Groups = await directorOrchestration.GetGroups(id, DirectorType.ParentCompany)
        });
    }

    [HttpPost]
    [Route("licence-conditions/parent-companies")]
    public async Task<IActionResult> ParentCompanies(ParentCompaniesViewModel model)
    {
        var id = GetApplicationIdFromSession();

        if (id == 0)
        {
            return RedirectToAction(nameof(ApplicationControllerConstants.Controller_Application_Method_SessionTimeout), nameof(ApplicationControllerConstants.Controller_Name_Application));
        }

        var numberOfGroups = await directorOrchestration.CountGroups(id, DirectorType.ParentCompany);

        if (numberOfGroups == 0)
        {
            return await SetPageAndRedirectPostDirectors(id, ApplicationPage.ParentCompaniesCheck);
        }

        return await SetPageAndRedirectPostDirectors(id, ApplicationPage.DirectorsSatisfyG3FitAndProperRequirements);
    }

    [Route("licence-conditions/parent-company-name")]
    public async Task<IActionResult> ParentCompanyName()
    {
        var id = GetApplicationIdFromSession();

        if (id == 0)
        {
            return RedirectToAction(nameof(ApplicationControllerConstants.Controller_Application_Method_SessionTimeout), nameof(ApplicationControllerConstants.Controller_Name_Application));
        }

        return View(new ParentCompanyNameViewModel()
        {
            ParentCompanyName = GetDirectorGroupNameFromSession(),
            ProviderName = await applicationOrchestration.GetCQCProviderName(id),
            ValidationFailure = GetThenResetFormValidationErrorFromSession()
        });
    }

    [HttpPost]
    [Route("licence-conditions/parent-company-name")]
    public async Task<IActionResult> ParentCompanyName(ParentCompanyNameViewModel model)
    {
        var id = GetApplicationIdFromSession();

        if (id == 0)
        {
            return RedirectToAction(nameof(ApplicationControllerConstants.Controller_Application_Method_SessionTimeout), nameof(ApplicationControllerConstants.Controller_Name_Application));
        }

        SetSessionFormValidationError(false);

        await AquireDirectorGroup(id, GetDirectorGroupIdFromSession(), model.ParentCompanyName?.Trim() ?? string.Empty, DirectorType.ParentCompany);

        if (string.IsNullOrWhiteSpace(model.ParentCompanyName))
        {
            SetSessionFormValidationError(true);

            return RedirectToAction();
        }

        return await SetPageAndRedirectToAction(id, ApplicationPage.ParentCompanyDirectorsOrEquivalents);
    }

    [Route("licence-conditions/parent-company-name-add")]
    public IActionResult ParentCompanyNameAdd(int groupId)
    {
        ResetGroupSessionVariables();

        return RedirectToAction(nameof(ParentCompanyName));
    }

    [Route("licence-conditions/parent-company-name-edit")]
    public async Task<IActionResult> ParentCompanyNameEdit(int groupId)
    {
        var result = await SetupGroupSessionDetails(groupId);

        return result == true ? RedirectToAction(nameof(ParentCompanyName)) : ReturnToStart();
    }

    [Route("licence-conditions/delete-parent-company")]
    public async Task<IActionResult> ParentCompanyDelete(int groupId)
    {
        var result = await SetupGroupSessionDetails(groupId);

        if (result == false)
        {
            throw new NotFoundException($"Parent company with the id of {groupId} doesn't exist for user {GetOktaUserId()}");
        }

        return View(new ParentCompanyDeleteViewModel()
        {
            GroupName = GetDirectorGroupNameFromSession(),
            ValidationFailure = GetThenResetFormValidationErrorFromSession()
        });
    }

    [HttpPost]
    [Route("licence-conditions/delete-parent-company")]
    public async Task<IActionResult> ParentCompanyDelete(ParentCompanyDeleteViewModel model)
    {
        var id = GetApplicationIdFromSession();

        if (id == 0)
        {
            return RedirectToAction(nameof(ApplicationControllerConstants.Controller_Application_Method_SessionTimeout), nameof(ApplicationControllerConstants.Controller_Name_Application));
        }

        SetSessionFormValidationError(false);

        if (string.IsNullOrWhiteSpace(model.Answer))
        {
            SetSessionFormValidationError(true);

            return RedirectToAction();
        }

        if (model.Answer == ApplicationFormConstants.Yes)
        {
            await directorOrchestration.DeleteGroup(id, GetDirectorGroupIdFromSession());
        }

        return await SetPageAndRedirectToAction(id, ApplicationPage.ParentCompanies);
    }

    [Route("licence-conditions/parent-company-directors-or-equivalents")]
    public async Task<IActionResult> ParentCompanyDirectorsOrEquivalents()
    {
        var id = GetApplicationIdFromSession();

        if (id == 0)
        {
            return RedirectToAction(nameof(ApplicationControllerConstants.Controller_Application_Method_SessionTimeout), nameof(ApplicationControllerConstants.Controller_Name_Application));
        }

        var model = new ParentCompanyDirectorsOrEquivalentsViewModel()
        {
            ParentCompanyName = GetDirectorGroupNameFromSession(),
            ValidationFailure = GetThenResetFormValidationErrorFromSession()
        };

        var previousResponse = await directorOrchestration.GetGroupOneOrMoreIndividuals(GetDirectorGroupIdFromSession());

        if (previousResponse != null)
        {
            model.Answer = responseConverter.ConvertToYesOrNo(previousResponse);
        }

        return View(model);
    }

    [HttpPost]
    [Route("licence-conditions/parent-company-directors-or-equivalents")]
    public async Task<IActionResult> ParentCompanyDirectorsOrEquivalents(ParentCompanyDirectorsOrEquivalentsViewModel model)
    {
        var id = GetApplicationIdFromSession();

        if (id == 0)
        {
            return RedirectToAction(nameof(ApplicationControllerConstants.Controller_Application_Method_SessionTimeout), nameof(ApplicationControllerConstants.Controller_Name_Application));
        }

        SetSessionFormValidationError(false);

        await directorOrchestration.SetGroupOneOrMoreIndividuals(GetDirectorGroupIdFromSession(), responseConverter.Convert(model.Answer));

        if (string.IsNullOrWhiteSpace(model.Answer))
        {
            SetSessionFormValidationError(true);

            return RedirectToAction();
        }

        var nextPage = model.Answer == ApplicationFormConstants.No ? ApplicationPage.ParentCompanies : ApplicationPage.ParentCompanyDirectors;

        return await SetPageAndRedirectToAction(id, nextPage);
    }

    [Route("licence-conditions/parent-company-directors-or-equivalents-edit")]
    public async Task<IActionResult> ParentCompanyDirectorsOrEquivalentsEdit(int groupId)
    {
        var result = await SetupGroupSessionDetails(groupId);

        return result == true ? RedirectToAction(nameof(ParentCompanyDirectorsOrEquivalents)) : ReturnToStart();
    }

    [Route("licence-conditions/parent-company-directors")]
    public async Task<IActionResult> ParentCompanyDirectors()
    {
        var id = GetApplicationIdFromSession();

        if (id == 0)
        {
            return RedirectToAction(nameof(ApplicationControllerConstants.Controller_Application_Method_SessionTimeout), nameof(ApplicationControllerConstants.Controller_Name_Application));
        }

        return View(new DirectorsViewModel()
        {
            GroupName = GetDirectorGroupNameFromSession(),
            Directors = await directorOrchestration.GetDirectors(id, DirectorType.ParentCompany, GetDirectorGroupIdFromSession()),
            ProviderName = await applicationOrchestration.GetCQCProviderName(id),
        });
    }

    [HttpPost]
    [Route("licence-conditions/parent-company-directors")]
    public async Task<IActionResult> ParentCompanyDirectors(DirectorsViewModel _)
    {
        var id = GetApplicationIdFromSession();

        if (id == 0)
        {
            return RedirectToAction(nameof(ApplicationControllerConstants.Controller_Application_Method_SessionTimeout), nameof(ApplicationControllerConstants.Controller_Name_Application));
        }

        var numberOfDirectors = await directorOrchestration.CountDirectorsInGroup(GetDirectorGroupIdFromSession());

        if (numberOfDirectors == 0)
        {
            return await SetPageAndRedirectPostDirectors(id, ApplicationPage.ParentCompanyDirectorsOrEquivalents);
        }

        return await SetPageAndRedirectToAction(id, Domain.Models.Database.ApplicationPage.ParentCompanies);
    }

    [Route("licence-conditions/parent-company-directors-edit")]
    public async Task<IActionResult> ParentCompanyDirectorsEdit(int groupId)
    {
        var result = await SetupGroupSessionDetails(groupId);

        return result == true ? RedirectToAction(nameof(ParentCompanyDirectors)) : ReturnToStart();
    }

    [HttpPost]
    [Route("licence-conditions/add-parent-company-director")]
    public IActionResult ParentCompanyDirectorAdd(DirectorsViewModel _)
    {
        var applicationID = GetApplicationIdFromSession();

        if (applicationID == 0)
        {
            return RedirectToAction(nameof(ApplicationControllerConstants.Controller_Application_Method_SessionTimeout), nameof(ApplicationControllerConstants.Controller_Name_Application));
        }

        ResetDirectorSessionVariables();

        /* WE ARE NOT SETTING THE PAGE HERE AS IT WOULD BE TRICKIER TO IMPLEMENT */
        return RedirectToAction(nameof(ParentCompanyDirectorName));
    }

    [HttpPost]
    [Route("licence-conditions/parent-company-director-edit")]
    public async Task<IActionResult> ParentCompanyDirectorEdit(DirectorsEditViewModel model)
    {
        var applicationID = GetApplicationIdFromSession();

        if (applicationID == 0)
        {
            return RedirectToAction(
                nameof(ApplicationControllerConstants.Controller_Application_Method_SessionTimeout),
                nameof(ApplicationControllerConstants.Controller_Name_Application));
        }

        var exists = await directorOrchestration.DirectorExists(applicationID, model.Id);

        if (exists)
        {
            sessionOrchestration.Set(ApplicationFormConstants.SessionDirectorDatabaseId, model.Id);

            /* WE ARE NOT SETTING THE PAGE HERE AS IT WOULD BE TRICKIER TO IMPLEMENT */
            return RedirectToAction(nameof(ParentCompanyDirectorName));
        }

        throw new NotFoundException($"Parent company: Director with the id of {model.Id} doesn't exist for user {GetOktaUserId()}");
    }

    [HttpPost]
    [Route("licence-conditions/parent-company-director-remove")]
    public async Task<IActionResult> ParentCompanyDirectorRemove(DirectorsEditViewModel model)
    {
        var (applicationID, _) = GetDirectorIds();

        if (applicationID == 0 || model.Id == 0)
        {
            return RedirectToAction(nameof(ApplicationControllerConstants.Controller_Application_Method_SessionTimeout), nameof(ApplicationControllerConstants.Controller_Name_Application));
        }

        var exists = await directorOrchestration.DirectorExists(applicationID, model.Id);

        if (exists)
        {
            sessionOrchestration.Set(ApplicationFormConstants.SessionDirectorDatabaseId, model.Id);

            /* WE ARE NOT SETTING THE PAGE HERE AS IT WOULD BE TRICKIER TO IMPLEMENT */
            return RedirectToAction(nameof(ParentCompanyDirectorRemoveConfirm));
        }


        throw new NotFoundException($"Parent company: Director with the id of {model.Id} doesn't exist for user {GetOktaUserId()}");
    }

    [Route("licence-conditions/parent-company-director-remove-confirm")]
    public async Task<IActionResult> ParentCompanyDirectorRemoveConfirm()
    {
        var (applicationID, directorID) = GetDirectorIds();

        if (applicationID == 0 || directorID == 0)
        {
            return RedirectToAction(nameof(ApplicationControllerConstants.Controller_Application_Method_SessionTimeout), nameof(ApplicationControllerConstants.Controller_Name_Application));
        }

        return View(new DirectorRemoveConfirmViewModel()
        {
            Director = await directorOrchestration.GetDirectorDetails(applicationID, directorID),
            ValidationFailure = GetThenResetFormValidationErrorFromSession()
        });
    }

    [HttpPost]
    [Route("licence-conditions/parent-company-director-remove-confirm")]
    public async Task<IActionResult> ParentCompanyDirectorRemoveConfirm(DirectorRemoveConfirmViewModel model)
    {
        var (applicationID, directorID) = GetDirectorIds();

        if (applicationID == 0)
        {
            return RedirectToAction(nameof(ApplicationControllerConstants.Controller_Application_Method_SessionTimeout), nameof(ApplicationControllerConstants.Controller_Name_Application));
        }

        if (directorID == 0)
        {
            // If someone has deleted a director and then tried to go back from the directors page
            return RedirectToAction(nameof(ParentCompanyDirectors));
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

        return await SetPageAndRedirectToAction(applicationID, Domain.Models.Database.ApplicationPage.ParentCompanyDirectors);
    }

    [Route("licence-conditions/add-parent-company-director-name")]
    public async Task<IActionResult> ParentCompanyDirectorName()
    {
        return await GetDirectorDetailsFromSessionOrDatabase();
    }

    [HttpPost]
    [Route("licence-conditions/add-parent-company-director-name")]
    public async Task<IActionResult> ParentCompanyDirectorName(DirectorNameViewModel model)
    {
        return await ProcessDirectorName(model, nameof(ParentCompanyDirectorName), nameof(ParentCompanyDirectorDateOfBirth));
    }

    [Route("licence-conditions/add-parent-company-director-dob")]
    public async Task<IActionResult> ParentCompanyDirectorDateOfBirth()
    {
        return await SetupDirectorDateOfBirth();
    }

    [HttpPost]
    [Route("licence-conditions/add-parent-company-director-dob")]
    public async Task<IActionResult> ParentCompanyDirectorDateOfBirth(DirectorDateOfBirthViewModel model)
    {
        return await ProcessDirectorDateOfBirth(
            model,
            nameof(ParentCompanyDirectorDateOfBirth),
            nameof(ParentCompanyDirectors),
            GetDirectorGroupIdFromSession());
    }
}
