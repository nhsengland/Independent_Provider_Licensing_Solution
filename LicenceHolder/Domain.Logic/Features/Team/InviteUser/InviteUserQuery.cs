namespace Domain.Logic.Features.Team.InviteUser;

public record InviteUserQuery
{
    public string OktaUserId { get; init; } = default!;
}
