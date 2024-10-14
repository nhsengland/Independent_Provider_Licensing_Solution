using Database.Entites;

namespace Database.Repositories;

public interface IOrganisationRepository
{
    Task<Organisation> GetByUserIdAsync(int userId, CancellationToken cancellationToken);

    Task<string> GetUsersOrganisationNameAsync(int userId, CancellationToken cancellationToken);

    Task<string> GetOrganisationNameAsync(int organisationId, CancellationToken cancellationToken);

    Task<List<Entites.User>> GetOrganisationUsersAsync(int userId, CancellationToken cancellationToken);
}
