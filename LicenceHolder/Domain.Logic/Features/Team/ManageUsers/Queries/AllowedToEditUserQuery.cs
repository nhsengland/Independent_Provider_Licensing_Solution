namespace Domain.Logic.Features.Team.ManageUsers.Queries;

public record AllowedToEditUserQuery
{
    public string OktaUserId { get; init; } = default!;

    public int UserId { get; init; }
}
