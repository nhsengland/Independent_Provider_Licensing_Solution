using System.Text.RegularExpressions;
using Domain.Logic.Forms.Application;
using Domain.Logic.Forms.Application.Page;
using Domain.Logic.Forms.Helpers;
using Domain.Logic.Forms.Helpers.Session;
using Domain.Models.Database;
using Licensing.Gateway.Models.Directors;
using Licensing.Gateway.Models.Directors.Corporate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Licensing.Gateway.Controllers;

[Authorize]
public class CorporateDirectorController(
    ILogger<CorporateDirectorController> logger,
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
    private readonly ILogger<CorporateDirectorController> logger = logger;
    private readonly IApplicationOrchestration applicationOrchestration = applicationOrchestration;
    private readonly IResponseConverter responseConverter = responseConverter;
    private readonly INextPageOrchestor nextPageOrchestor = nextPageOrchestor;
    private readonly IDirectorOrchestration directorOrchestration = directorOrchestration;
    private readonly ISessionOrchestration sessionOrchestration = sessionOrchestration;
    private readonly ISessionDateHandler sessionDateHandler = sessionDateHandler;

    [Route("licence-conditions/corporate-director-check")]
    public async Task<IActionResult> CorporateDirectorCheck()
    {
        var id = GetApplicationIdFromSession();

        if (id == 0)
        {
            return RedirectToAction(nameof(ApplicationControllerConstants.Controller_Application_Method_SessionTimeout), nameof(ApplicationControllerConstants.Controller_Name_Application));
        }

        return View(new CorporateDirectorCheck()
        {
            CorporateDirectorsCheck = await applicationOrchestration.GetCorporateDirectorsCheck(id),
            ProviderName = await applicationOrchestration.GetCQCProviderName(id),
            ValidationFailure = GetThenResetFormValidationErrorFromSession()
        });
    }

    [HttpPost]
    [Route("licence-conditions/corporate-director-check")]
    public async Task<IActionResult> CorporateDirectorCheck(CorporateDirectorCheck model)
    {
        var id = GetApplicationIdFromSession();

        if (id == 0)
        {
            return RedirectToAction(nameof(ApplicationControllerConstants.Controller_Application_Method_SessionTimeout), nameof(ApplicationControllerConstants.Controller_Name_Application));
        }

        SetSessionFormValidationError(false);

        var response = model.CorporateDirectorsCheck?.Trim() ?? string.Empty;

        await applicationOrchestration.SetCorporateDirectorsCheck(id, response);

        if (string.IsNullOrWhiteSpace(model.CorporateDirectorsCheck))
        {
            SetSessionFormValidationError(true);

            return RedirectToAction();
        }

        ResetGroupSessionVariables();

        if(model.CorporateDirectorsCheck == ApplicationFormConstants.Yes)
        {
            return await SetPageAndRedirectToAction(id, ApplicationPage.CorporateBodies);
        }

        return await SetPageAndRedirectToAction(
            id,
            nextPageOrchestor.NextPageAfterCorporateDirector(
                await directorOrchestration.CountDirectorsOfGroupType(id, DirectorType.Board),
                await directorOrchestration.CountDirectorsOfGroupType(id, DirectorType.Corporate),
                await directorOrchestration.CountGroups(id, DirectorType.Corporate)));
    }

    [Route("licence-conditions/corporate-bodies")]
    public async Task<IActionResult> CorporateBodies()
    {
        var id = GetApplicationIdFromSession();

        if (id == 0)
        {
            return RedirectToAction(nameof(ApplicationControllerConstants.Controller_Application_Method_SessionTimeout), nameof(ApplicationControllerConstants.Controller_Name_Application));
        }

        return View(new CorporateBodiesViewModel()
        {
            ProviderName = await applicationOrchestration.GetCQCProviderName(id),
            Groups = await directorOrchestration.GetGroups(id, DirectorType.Corporate)
        });
    }

    [HttpPost]
    [Route("licence-conditions/corporate-bodies")]
    public async Task<IActionResult> CorporateBodies(CorporateBodiesViewModel model)
    {
        var id = GetApplicationIdFromSession();

        if (id == 0)
        {
            return RedirectToAction(nameof(ApplicationControllerConstants.Controller_Application_Method_SessionTimeout), nameof(ApplicationControllerConstants.Controller_Name_Application));
        }

        var numberOfGroups = await directorOrchestration.CountGroups(id, DirectorType.Corporate);

        if(numberOfGroups == 0)
        {
            return await SetPageAndRedirectPostDirectors(id, ApplicationPage.CorporateDirectorCheck);
        }

        return await SetPageAndRedirectToAction(
            id,
            nextPageOrchestor.NextPageAfterCorporateDirector(
                await directorOrchestration.CountDirectorsOfGroupType(id, DirectorType.Board),
                await directorOrchestration.CountDirectorsOfGroupType(id, DirectorType.Corporate),
                await directorOrchestration.CountGroups(id, DirectorType.Corporate)));
    }

    [Route("licence-conditions/corporate-body-name")]
    public async Task<IActionResult> CorporateBodyName()
    {
        var id = GetApplicationIdFromSession();

        if (id == 0)
        {
            return RedirectToAction(nameof(ApplicationControllerConstants.Controller_Application_Method_SessionTimeout), nameof(ApplicationControllerConstants.Controller_Name_Application));
        }

        return View(new CorporateBodyNameViewModel()
        {
            CorporateBodyName = GetDirectorGroupNameFromSession(),
            ProviderName = await applicationOrchestration.GetCQCProviderName(id),
            ValidationFailure = GetThenResetFormValidationErrorFromSession()
        });
    }

    [HttpPost]
    [Route("licence-conditions/corporate-body-name")]
    public async Task<IActionResult> CorporateBodyName(CorporateBodyNameViewModel model)
    {
        var id = GetApplicationIdFromSession();

        if (id == 0)
        {
            return RedirectToAction(nameof(ApplicationControllerConstants.Controller_Application_Method_SessionTimeout), nameof(ApplicationControllerConstants.Controller_Name_Application));
        }

        SetSessionFormValidationError(false);

        await AquireDirectorGroup(id, GetDirectorGroupIdFromSession(), model.CorporateBodyName?.Trim() ?? string.Empty, DirectorType.Corporate);

        if (string.IsNullOrWhiteSpace(model.CorporateBodyName))
        {
            SetSessionFormValidationError(true);

            return RedirectToAction();
        }

        return await SetPageAndRedirectToAction(id, ApplicationPage.CorporateBodyIndividualsOrCompany);
    }

    [Route("licence-conditions/corporate-body-name-add")]
    public IActionResult CorporateBodyNameAdd()
    {
        ResetGroupSessionVariables();

        return RedirectToAction(nameof(CorporateBodyName));
    }

    [Route("licence-conditions/corporate-body-name-edit")]
    public async Task<IActionResult> CorporateBodyNameEdit(int groupId)
    {
        var result = await SetupGroupSessionDetails(groupId);

        return result == true ? RedirectToAction(nameof(CorporateBodyName)) : ReturnToStart();
    }

    [Route("licence-conditions/delete-corporate-body")]
    public async Task<IActionResult> CorporateBodyDelete(int groupId)
    {
        var result = await SetupGroupSessionDetails(groupId);

        if (result == false)
        {
            return RedirectToAction(nameof(ApplicationControllerConstants.Controller_Application_Method_SessionTimeout), nameof(ApplicationControllerConstants.Controller_Name_Application));
        }

        return View(new CorporateBodyDeleteViewModel()
        {
            GroupName = GetDirectorGroupNameFromSession(),
            ValidationFailure = GetThenResetFormValidationErrorFromSession()
        });
    }

    [HttpPost]
    [Route("licence-conditions/delete-corporate-body")]
    public async Task<IActionResult> CorporateBodyDelete(CorporateBodyDeleteViewModel model)
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

        return await SetPageAndRedirectToAction(id, ApplicationPage.CorporateBodies);
    }

    [Route("licence-conditions/corporate-body-individuals-or-company")]
    public async Task<IActionResult> CorporateBodyIndividualsOrCompany()
    {
        var id = GetApplicationIdFromSession();

        if (id == 0)
        {
            return RedirectToAction(nameof(ApplicationControllerConstants.Controller_Application_Method_SessionTimeout), nameof(ApplicationControllerConstants.Controller_Name_Application));
        }

        var model = new CorporateBodyIndividualsOrCompanyViewModel()
        {
            CorporateBodyName = GetDirectorGroupNameFromSession(),
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
    [Route("licence-conditions/corporate-body-individuals-or-company")]
    public async Task<IActionResult> CorporateBodyIndividualsOrCompany(CorporateBodyIndividualsOrCompanyViewModel model)
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

        var nextPage = model.Answer == ApplicationFormConstants.No ? ApplicationPage.CorporateBodies : ApplicationPage.CorporateDirectors;

        return await SetPageAndRedirectToAction(id, nextPage);
    }

    [Route("licence-conditions/corporate-body-individuals-or-company-edit")]
    public async Task<IActionResult> CorporateBodyIndividualsOrCompanyEdit(int groupId)
    {
        var result = await SetupGroupSessionDetails(groupId);

        return result == true ? RedirectToAction(nameof(CorporateBodyIndividualsOrCompany)) : ReturnToStart();
    }

    [Route("licence-conditions/corporate-directors")]
    public async Task<IActionResult> CorporateDirectors(int groupId)
    {
        var id = GetApplicationIdFromSession();

        if (id == 0)
        {
            return RedirectToAction(nameof(ApplicationControllerConstants.Controller_Application_Method_SessionTimeout), nameof(ApplicationControllerConstants.Controller_Name_Application));
        }

        if (groupId > 0)
        {
            var groupBelongsToOrganisation = await SetupGroupSessionDetails(groupId);

            if (!groupBelongsToOrganisation)
            {
                RedirectToAction(ApplicationControllerConstants.Controller_Home_Method_Forbidden, ApplicationControllerConstants.Controller_Name_Home);
            }
        }

        return View(new DirectorsViewModel()
        {
            GroupName = GetDirectorGroupNameFromSession(),
            Directors = await directorOrchestration.GetDirectors(id, DirectorType.Corporate, GetDirectorGroupIdFromSession()),
            ProviderName = await applicationOrchestration.GetCQCProviderName(id),
        });
    }

    [HttpPost]
    [Route("licence-conditions/corporate-directors")]
    public async Task<IActionResult> CorporateDirectors(DirectorsViewModel _)
    {
        var id = GetApplicationIdFromSession();

        if (id == 0)
        {
            return RedirectToAction(nameof(ApplicationControllerConstants.Controller_Application_Method_SessionTimeout), nameof(ApplicationControllerConstants.Controller_Name_Application));
        }

        var numberOfDirectors = await directorOrchestration.CountDirectorsInGroup(GetDirectorGroupIdFromSession());

        if (numberOfDirectors == 0)
        {
            return await SetPageAndRedirectPostDirectors(id, ApplicationPage.CorporateBodyIndividualsOrCompany);
        }

        return await SetPageAndRedirectToAction(id, Domain.Models.Database.ApplicationPage.CorporateBodies);
    }

    [Route("licence-conditions/corporate-directors-edit")]
    public async Task<IActionResult> CorporateDirectorsEdit(int groupId)
    {
        var result = await SetupGroupSessionDetails(groupId);

        return result == true ? RedirectToAction(nameof(CorporateDirectors)) : ReturnToStart();
    }

    [HttpPost]
    [Route("licence-conditions/add-corporate-director")]
    public IActionResult CorporateDirectorAdd(DirectorsViewModel _)
    {
        var applicationID = GetApplicationIdFromSession();

        if (applicationID == 0)
        {
            return RedirectToAction(nameof(ApplicationControllerConstants.Controller_Application_Method_SessionTimeout), nameof(ApplicationControllerConstants.Controller_Name_Application));
        }

        ResetDirectorSessionVariables();

        /* WE ARE NOT SETTING THE PAGE HERE AS IT WOULD BE TRICKIER TO IMPLEMENT */
        return RedirectToAction(nameof(CorporateDirectorName));
    }

    [HttpPost]
    [Route("licence-conditions/corporate-director-edit")]
    public async Task<IActionResult> CorporateDirectorEdit(DirectorsEditViewModel model)
    {
        var applicationID = GetApplicationIdFromSession();

        if (applicationID == 0)
        {
            return RedirectToAction(nameof(ApplicationControllerConstants.Controller_Application_Method_SessionTimeout), nameof(ApplicationControllerConstants.Controller_Name_Application));
        }

        var exists = await directorOrchestration.DirectorExists(applicationID, model.Id);

        if (exists)
        {
            sessionOrchestration.Set(ApplicationFormConstants.SessionDirectorDatabaseId, model.Id);

            /* WE ARE NOT SETTING THE PAGE HERE AS IT WOULD BE TRICKIER TO IMPLEMENT */
            return RedirectToAction(nameof(CorporateDirectorName));
        }

        return RedirectToAction(nameof(ApplicationControllerConstants.Controller_Application_Method_SessionTimeout), nameof(ApplicationControllerConstants.Controller_Name_Application));
    }

    [HttpPost]
    [Route("licence-conditions/corporate-director-remove")]
    public async Task<IActionResult> CorporateDirectorRemove(DirectorsEditViewModel model)
    {
        var (applicationID, directorID) = GetDirectorIds();

        if (applicationID == 0 || model.Id == 0)
        {
            return RedirectToAction(nameof(ApplicationControllerConstants.Controller_Application_Method_SessionTimeout), nameof(ApplicationControllerConstants.Controller_Name_Application));
        }

        var exists = await directorOrchestration.DirectorExists(applicationID, model.Id);

        if (exists)
        {
            sessionOrchestration.Set(ApplicationFormConstants.SessionDirectorDatabaseId, model.Id);

            /* WE ARE NOT SETTING THE PAGE HERE AS IT WOULD BE TRICKIER TO IMPLEMENT */
            return RedirectToAction(nameof(CorporateDirectorRemoveConfirm));
        }

        return RedirectToAction(nameof(ApplicationControllerConstants.Controller_Application_Method_SessionTimeout), nameof(ApplicationControllerConstants.Controller_Name_Application));
    }

    [Route("licence-conditions/corporate-director-remove-confirm")]
    public async Task<IActionResult> CorporateDirectorRemoveConfirm()
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
    [Route("licence-conditions/corporate-director-remove-confirm")]
    public async Task<IActionResult> CorporateDirectorRemoveConfirm(DirectorRemoveConfirmViewModel model)
    {
        var (applicationID, directorID) = GetDirectorIds();

        if (applicationID == 0)
        {
            return RedirectToAction(nameof(ApplicationControllerConstants.Controller_Application_Method_SessionTimeout), nameof(ApplicationControllerConstants.Controller_Name_Application));
        }

        if (directorID == 0)
        {
            // If someone has deleted a director and then tried to go back from the directors page
            return RedirectToAction(nameof(CorporateDirectors));
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

        return await SetPageAndRedirectToAction(applicationID, Domain.Models.Database.ApplicationPage.CorporateDirectors);
    }

    [Route("licence-conditions/add-corporate-director-name")]
    public async Task<IActionResult> CorporateDirectorName()
    {
        return await GetDirectorDetailsFromSessionOrDatabase();
    }

    [HttpPost]
    [Route("licence-conditions/add-corporate-director-name")]
    public async Task<IActionResult> CorporateDirectorName(DirectorNameViewModel model)
    {
        return await ProcessDirectorName(model, nameof(CorporateDirectorName), nameof(CorporateDirectorDateOfBirth));
    }

    [Route("licence-conditions/add-corporate-director-dob")]
    public async Task<IActionResult> CorporateDirectorDateOfBirth()
    {
        return await SetupDirectorDateOfBirth();
    }

    [HttpPost]
    [Route("licence-conditions/add-corporate-director-dob")]
    public async Task<IActionResult> CorporateDirectorDateOfBirth(DirectorDateOfBirthViewModel model)
    {
        return await ProcessDirectorDateOfBirth(
            model,
            nameof(CorporateDirectorDateOfBirth),
            nameof(CorporateDirectors),
            GetDirectorGroupIdFromSession());
    }
}
