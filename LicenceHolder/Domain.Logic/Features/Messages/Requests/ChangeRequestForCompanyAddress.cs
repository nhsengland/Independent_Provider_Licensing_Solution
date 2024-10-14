using Domain.Objects.Database;

namespace Domain.Logic.Features.Messages.Requests;

public record ChangeRequestForCompanyAddress
{
    public int OrganisationId { get; init; }
    public string CompanyName { get; init; } = default!;
    public string LicenseNumber { get; init; } = default!;
    public string RequestorName { get; init; } = default!;
    public DateTime RequestedOn { get; init; }
    public string PreviousAddress { get; init; } = default!;
    public string NewAddress { get; init; } = default!;
}
