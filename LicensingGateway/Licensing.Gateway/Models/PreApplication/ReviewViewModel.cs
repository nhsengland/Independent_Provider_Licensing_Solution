namespace Licensing.Gateway.Models.PreApplication;

public class ReviewViewModel : ApplicationBase
{
    public string IsCQCRegistered { get; set; } = default!;

    public string CQCProviderID { get; set; } = default!;

    public string CQCProviderInformation_Name { get; set; } = default!;

    public string CQCProviderInformation_Address { get; set; } = default!;

    public string CQCProviderInformation_PhoneNumber { get; set; } = default!;

    public string CQCInformationIsCorrect { get; set; } = default!;

    public string ProvidesHealthCareService { get; set; } = default!;

    public string CQCRegulatedActivites { get; set; } = default!;

    public string ExclusiveServices { get; set; } = default!;

    public string Earnings { get; set; } = default!;

    public string ContactDetails_Forename { get; set; } = default!;

    public string ContactDetails_Surname { get; set; } = default!;

    public string ContactDetails_JobTitle { get; set; } = default!;

    public string ContactDetails_Email { get; set; } = default!;

    public string ContactDetails_PhoneNumber { get; set; } = default!;
}
