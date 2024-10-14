namespace Domain.Logic.Features.Licence.Queries;

public record CompanyNameChangeRequest
{
    public string UserOktaId { get; init; } = default!;

    public int LicenseId { get; init; }

    public int CompanyId { get; init; }

    public string Name { get; init; } = default!;
}
