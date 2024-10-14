using Domain.Objects.ViewModels.Team;

namespace Domain.Logic.Features.Team.InviteUser;

public interface IInviteNewUserHandler
{
    Task<InviteUserViewModel> GetAsync(InviteUserQuery query, CancellationToken cancellationToken);
}
