using Domain.Objects.ViewModels.Team;

namespace Domain.Logic.Features.Team;

public interface ITeamManagementHandler
{
    Task<TeamManagementViewModel> Get(TeamManagementRequest request, CancellationToken cancellationToken);
}
