using Domain.Logic.Features.Team.Delete.Requests;
using Licence.Holder.Application.Models.Team;

namespace Domain.Logic.Features.Team.Delete;

public interface IDeleteUserHandler
{
    Task<DeleteUserDetailsDTO> Get(GetUserDetailsForDeleteUserRequest request, CancellationToken cancellationToken);

    Task<bool> UsersAreInTheSameOrganisation(int userId, string requestorOktaId, CancellationToken cancellationToken);

    Task SoftDeleteUser(string oktaId, int userId, CancellationToken cancellationToken);

    Task<bool> UserEmailIsVerified(int userId, CancellationToken cancellationToken);
}
