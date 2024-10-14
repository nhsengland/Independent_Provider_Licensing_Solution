using Database.Repositories.Orchestrate;
using Database.Repositories.User;
using Domain.Logic.Features.Team;
using Domain.Logic.Features.Team.InviteUser;
using Domain.Logic.Features.Team.ManageUsers;
using Domain.Logic.Features.Team.ManageUsers.Queries;
using Domain.Logic.Features.Team.Maps;
using Domain.Logic.Integrations.Session;
using Domain.Objects;
using Domain.Objects.ViewModels.Team;
using Licence.Holder.Application.Models.Team;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Licence.Holder.Application.Controllers;

[Authorize]
public class TeamAddController(
    ILogger<TeamAddController> logger,
    IManageUsersHandler manageUsersViewModelHandler,
    ITeamManagementHandler settingsControllerHandler,
    IInviteNewUserHandler inviteNewUserHandler,
    IAddUserHandler addUserHandler,
    IRepositoryOrchestrator repositoryOrchestrator,
    IRepositoryForUser repositoryForUser,
    IAccessLevelMapper accessLevelMapper,
    ISessionOrchestration sessionOrchestration) : BaseController(sessionOrchestration)
{
    private readonly ILogger<TeamAddController> logger = logger;
    private readonly IManageUsersHandler manageUsersViewModelHandler = manageUsersViewModelHandler;
    private readonly ITeamManagementHandler settingsControllerHandler = settingsControllerHandler;
    private readonly IInviteNewUserHandler inviteNewUserHandler = inviteNewUserHandler;
    private readonly IAddUserHandler addUserHandler = addUserHandler;
    private readonly IRepositoryForUser repositoryForUser = repositoryForUser;
    private readonly IAccessLevelMapper accessLevelMapper = accessLevelMapper;
    private readonly ISessionOrchestration sessionOrchestration = sessionOrchestration;

    [Route("team-management/invite-user")]
    public async Task<IActionResult> InviteUser(
        CancellationToken cancellationToken)
    {
        ClearAddUserSession();

        return View(await inviteNewUserHandler.GetAsync(new InviteUserQuery() { OktaUserId = GetOktaUserId() }, cancellationToken));
    }

    [HttpPost]
    [Route("team-management/invite-user")]
    public IActionResult InviteUser(InviteUserViewModel _)
    {
        return RedirectToAction(nameof(AddUserName));
    }

    [Route("team-management/add-user-name")]
    public IActionResult AddUserName()
    {
        return View(new AddUserNameViewModel()
        {
            FirstName = sessionOrchestration.Get<string>(PageConstants.Session_AddUser_FirstName) ?? string.Empty,
            LastName = sessionOrchestration.Get<string>(PageConstants.Session_AddUser_LastName) ?? string.Empty
        });
    }

    [HttpPost]
    [Route("team-management/add-user-name")]
    public IActionResult AddUserName(AddUserNameViewModel model)
    {
        if (string.IsNullOrWhiteSpace(model.FirstName) || string.IsNullOrWhiteSpace(model.LastName))
        {
            model.ValidationFailure = true;
            return View(model);
        }

        sessionOrchestration.Set(PageConstants.Session_AddUser_FirstName, model.FirstName.Trim());

        sessionOrchestration.Set(PageConstants.Session_AddUser_LastName, model.LastName.Trim());

        var addUserCheck = sessionOrchestration.Get<bool>(PageConstants.Session_AddUser_CheckDetails);

        if (addUserCheck)
        {
            return RedirectToAction(nameof(AddUserCheckDetails));
        }

        return RedirectToAction(nameof(AddUserEmail));
    }

    [Route("team-management/add-user-email")]
    public IActionResult AddUserEmail()
    {
        return View(new AddUserEmailViewModel()
        {
            Email = sessionOrchestration.Get<string>(PageConstants.Session_AddUser_Email) ?? string.Empty,
            ComparisonEmail = sessionOrchestration.Get<string>(PageConstants.Session_AddUser_Email) ?? string.Empty
        });
    }

    [HttpPost]
    [Route("team-management/add-user-email")]
    public async Task<IActionResult> AddUserEmailAsync(
        AddUserEmailViewModel model,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(model.Email) 
            || string.IsNullOrWhiteSpace(model.ComparisonEmail)
            || !model.Email.Contains('@')
            || !model.ComparisonEmail.Contains('@'))
        {
            model.ValidationFailure = true;
            return View(model);
        }

        if (model.Email != model.ComparisonEmail)
        {
            model.EmailDoesNotMatch = true;
            model.ValidationFailure = true;
            return View(model);
        }

        if (await addUserHandler.IsEmailInUse(model.Email, cancellationToken))
        {
            model.EmailInUse = true;
            model.ValidationFailure = true;
            return View(model);
        }

        if (addUserHandler.IsEmailDomainBlackListed(model.Email, cancellationToken))
        {
            model.EmailInBlackList = true;
            model.ValidationFailure = true;
            return View(model);
        }

        sessionOrchestration.Set(PageConstants.Session_AddUser_Email, model.Email.Trim());

        var addUserCheck = sessionOrchestration.Get<bool>(PageConstants.Session_AddUser_CheckDetails);

        if (addUserCheck)
        {
            return RedirectToAction(nameof(AddUserCheckDetails));
        }

        return RedirectToAction(nameof(AddUserAccessLevel));
    }

    [Route("team-management/add-user-access-level")]
    public async Task<IActionResult> AddUserAccessLevelAsync(
        CancellationToken cancellationToken)
    {
        if (!await repositoryOrchestrator.UserAllowedToSetAccessLevel(GetOktaUserId(), cancellationToken))
        {
            sessionOrchestration.Set(PageConstants.Session_AddUser_AccessLevel, PageConstants.No);

            return RedirectToAction(nameof(AddUserCheckDetails));
        }

        return View(new AddUserAccessLevelViewModel()
        {
            AccessLevel = sessionOrchestration.Get<string>(PageConstants.Session_AddUser_AccessLevel) ?? string.Empty
        });
    }

    [HttpPost]
    [Route("team-management/add-user-access-level")]
    public IActionResult AddUserAccessLevel(AddUserAccessLevelViewModel model)
    {
        if (string.IsNullOrWhiteSpace(model.AccessLevel))
        {
            model.ValidationFailure = true;
            return View(model);
        }

        sessionOrchestration.Set(PageConstants.Session_AddUser_AccessLevel, model.AccessLevel.Trim());

        return RedirectToAction(nameof(AddUserCheckDetails));
    }

    [Route("team-management/add-user-check-details")]
    public async Task<IActionResult> AddUserCheckDetailsAsync(
        CancellationToken cancellationToken)
    {
        sessionOrchestration.Set(PageConstants.Session_AddUser_CheckDetails, true);

        var firstname = sessionOrchestration.Get<string>(PageConstants.Session_AddUser_FirstName);
        var lastname = sessionOrchestration.Get<string>(PageConstants.Session_AddUser_LastName);

        if (string.IsNullOrWhiteSpace(firstname) || string.IsNullOrWhiteSpace(lastname))
        {
            logger.LogError("Failed to get first name and last name from session {oktaId}", GetOktaUserId());
            return RedirectToAction(nameof(AddUserName));
        }

        var email = sessionOrchestration.Get<string>(PageConstants.Session_AddUser_Email);

        if (string.IsNullOrWhiteSpace(email))
        {
            logger.LogError("Failed to get email from session {oktaId}", GetOktaUserId());
            return RedirectToAction(nameof(AddUserEmail));
        }

        var accessLevel = sessionOrchestration.Get<string>(PageConstants.Session_AddUser_AccessLevel);

        if (string.IsNullOrWhiteSpace(accessLevel))
        {
            logger.LogError("Failed to get accessLevel from session {oktaId}", GetOktaUserId());
            return RedirectToAction(nameof(AddUserAccessLevel));
        }

        return View(new AddUserCheckDetails()
        {
            FirstName = firstname,
            LastName = lastname,
            Email = email,
            AccessLevel = accessLevel,
            ShowAccessLevel = await repositoryOrchestrator.UserAllowedToSetAccessLevel(GetOktaUserId(), cancellationToken)
        });
    }

    [HttpPost]
    [Route("team-management/add-user-check-details")]
    public async Task<IActionResult> AddUserCheckDetails(
        AddUserCheckDetails _,
        CancellationToken cancellationToken)
    {
        var firstname = sessionOrchestration.Get<string>(PageConstants.Session_AddUser_FirstName);
        var lastname = sessionOrchestration.Get<string>(PageConstants.Session_AddUser_LastName);

        if (string.IsNullOrWhiteSpace(firstname) || string.IsNullOrWhiteSpace(lastname))
        {
            logger.LogError("Failed to get first name and last name from session {oktaId}", GetOktaUserId());
            return RedirectToAction(nameof(AddUserName));
        }

        var email = sessionOrchestration.Get<string>(PageConstants.Session_AddUser_Email);

        if (string.IsNullOrWhiteSpace(email))
        {
            logger.LogError("Failed to get email from session {oktaId}", GetOktaUserId());
            return RedirectToAction(nameof(AddUserEmail));
        }

        var accessLevel = sessionOrchestration.Get<string>(PageConstants.Session_AddUser_AccessLevel);

        if (string.IsNullOrWhiteSpace(accessLevel))
        {
            logger.LogError("Failed to get access level from session {oktaId}", GetOktaUserId());
            return RedirectToAction(nameof(AddUserAccessLevel));
        }

        var role = accessLevelMapper.MapToUserRole(accessLevel);

        var organisation = await repositoryOrchestrator.GetOrganisationAsync(GetOktaUserId(), cancellationToken);

        var requestorId = await repositoryForUser.GetIdAsync(GetOktaUserId(), cancellationToken);

        await addUserHandler.Create(new NewUserRequest()
        {
            Firstname = firstname,
            Lastname = lastname,
            Email = email,
            UserRole = role,
            OrganisationId = organisation.Id,
            RequestedById = requestorId
        }, cancellationToken);

        ClearAddUserSession();

        return RedirectToAction(nameof(AddUserConfirmation));
    }

    [Route("team-management/add-user-confirmation")]
    public IActionResult AddUserConfirmation()
    {
        return View();
    }

    [Route("team-management/add-user-cancel")]
    public IActionResult AddUserCancel()
    {
        ClearAddUserSession();

        return RedirectToAction(nameof(TeamController.Index), "Team");
    }

    [Route("team-management/invite-user-feedback")]
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
        return await manageUsersViewModelHandler.UsersAreInTheSameOrganisation(new AllowedToEditUserQuery() { OktaUserId = GetOktaUserId(), UserId = id }, cancellationToken);
    }

    private void ClearAddUserSession()
    {
        sessionOrchestration.Remove(PageConstants.Session_AddUser_FirstName);
        sessionOrchestration.Remove(PageConstants.Session_AddUser_LastName);
        sessionOrchestration.Remove(PageConstants.Session_AddUser_Email);
        sessionOrchestration.Remove(PageConstants.Session_AddUser_AccessLevel);
        sessionOrchestration.Remove(PageConstants.Session_AddUser_CheckDetails);
    }
}
