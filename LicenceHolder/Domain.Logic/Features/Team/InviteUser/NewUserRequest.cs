using Domain.Objects.Database;

namespace Domain.Logic.Features.Team.InviteUser;

public record NewUserRequest
{
    public string Firstname { get; init; } = default!;
    public string Lastname { get; init; } = default!;
    public string Email { get; init; } = default!;
    public UserRole UserRole { get; init; }
    public int OrganisationId { get; init; }
    public int RequestedById { get; init; }
}
