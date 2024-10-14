using Database.Repositories.Orchestrate;
using Database.Repositories.User;
using Domain.Logic.Features.Team;
using Domain.Logic.Features.Team.Delete;
using Domain.Logic.Features.Team.InviteUser;
using Domain.Logic.Features.Team.ManageUsers;
using Domain.Logic.Features.Team.Maps;
using Domain.Logic.Integrations.Session;
using Domain.Objects;
using Licence.Holder.Application.Models.Team;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Licence.Holder.Application.Controllers;

[Authorize]
public class TeamDeleteController(
    ILogger<TeamDeleteController> logger,
    IManageUsersHandler manageUsersViewModelHandler,
    IInviteNewUserHandler inviteNewUserHandler,
    IRepositoryForUser repositoryForUser,
    IDeleteUserHandler deleteUserHandler,
    ISessionOrchestration sessionOrchestration) : BaseController(sessionOrchestration)
{
    private readonly ILogger<TeamDeleteController> logger = logger;
    private readonly IManageUsersHandler manageUsersViewModelHandler = manageUsersViewModelHandler;
    private readonly IInviteNewUserHandler inviteNewUserHandler = inviteNewUserHandler;
    private readonly IRepositoryForUser repositoryForUser = repositoryForUser;
    private readonly IDeleteUserHandler deleteUserHandler = deleteUserHandler;
    private readonly ISessionOrchestration sessionOrchestration = sessionOrchestration;

    [Route("team-management/delete-user")]
    public async Task<IActionResult> DeleteUser(int id, CancellationToken cancellationToken)
    {
        var userCanEdit = await deleteUserHandler.UsersAreInTheSameOrganisation(id, GetOktaUserId(), cancellationToken);

        if (!userCanEdit)
        {
            logger.LogWarning("User, {GetOktaUserId}, is not allowed to edit this user: {id}", GetOktaUserId(), id);
            return RedirectToAction(nameof(TeamController.ManageUsers), "Team");
        }

        sessionOrchestration.Set(PageConstants.Session_ChangeUser_Id, id);

        return RedirectToAction(nameof(DeleteUserCheck));
    }

    [Route("team-management/delete-user-check")]
    public async Task<IActionResult> DeleteUserCheck(
        CancellationToken cancellationToken)
    {
        var userId = sessionOrchestration.Get<int>(PageConstants.Session_ChangeUser_Id);

        var userDetails = await deleteUserHandler.Get(new Domain.Logic.Features.Team.Delete.Requests.GetUserDetailsForDeleteUserRequest() { UserID = userId, OktaUserId = GetOktaUserId() }, cancellationToken);

        return View(new DeleteUserCheckViewModel()
        {
            FirstName = userDetails.FirstName,
            LastName = userDetails.LastName,
            Email = userDetails.Email,
            IsVerified = userDetails.IsVerified,
            DateLastEmailNotificationWasCreated = userDetails.DateLastEmailNotificationWasCreated
        });
    }

    [HttpPost]
    [Route("team-management/delete-user-check")]
    public async Task<IActionResult> DeleteUserCheck(
        DeleteUserCheckViewModel model,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(model.DeleteUser))
        {
            model.ValidationFailure = true;
            return View(model);
        }

        if (model.DeleteUser == PageConstants.Yes)
        {
            var userId = sessionOrchestration.Get<int>(PageConstants.Session_ChangeUser_Id);

            await deleteUserHandler.SoftDeleteUser(
                GetOktaUserId(),
                userId,
                cancellationToken);

            var emailIsVerified = await deleteUserHandler.UserEmailIsVerified(userId, cancellationToken);

            sessionOrchestration.Remove(PageConstants.Session_ChangeUser_Id);

            if (emailIsVerified)
            {
                return RedirectToAction(nameof(DeleteUserConfirmation));
            }
            else
            {
                return RedirectToAction(nameof(DeletePendingUserConfirmation));
            }

        }

        sessionOrchestration.Remove(PageConstants.Session_ChangeUser_Id);

        return RedirectToAction(nameof(TeamController.ManageUsers), "Team");
    }

    [Route("team-management/delete-pending-user/confirmation")]
    public IActionResult DeletePendingUserConfirmation()
    {
        return View();
    }

    [Route("team-management/delete-user/confirmation")]
    public IActionResult DeleteUserConfirmation()
    {
        return View();
    }

    [HttpPost]
    [Route("team-management/delete-user-cancel")]
    public IActionResult DeleteUserCancel()
    {
        sessionOrchestration.Remove(PageConstants.Session_ChangeUser_Id);

        return RedirectToAction(nameof(TeamController.ManageUsers), "Team");
    }

    [Route("team-management/delete-user-feedback")]
    public IActionResult Feedback()
    {
        Set_Session_FeedbackType(Domain.Objects.Database.FeedbackType.Team);

        return RedirectToAction("Index", "Feedback");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new Models.ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
