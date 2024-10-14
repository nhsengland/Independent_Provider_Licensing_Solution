
namespace Domain.Logic.Features.Messages.Requests;

public record UpdateAnnualCertificateTaskStatusRequest
{
    public int OrganisationId { get; init; }
    public string LicenceName { get; init; } = default!;
    public string RequestorName { get; init; } = default!;
    public DateTime RequestedOn { get; init; }
}
