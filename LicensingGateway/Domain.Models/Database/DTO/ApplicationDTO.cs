namespace Domain.Models.Database.DTO;
public record ApplicationDTO
{
    public string ApplicationCode { get; set; } = default!;

    public string CQCProviderID { get; set; } = default!;

    public string CQCProviderName { get; set; } = default!;

    public string CQCProviderAddress { get; set; } = default!;

    public string CQCProviderPhoneNumber { get; set; } = default!;

    public bool? CompanyNumberCheck { get; set; } = default!;

    public string CompanyNumber { get; set; } = default!;

    public bool? SubmitApplication { get; init; } = default!;

    public string ReferenceId { get; set; } = default!;

    public bool? DirectorsCheck { get; set; } = default!;

    public bool? CorporateDirectorsCheck { get; set; } = default!;

    public bool? UltimateController { get; set; } = default!;

    public bool? DirectorsSatisfyG3FitAndProperRequirements { get; set; } = default!;

    public string DirectorsSatisfyG3FitAndProperRequirements_IfNoWhy { get; set; } = default!;

    public bool? NewlyIncorporatedCompany { get; set; } = default!;

    public DateOnly? LastFinancialYear { get; set; } = default!;

    public DateOnly? NextFinancialYear { get; set; } = default!;
    public bool? OneOrMoreParentCompanies { get; set; } = default!;
}
