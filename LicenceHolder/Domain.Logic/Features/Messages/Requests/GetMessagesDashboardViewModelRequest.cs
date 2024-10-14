using Domain.Objects.Database;

namespace Domain.Logic.Features.Messages.Requests;

public record GetMessagesDashboardViewModelRequest
{
    public string UserOktaId { get; init; } = default!;

    public int PageNumber { get; set; }

    public int OrganisationId { get; set; }

    public MessageType Type { get; set; }
}
