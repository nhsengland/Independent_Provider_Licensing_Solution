namespace Domain.Logic.Features.Team.Delete.Requests;

public record GetUserDetailsForDeleteUserRequest
{
    public string OktaUserId { get; init; } = default!;

    public int UserID { get; init; }
}
