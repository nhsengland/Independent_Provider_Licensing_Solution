namespace Domain.Logic.Features.Licence.Queries;

public record CompanyAddressChangeRequest
{
    public string UserOktaId { get; init; } = default!;

    public int LicenseId { get; init; }

    public int CompanyId { get; init; }

    public string Address_Line_1 { get; init; } = default!;

    public string Address_Line_2 { get; init; } = default!;

    public string Address_TownOrCity { get; init; } = default!;

    public string Address_County { get; init; } = default!;

    public string Address_Postcode { get; init; } = default!;
}
