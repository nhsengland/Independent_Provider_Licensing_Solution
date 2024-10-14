using Database.Repositories.Orchestrate;
using Database.Repositories.User;
using Domain.Logic.Features.Team;
using Domain.Logic.Features.Team.InviteUser;
using Domain.Logic.Features.Team.ManageUsers;
using Domain.Logic.Features.Team.ManageUsers.Queries;
using Domain.Logic.Features.Team.Maps;
using Domain.Logic.Integrations.Session;
using Domain.Objects;
using Licence.Holder.Application.Models.Team;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Licence.Holder.Application.Controllers;

[Authorize]
public class TeamController(
    ILogger<TeamController> logger,
    IManageUsersHandler manageUsersHandler,
    ITeamManagementHandler settingsControllerHandler,
    IAddUserHandler addUserHandler,
    IAccessLevelMapper accessLevelMapper,
    ISessionOrchestration sessionOrchestration) : BaseController(sessionOrchestration)
{
    private readonly ILogger<TeamController> logger = logger;
    private readonly IManageUsersHandler manageUsersHandler = manageUsersHandler;
    private readonly ITeamManagementHandler settingsControllerHandler = settingsControllerHandler;
    private readonly IAddUserHandler addUserHandler = addUserHandler;
    private readonly IAccessLevelMapper accessLevelMapper = accessLevelMapper;
    private readonly ISessionOrchestration sessionOrchestration = sessionOrchestration;

    [Route("team-management/index")]
    public async Task<IActionResult> Index(
        CancellationToken cancellationToken)
    {
        return View(await settingsControllerHandler.Get(new TeamManagementRequest() { OktaUserId = GetOktaUserId() }, cancellationToken));
    }

    [Route("team-management/manage-users")]
    public async Task<IActionResult> ManageUsers(
        CancellationToken cancellationToken)
    {
        return View(await manageUsersHandler.Get(new GetManageUsersViewModelQuery() { OktaUserId = GetOktaUserId() }, cancellationToken));
    }

    [Route("team-management/manage-user")]
    public async Task<IActionResult> ManageUser(int id, CancellationToken cancellationToken)
    {
        var oktaUserId = GetOktaUserId();

        var allowedToEditUser = await UsersAreInTheSameOrganisation(id, cancellationToken);

        if (!allowedToEditUser)
        {
            logger.LogWarning("Okta User {oktaUserId}, has attempted to edit a user that they are not allowed to, user id {id}", oktaUserId, id);
            return RedirectToAction(nameof(ManageUsers));
        }

        return View(await manageUsersHandler.Get(new GetManageUserViewModelQuery() { OktaUserId = oktaUserId, UserId = id }, cancellationToken));
    }

    [HttpPost]
    [Route("team-management/manage-user-resend-invitation")]
    public async Task<IActionResult> ManageUserResendInvitation(int id, CancellationToken cancellationToken)
    {
        var userCanEdit = await UsersAreInTheSameOrganisation(id, cancellationToken);

        if (!userCanEdit)
        {
            logger.LogWarning("User, {GetOktaUserId}, is not allowed to edit this user: {id}", GetOktaUserId(), id);
            return RedirectToAction(nameof(ManageUsers));
        }

        await addUserHandler.ReInviteUser(id, GetOktaUserId(), cancellationToken);

        return View(new ResendInvitationViewModel() { UserId = id });
    }

    [Route("team-management/manage-user-change-access-level")]
    public async Task<IActionResult> ManageUserChangeAccessLevel(
        int id,
        CancellationToken cancellationToken)
    {
        var oktaUserId = GetOktaUserId();

        var userCanEdit = await UsersAreInTheSameOrganisation(id, cancellationToken);

        if (!userCanEdit)
        {
            logger.LogWarning("User, {oktaUserId}, is not allowed to edit this user: {id}", oktaUserId, id);
            return RedirectToAction(nameof(ManageUsers));
        }

        var model = await manageUsersHandler.Get(new GetManageUserChangeAccessLevelViewModel() { OktaUserId = oktaUserId, UserId = id }, cancellationToken);

        sessionOrchestration.Set(PageConstants.Session_ChangeUser_Id, id);

        return View(new UserChangeAccessLevelViewModel()
        {
            FirstName = model.FirstName,
            LastName = model.LastName,
            Email = model.Email,
            AccessLevel = model.AccessLevel,
        });
    }

    [HttpPost]
    [Route("team-management/manage-user-change-access-level")]
    public async Task<IActionResult> ManageUserChangeAccessLevel(
        UserChangeAccessLevelViewModel model,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(model.AccessLevel))
        {
            model.ValidationFailure = true;
            return View(model);
        }

        var id = sessionOrchestration.Get<int>(PageConstants.Session_ChangeUser_Id);

        await manageUsersHandler.UpdateUserAccessLevel(id, accessLevelMapper.MapToUserRole(model.AccessLevel), cancellationToken);

        sessionOrchestration.Remove(PageConstants.Session_ChangeUser_Id);

        return RedirectToAction(nameof(ManageUserChangeAccessLevelConfirmation));
    }

    [Route("team-management/manage-user-change-access-level-confirmation")]
    public IActionResult ManageUserChangeAccessLevelConfirmation()
    {
        return View();
    }

    [Route("team-management/manage-user-change-access-level-cancel")]
    public IActionResult ManageUserChangeAccessLevelCancel(UserChangeAccessLevelViewModel _)
    {
        sessionOrchestration.Remove(PageConstants.Session_ChangeUser_Id);

        return RedirectToAction(nameof(ManageUsers));
    }

    [Route("team-management/feedback")]
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

    private async Task<bool> UsersAreInTheSameOrganisation(int id, CancellationToken cancellationToken)
    {
        return await manageUsersHandler.UsersAreInTheSameOrganisation(new AllowedToEditUserQuery() { OktaUserId = GetOktaUserId(), UserId = id }, cancellationToken);
    }
}
