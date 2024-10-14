namespace Domain.Objects.ViewModels.Messages;

public record MessageDTO
{
    public int Id { get; init; }

    public string Title { get; init; } = default!;

    public string Body { get; init; } = default!;

    public string From { get; init; } = default!;

    public string DateSent { get; init; } = default!;

    public DateTime ActualDateSent { get; init; } = default!;

    public bool IsRead { get; init; }
}
