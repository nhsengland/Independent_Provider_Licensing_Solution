using Database.Entites;
using Domain.Objects.Database.DTO;

namespace Database.Repositories.Orchestrate
{
    public interface IRepositoryOrchestrator
    {
        Task<Organisation> GetOrganisationAsync(string oktaId, CancellationToken cancellationToken);

        Task<OrganisationDTO> GetOrganisationGetDetails(string oktaId, CancellationToken cancellationToken);

        Task<string> GetOrganisationNameAsync(string oktaId, CancellationToken cancellationToken);

        Task<List<Entites.User>> GetUsersInMyOrganisation(string oktaId, CancellationToken cancellationToken);

        Task<bool> RequestingUsersOrgansiationContainsUserWithId(string requestingUserOktaId, int userId, CancellationToken cancellationToken);

        Task<bool> UserAllowedToSetAccessLevel(string oktaId, CancellationToken cancellationToken);
    }
}
