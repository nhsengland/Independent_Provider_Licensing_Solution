namespace Domain.Logic.Features.Messages.Requests;

public record ChangeRequestForCompanyName
{
    public int OrganisationId { get; init; }
    public string PreviousCompanyName { get; init; } = default!;
    public string NewCompanyName { get; init; } = default!;
    public string LicenseNumber { get; init; } = default!;
    public string RequestorName { get; init; } = default!;
    public DateTime RequestedOn { get; init; }
}
