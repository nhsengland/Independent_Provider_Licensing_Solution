using Domain.Logic.Features.Tasks;
using Domain.Logic.Features.Tasks.Requests;
using Domain.Logic.Integrations.Session;
using Domain.Objects;
using Licence.Holder.Application.Models.AnnualCertificateTasks;
using Microsoft.AspNetCore.Mvc;

namespace Licence.Holder.Application.Controllers;

public class AnnualCertificateTasksController(
    ITaskControllerHandler taskControllerHandler,
    ISessionOrchestration sessionOrchestration) : BaseController(sessionOrchestration)
{
    private readonly ITaskControllerHandler taskControllerHandler = taskControllerHandler;
    private readonly ISessionOrchestration sessionOrchestration = sessionOrchestration;

    [Route("annual-certification-template")]
    public async Task<IActionResult> Index(
        AnnualCertificateTaskRequest request,
        CancellationToken cancellationToken)
    {
        request.UserOktaId = GetOktaUserId();

        return View(await taskControllerHandler.Get(request, cancellationToken));
    }

    [Route("annual-certification-template/update-status")]
    public async Task<IActionResult> UpdateSubmissionStatus(
        AnnualCertificateTaskRequest request,
        CancellationToken cancellationToken)
    {
        return await OrchestrateUpdateSubmissionStatusResponse(request, cancellationToken);
    }

    public async Task<IActionResult> UpdateSubmissionStatusFromSession(
        CancellationToken cancellationToken)
    {
        var session = Get_Session_UpdateSubmissionStatus();
        return await OrchestrateUpdateSubmissionStatusResponse(new AnnualCertificateTaskRequest()
        {
            TaskId = session.taskId,
            LicenseId = session.organisationOrlicenseId,
            UserOktaId = GetOktaUserId(),
        }, cancellationToken);
    }

    [Route("annual-certification-template/update-status")]
    [HttpPost]
    public async Task<IActionResult> UpdateSubmissionStatus(
        ACTUpdateSubmissionStatusViewModel model,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(model.UpdateSubmissionStatus))
        {
            Set_Session_UpdateSubmissionStatus(model.TaskId, model.LicenseId);

            return RedirectToAction(nameof(UpdateSubmissionStatusFromSession));
        }

        Remove_Session_UpdateSubmissionStatus();

        if (model.UpdateSubmissionStatus == PageConstants.Yes)
        {
            await taskControllerHandler.UpdateStatusOfTask(new AnnualCertificateTaskRequest()
            {
                UserOktaId = GetOktaUserId(),
                TaskId = model.TaskId,
                LicenseId = model.LicenseId,
            }, Domain.Objects.Database.TaskStatus.Completed, cancellationToken);

            return RedirectToAction(nameof(UpdateSubmissionStatusConfirmation));
        }
        else
        {
            return RedirectToAction("Index", "Home");
        }
    }

    [Route("annual-certification-template/update-status/confirmation")]
    public IActionResult UpdateSubmissionStatusConfirmation()
    {
        return View();
    }

    [Route("annual-certification-template/update-status/cancel")]
    public IActionResult UpdateSubmissionStatusCancel()
    {
        Remove_Session_UpdateSubmissionStatus();

        return RedirectToAction("Index", "Home");
    }

    [Route("annual-certification-template/feedback")]
    public IActionResult Feedback()
    {
        Set_Session_FeedbackType(Domain.Objects.Database.FeedbackType.Tasks);

        return RedirectToAction("Index", "Feedback");
    }

    private async Task<IActionResult> OrchestrateUpdateSubmissionStatusResponse(AnnualCertificateTaskRequest request, CancellationToken cancellationToken)
    {
        request.UserOktaId = GetOktaUserId();

        var details = await taskControllerHandler.Get(request, cancellationToken);

        return View(nameof(UpdateSubmissionStatus), new ACTUpdateSubmissionStatusViewModel()
        {
            LicenceName = details.Name,
            TaskId = request.TaskId,
            LicenseId = request.LicenseId,
            ValidationFailure = GetFromSession_FormValidationFailure_ThenReset(),
        });
    }
}
