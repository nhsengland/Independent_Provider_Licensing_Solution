namespace Domain.Logic.Features.Team.ManageUsers.Queries;

public record GetManageUsersViewModelQuery
{
    public string OktaUserId { get; init; } = default!;
}
