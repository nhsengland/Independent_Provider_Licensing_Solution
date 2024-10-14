using Domain.Objects.Integrations.StorageAccounts.Queues;

namespace Domain.Logic.Features.Team.InviteUser;

public interface IAddUserHandler
{

    Task Create(NewUserRequest userDetails, CancellationToken cancellationToken);

    Task CreateOktaUser(CreateOktaUserInputModel input, CancellationToken cancellationToken);

    Task<bool> IsEmailInUse(string email, CancellationToken cancellationToken);

    bool IsEmailDomainBlackListed(string email, CancellationToken cancellationToken);

    Task ReInviteUser(int userId, string requestorOktaUserId, CancellationToken cancellationToken);
}
