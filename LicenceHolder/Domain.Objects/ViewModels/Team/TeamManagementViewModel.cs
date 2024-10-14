using Domain.Objects.Database;

namespace Domain.Objects.ViewModels.Team;

public record TeamManagementViewModel
{
    public string ServiceName { get; init; } = default!;

    public UserRole UserRole { get; init; } = default!;
}
