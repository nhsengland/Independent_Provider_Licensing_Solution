namespace Domain.Logic.Features.Dashboard;

public record GetDashboardViewModelQuery
{
    public string UserId { get; init; } = default!;
}
