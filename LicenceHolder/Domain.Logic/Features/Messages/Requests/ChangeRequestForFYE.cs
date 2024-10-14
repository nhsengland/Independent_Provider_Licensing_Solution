namespace Domain.Logic.Features.Messages.Requests;

public class ChangeRequestForFYE
{
    public int OrganisationId { get; init; }
    public string CompanyName { get; init; } = default!;
    public string LicenseNumber { get; init; } = default!;
    public string RequestorName { get; init; } = default!;
    public DateTime RequestedOn { get; init; }
    public DateOnly PreviousFinancialYearEnd { get; init; }
    public DateOnly NewFinancialYearEnd { get; init; }
}
