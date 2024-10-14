using Domain.Logic.Forms.Application;
using Domain.Logic.Forms.Application.Page;
using Domain.Logic.Forms.Helpers;
using Domain.Logic.Forms.Helpers.Session;
using Domain.Models.Exceptions;
using Licensing.Gateway.Models.UltimateController;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Licensing.Gateway.Controllers;

[Authorize]
public class UltimateControllerController(
    ILogger<UltimateControllerController> logger,
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
    private readonly ILogger logger = logger;
    private readonly IApplicationOrchestration applicationOrchestration = applicationOrchestration;
    private readonly IResponseConverter responseConverter = responseConverter;
    private readonly INextPageOrchestor nextPageOrchestor = nextPageOrchestor;
    private readonly IDirectorOrchestration directorOrchestration = directorOrchestration;
    private readonly ISessionOrchestration sessionOrchestration = sessionOrchestration;
    private readonly ISessionDateHandler sessionDateHandler = sessionDateHandler;

    [Route("final-checks/ultimate-controller")]
    public async Task<IActionResult> UltimateController()
    {
        var id = GetApplicationIdFromSession();

        if (id == 0)
        {
            return RedirectToAction(nameof(ApplicationControllerConstants.Controller_Application_Method_SessionTimeout), nameof(ApplicationControllerConstants.Controller_Name_Application));
        }

        return View(new UltimateControllerViewModel()
        {
            UltimateController = await applicationOrchestration.GetUltimateController(id),
            ValidationFailure = GetThenResetFormValidationErrorFromSession()
        });
    }

    [HttpPost]
    [Route("final-checks/ultimate-controller")]
    public async Task<IActionResult> UltimateController(UltimateControllerViewModel model)
    {
        var id = GetApplicationIdFromSession();

        if (id == 0)
        {
            return RedirectToAction(nameof(ApplicationControllerConstants.Controller_Application_Method_SessionTimeout), nameof(ApplicationControllerConstants.Controller_Name_Application));
        }

        SetSessionFormValidationError(false);

        await applicationOrchestration.SetUltimateController(id, model.UltimateController?.Trim() ?? string.Empty);

        if (string.IsNullOrWhiteSpace(model.UltimateController))
        {
            SetSessionFormValidationError(true);
            return RedirectToAction();
        }

        if (model.UltimateController == ApplicationFormConstants.Yes)
        {
            return await SetPageAndRedirectToAction(id, Domain.Models.Database.ApplicationPage.UltimateControllers);
        }

        return await RedirectToReviewOrPage(id, Domain.Models.Database.ApplicationPage.Review);
    }

    [Route("final-checks/ultimate-controllers")]
    public async Task<IActionResult> UltimateControllers()
    {
        var id = GetApplicationIdFromSession();

        if (id == 0)
        {
            return RedirectToAction(nameof(ApplicationControllerConstants.Controller_Application_Method_SessionTimeout), nameof(ApplicationControllerConstants.Controller_Name_Application));
        }

        return View(new UltimateControllersViewModel()
        {
            UltimateControllers = await applicationOrchestration.GetUltimateContorllers(id),
        });
    }

    [HttpPost]
    [Route("final-checks/ultimate-controllers")]
    public async Task<IActionResult> UltimateControllers(UltimateControllersViewModel _)
    {
        var id = GetApplicationIdFromSession();

        if (id == 0)
        {
            return RedirectToAction(nameof(ApplicationControllerConstants.Controller_Application_Method_SessionTimeout), nameof(ApplicationControllerConstants.Controller_Name_Application));
        }

        var ultimateControllers = await applicationOrchestration.GetUltimateContorllers(id);

        if(ultimateControllers.Count == 0)
        {
            return RedirectToAction(nameof(UltimateController));
        }

        return await RedirectToReviewOrPage(id, Domain.Models.Database.ApplicationPage.Review);
    }

    [Route("final-checks/ultimate-controller-name")]
    public async Task<IActionResult> UltimateControllerName()
    {
        var (applicationId, ultimateControllerId) = GetUltimateControllerIds();

        if (applicationId == 0 || ultimateControllerId == 0)
        {
            return RedirectToAction(nameof(ApplicationControllerConstants.Controller_Application_Method_SessionTimeout), nameof(ApplicationControllerConstants.Controller_Name_Application));
        }

        return View(new UltimateControllerNameViewModel()
        {
            Name = await applicationOrchestration.GetUltimateControllerName(applicationId, ultimateControllerId),
            ValidationFailure = GetThenResetFormValidationErrorFromSession()
        });
    }

    [HttpPost]
    [Route("final-checks/ultimate-controller-name")]
    public async Task<IActionResult> UltimateControllerName(UltimateControllerNameViewModel model)
    {
        var (applicationId, ultimateControllerId) = GetUltimateControllerIds();

        if (applicationId == 0 || ultimateControllerId == 0)
        {
            return RedirectToAction(nameof(ApplicationControllerConstants.Controller_Application_Method_SessionTimeout), nameof(ApplicationControllerConstants.Controller_Name_Application));
        }

        SetSessionFormValidationError(false);

        await applicationOrchestration.SetUltimateControllerName(applicationId, ultimateControllerId, model.Name?.Trim() ?? string.Empty);

        if (string.IsNullOrWhiteSpace(model.Name))
        {
            SetSessionFormValidationError(true);

            return RedirectToAction();
        }

        return await RedirectToReviewOrPage(applicationId, Domain.Models.Database.ApplicationPage.UltimateControllers);
    }

    [HttpPost]
    [Route("final-checks/add-ultimate-controller")]
    public async Task<IActionResult> UltimateControllerAdd(UltimateControllerViewModel _)
    {
        var id = GetApplicationIdFromSession();

        if (id == 0)
        {
            return RedirectToAction(nameof(ApplicationControllerConstants.Controller_Application_Method_SessionTimeout), nameof(ApplicationControllerConstants.Controller_Name_Application));
        }

        var ultimateControllerId = await applicationOrchestration.CreateUltimateContorller(id);

        sessionOrchestration.Set(ApplicationFormConstants.SessionUltimateControllerId, ultimateControllerId);

        /* WE ARE NOT SETTING THE PAGE HERE AS IT WOULD BE TRICKIER TO IMPLEMENT */
        return RedirectToAction(nameof(UltimateControllerName));
    }

    [HttpPost]
    [Route("final-checks/edit-ultimate-controller")]
    public async Task<IActionResult> UltimateControllerEdit(UltimateControllerEditViewModel model)
    {
        var id = GetApplicationIdFromSession();

        if (id == 0)
        {
            return RedirectToAction(nameof(ApplicationControllerConstants.Controller_Application_Method_SessionTimeout), nameof(ApplicationControllerConstants.Controller_Name_Application));
        }

        var exists = await applicationOrchestration.UltimateControllerExists(id, model.Id);

        if (exists)
        {
            sessionOrchestration.Set(ApplicationFormConstants.SessionUltimateControllerId, model.Id);

            /* WE ARE NOT SETTING THE PAGE HERE AS IT WOULD BE TRICKIER TO IMPLEMENT */
            return RedirectToAction(nameof(UltimateControllerName));
        }

        throw new NotFoundException($"Ultimate controller (EDIT) with the id of {model.Id} doesn't exist for user {GetOktaUserId()}");
    }

    [HttpPost]
    [Route("final-checks/remove-ultimate-controller")]
    public async Task<IActionResult> UltimateControllerRemove(UltimateControllerEditViewModel model)
    {
        var id = GetApplicationIdFromSession();

        if (id == 0)
        {
            return RedirectToAction(nameof(ApplicationControllerConstants.Controller_Application_Method_SessionTimeout), nameof(ApplicationControllerConstants.Controller_Name_Application));
        }

        var exists = await applicationOrchestration.UltimateControllerExists(id, model.Id);

        if (exists)
        {
            sessionOrchestration.Set(ApplicationFormConstants.SessionUltimateControllerId, model.Id);

            /* WE ARE NOT SETTING THE PAGE HERE AS IT WOULD BE TRICKIER TO IMPLEMENT */
            return RedirectToAction(nameof(UltimateControllerRemoveConfirm));
        }

        throw new NotFoundException($"Ultimate controller (REMOVE) with the id of {model.Id} doesn't exist for user {GetOktaUserId()}");
    }

    [Route("final-checks/remove-ultimate-controller-confirm")]
    public async Task<IActionResult> UltimateControllerRemoveConfirm()
    {
        var (applicationId, ultimateControllerId) = GetUltimateControllerIds();

        if (applicationId == 0 || ultimateControllerId == 0)
        {
            return RedirectToAction(nameof(ApplicationControllerConstants.Controller_Application_Method_SessionTimeout), nameof(ApplicationControllerConstants.Controller_Name_Application));
        }

        return View(new UltimateControllerRemoveConfirmViewModel()
        {
            UltimateControllerName = await applicationOrchestration.GetUltimateControllerName(applicationId, ultimateControllerId),
            ValidationFailure = GetThenResetFormValidationErrorFromSession()
        });
    }

    [HttpPost]
    [Route("final-checks/remove-ultimate-controller-confirm")]
    public async Task<IActionResult> UltimateControllerRemoveConfirm(UltimateControllerRemoveConfirmViewModel model)
    {
        var (applicationId, ultimateControllerId) = GetUltimateControllerIds();

        if (applicationId == 0 || ultimateControllerId == 0)
        {
            return RedirectToAction(
                nameof(ApplicationControllerConstants.Controller_Application_Method_SessionTimeout),
                nameof(ApplicationControllerConstants.Controller_Name_Application));
        }

        SetSessionFormValidationError(false);

        if (string.IsNullOrWhiteSpace(model.Confirmation))
        {
            SetSessionFormValidationError(true);

            return RedirectToAction();
        }

        if (model.Confirmation == ApplicationFormConstants.Yes)
        {
            await applicationOrchestration.DeleteUltimateContorller(applicationId, ultimateControllerId);
        }

        return await RedirectToReviewOrPage(applicationId, Domain.Models.Database.ApplicationPage.UltimateControllers);
    }
}
