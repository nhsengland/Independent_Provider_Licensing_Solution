namespace Domain.Logic.Features.Messages.Requests;

public record GetMessageViewModelRequest
{
    public string UserOktaId { get; init; } = default!;

    public int NotificationId { get; set; }

    public int OrganisationId { get; set; }
}
