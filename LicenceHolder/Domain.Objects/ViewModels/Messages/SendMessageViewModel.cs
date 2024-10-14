namespace Domain.Objects.ViewModels.Messages;

public record SendMessageViewModel
{
    public bool ValidationFailure { get; set; } = false;

    public string Title { get; set; } = default!;

    public string Body { get; set; } = default!;
}
