namespace Domain.Logic.Features.Licence.Queries;

public record LicenceRequest
{
    public int LicenseId { get; init; }

    public string UserOktaId { get; init; } = default!;
}
