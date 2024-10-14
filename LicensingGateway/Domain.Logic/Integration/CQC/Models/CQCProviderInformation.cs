namespace Domain.Logic.Integration.CQC.Models;
public record CQCProviderInformation
{
    public string Name { get; init; } = default!;
    public string Address { get; init; } = default!;
    public string PhoneNumber { get; init; } = default!;
    public bool HasLicence { get; init; }
}
