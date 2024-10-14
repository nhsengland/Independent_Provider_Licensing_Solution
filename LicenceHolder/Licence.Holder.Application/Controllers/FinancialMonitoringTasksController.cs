using Domain.Logic.Features.Tasks;
using Domain.Logic.Features.Tasks.Requests;
using Domain.Logic.Integrations.Session;
using Domain.Objects;
using Licence.Holder.Application.Models.AnnualCertificateTasks;
using Microsoft.AspNetCore.Mvc;

namespace Licence.Holder.Application.Controllers;

public class FinancialMonitoringTasksController(
    ILogger<FinancialMonitoringTasksController> logger,
    ITaskControllerHandler taskControllerHandler,
    ISessionOrchestration sessionOrchestration) : BaseController(sessionOrchestration)
{
    private readonly ILogger<FinancialMonitoringTasksController> logger = logger;
    private readonly ITaskControllerHandler taskControllerHandler = taskControllerHandler;

    [Route("financial-monitoring-template")]
    public async Task<IActionResult> Index(
        FinancialMonitoringTaskRequest request,
        CancellationToken cancellationToken)
    {
        request.UserOktaId = GetOktaUserId();

        var isUsersRoleLevel2 = await taskControllerHandler.IsUsersRoleLevel2(request.UserOktaId, cancellationToken);

        if (!isUsersRoleLevel2)
        {
            logger.LogWarning("User {UserOktaId} attempted to access Financial Monitoring Task with insufficient permissions", request.UserOktaId);

            return RedirectToAction(nameof(NotAuthorised));
        }

        return View(await taskControllerHandler.Get(request, cancellationToken));
    }

    [Route("financial-monitoring-template/update-status")]
    public async Task<IActionResult> UpdateSubmissionStatus(
        FinancialMonitoringTaskRequest request,
        CancellationToken cancellationToken)
    {
        return await OrchestrateUpdateSubmissionStatusResponse(request, cancellationToken);
    }

    public async Task<IActionResult> UpdateSubmissionStatusFromSession(
        CancellationToken cancellationToken)
    {
        var session = Get_Session_UpdateSubmissionStatus();

        if(session.taskId == 0 || session.organisationOrlicenseId == 0)
        {
            return RedirectToAction("Index", "Home");
        }

        return await OrchestrateUpdateSubmissionStatusResponse(new FinancialMonitoringTaskRequest()
        {
            TaskId = session.taskId,
            OrganisationId = session.organisationOrlicenseId,
            UserOktaId = GetOktaUserId(),
        }, cancellationToken);
    }

    [Route("financial-monitoring-template/update-status")]
    [HttpPost]
    public async Task<IActionResult> UpdateSubmissionStatus(
        FMTUpdateSubmissionStatusViewModel model,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(model.UpdateSubmissionStatus))
        {
            Set_Session_UpdateSubmissionStatus(model.TaskId, model.OrganisationId);

            return RedirectToAction(nameof(UpdateSubmissionStatusFromSession));
        }

        Remove_Session_UpdateSubmissionStatus();

        if (model.UpdateSubmissionStatus == PageConstants.Yes)
        {
            await taskControllerHandler.UpdateStatusOfTask(new FinancialMonitoringTaskRequest()
            {
                UserOktaId = GetOktaUserId(),
                TaskId = model.TaskId,
                OrganisationId = model.OrganisationId,
            }, Domain.Objects.Database.TaskStatus.Completed, cancellationToken);

            return RedirectToAction(nameof(UpdateSubmissionStatusConfirmation));
        }
        else
        {
            return RedirectToAction("Index", "Home");
        }
    }

    [Route("financial-monitoring-template/update-status/confirmation")]
    public IActionResult UpdateSubmissionStatusConfirmation()
    {
        return View();
    }

    [Route("financial-monitoring-template/update-status/cancel")]
    public IActionResult UpdateSubmissionStatusCancel()
    {
        Remove_Session_UpdateSubmissionStatus();

        return RedirectToAction("Index", "Home");
    }

    [Route("financial-monitoring-template/feedback")]
    public IActionResult Feedback()
    {
        Set_Session_FeedbackType(Domain.Objects.Database.FeedbackType.Tasks);

        return RedirectToAction("Index", "Feedback");
    }

    private async Task<IActionResult> OrchestrateUpdateSubmissionStatusResponse(FinancialMonitoringTaskRequest request, CancellationToken cancellationToken)
    {
        request.UserOktaId = GetOktaUserId();

        var details = await taskControllerHandler.Get(request, cancellationToken);

        return View(nameof(UpdateSubmissionStatus), new FMTUpdateSubmissionStatusViewModel()
        {
            OrganisationName = details.Name,
            TaskId = request.TaskId,
            OrganisationId = request.OrganisationId,
            ValidationFailure = GetFromSession_FormValidationFailure_ThenReset(),
        });
    }

    public IActionResult NotAuthorised()
    {
        return View();
    }
}