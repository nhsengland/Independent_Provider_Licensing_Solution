using Domain.Logic.Features.Team.ManageUsers.Queries;
using Domain.Objects.Database;
using Domain.Objects.ViewModels.Team;

namespace Domain.Logic.Features.Team.ManageUsers;

public interface IManageUsersHandler
{
    Task<ManageUserViewModel> Get(
        GetManageUserViewModelQuery request,
        CancellationToken cancellationToken);

    Task<ManageUsersViewModel> Get(
        GetManageUsersViewModelQuery request,
        CancellationToken cancellationToken);

    Task<ManageUserChangeAccessLevelViewModel> Get(
        GetManageUserChangeAccessLevelViewModel request,
        CancellationToken cancellationToken);

    Task RemoveOktaUserFromGroup(int userId, CancellationToken cancellationToken);

    Task UpdateUserAccessLevel(int userId, UserRole userRole, CancellationToken cancellationToken);

    Task<bool> UsersAreInTheSameOrganisation(
        AllowedToEditUserQuery request,
        CancellationToken cancellationToken);
}
