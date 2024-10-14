namespace Domain.Objects.ViewModels.Licence;

public record LicenceDetailsViewModel
{
    public int CompanyId { get; init; }

    public string CQCProviderID { get; init; } = default!;

    public string CompanyName { get; init; } = default!;

    public int LicenseId { get; init; }

    public string LicenseNumber { get; init; } = default!;

    public string Address { get; init; } = default!;

    public DateOnly FinancialYearEnd { get; init; }

    public string? PublishedLicenceUrl { get; init; }
}
