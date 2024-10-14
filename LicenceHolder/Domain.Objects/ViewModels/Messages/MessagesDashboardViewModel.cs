namespace Domain.Objects.ViewModels.Messages;

public record MessagesDashboardViewModel
{
    public int NumberOfUnreadMessages { get; init; }

    public int CurrentPageNumber { get; init; }

    public int TotalNumberOfPages { get; init; }

    public List<MessageDTO> Messages { get; init; } = default!;

    public string OrganisationName { get; init; } = default!;
}
