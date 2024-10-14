namespace Domain.Logic.Integration.CQC.API.Models;
public record ProviderOutputModel
{
#pragma warning disable IDE1006 // Naming Styles
    public string providerId { get; set; } = default!;
    public string name { get; set; } = default!;
    public string registrationStatus { get; set; } = default!;
    public string postalAddressLine1 { get; set; } = default!;
    public string postalAddressLine2 { get; set; } = default!;
    public string postalAddressTownCity { get; set; } = default!;
    public string region { get; set; } = default!;
    public string postalCode { get; set; } = default!;
    public string? website { get; set; }
    public string? mainPhoneNumber { get; set; }

    public RegulatedActivities[] regulatedActivities { get; init; } = default!;
#pragma warning restore IDE1006 // Naming Styles
}

public record RegulatedActivities()
{
    public string name { get; init; } = default!;
    public string code { get; init; } = default!;
}
