namespace Domain.Logic.Forms.PreApplication.DTO;
public class PreApplicationDTO
{
    public string CQCProviderID { get; set; } = default!;
    public string CQCProviderName { get; set; } = default!;
    public string CQCProviderAddress { get; set; } = default!;
    public string CQCProviderPhoneNumber { get; set; } = default!;
    public bool IsHealthCareService { get; set; } = default!;
    public bool ConfirmationOfRegulatedActivities { get; set; } = default!;
    public bool IsExclusive { get; set; } = default!;
    public string Turnover { get; set; } = default!;
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string JobTitle { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;
}
