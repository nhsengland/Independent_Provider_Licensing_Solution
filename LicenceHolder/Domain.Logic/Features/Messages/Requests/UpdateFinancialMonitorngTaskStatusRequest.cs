
namespace Domain.Logic.Features.Messages.Requests;

public record UpdateFinancialMonitorngTaskStatusRequest
{
    public int OrganisationId { get; init; }
    public string RequestorName { get; init; } = default!;
    public DateTime RequestedOn { get; init; }
}
