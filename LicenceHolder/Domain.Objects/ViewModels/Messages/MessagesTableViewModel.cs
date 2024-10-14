namespace Domain.Objects.ViewModels.Messages;

public record MessagesTableViewModel
{
    public MessagesDashboardViewModel MessagesDashboardViewModel { get; init; } = default!;

    public string ActionName { get; init; } = default!;

    public bool ShowReadIcon { get; init; }
}
