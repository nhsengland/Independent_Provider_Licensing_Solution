namespace Domain.Logic.Features.Licence.Queries;

public record CompanyFYEChangeRequest
{
    public string UserOktaId { get; init; } = default!;

    public int LicenseId { get; init; }

    public int CompanyId { get; init; }

    public DateOnly FinancialYearEnd { get; init; } = default!;
}
