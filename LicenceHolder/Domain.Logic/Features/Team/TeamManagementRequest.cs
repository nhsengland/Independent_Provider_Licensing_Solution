namespace Domain.Logic.Features.Team;

public record TeamManagementRequest
{
    public string OktaUserId { get; init; } = default!;
}
