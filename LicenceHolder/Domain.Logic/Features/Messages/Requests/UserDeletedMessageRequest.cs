namespace Domain.Logic.Features.Messages.Requests;

public record UserDeletedMessageRequest
{
    public int OrganisationId { get; init; }
    public string RequestorName { get; init; } = default!;
    public string InviteeName { get; init; } = default!;
    public string InviteeEmail { get; init; } = default!;
    public DateTime RequestedOn { get; init; }
}
