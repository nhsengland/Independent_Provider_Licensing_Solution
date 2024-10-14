using Database.Repositories.Core;
using Domain.Objects.Database;
using Domain.Objects.Database.DTO;

namespace Database.Repositories.User;

public interface IRepositoryForUser : IReadWriteRepository<Entites.User>
{
    Task Delete(string oktaId, CancellationToken cancellationToken);

    Task<bool> Exists(string oktaId, CancellationToken cancellationToken);

    Task<UserDTO> GetDetails(string oktaId, CancellationToken cancellationToken);

    Task<UserDTOWithEmail> GetDetailsWithEmail(string oktaId, CancellationToken cancellationToken);

    Task<int> GetIdAsync(string oktaId, CancellationToken cancellationToken);

    Task<int> GetOrganisationId(string oktaId, CancellationToken cancellationToken);

    Task<string> GetUserFullName(string oktaId, CancellationToken cancellationToken);

    Task<UserRole> GetUserRole(string oktaId, CancellationToken cancellationToken);

    Task<bool> IsEmailInUse(string email, CancellationToken cancellationToken);

    Task<bool> UpdateUserWhenLoggingInAsync(
        string oktaId,
        string firstname,
        string lastname,
        string email,
        bool emailIsVerified);

    Task<bool> UserExistsInOrganisation(int userId, int organisationId, CancellationToken cancellationToken);
}
