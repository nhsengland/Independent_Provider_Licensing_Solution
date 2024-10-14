using Database.Repositories;
using Database.Repositories.User;
using Domain.Objects.ViewModels.Team;

namespace Domain.Logic.Features.Team;

public class TeamManagementHandler(
    IRepositoryForUser repositoryForUser,
    IOrganisationRepository organisationRepository) : ITeamManagementHandler
{
    private readonly IRepositoryForUser repositoryForUser = repositoryForUser;
    private readonly IOrganisationRepository organisationRepository = organisationRepository;

    public async Task<TeamManagementViewModel> Get(TeamManagementRequest query, CancellationToken cancellationToken)
    {
        var userId = await repositoryForUser.GetIdAsync(query.OktaUserId, cancellationToken);

        return new TeamManagementViewModel
        {
            ServiceName = await organisationRepository.GetUsersOrganisationNameAsync(userId, cancellationToken),
            UserRole = await repositoryForUser.GetUserRole(query.OktaUserId, cancellationToken)
        };
    }
}
