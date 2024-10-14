namespace Domain.Objects.ViewModels.Tasks;

public record TaskViewModel
{
    public int Id { get; init; }
    public int OrganisationId { get; init; } 
    public Database.TaskStatus Status { get; init; } = default!;
    public DateOnly DueDate { get; init; } = default!;
    public string? SharePointURL { get; init; } = default!;
}
