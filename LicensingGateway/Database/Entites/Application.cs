using System.ComponentModel.DataAnnotations;
using Database.Entites.Core;

namespace Database.Entites;
public class Application : BaseIntEntity
{
    public DateTime DateGenerated { get; set; }

    public DateTime DateModified { get; set; }

    public int ApplicationCodeId { get; set; } = default!;

    [Required]
    public ApplicationCode ApplicationCode { get; set; } = null!;

    public int CurrentPageId { get; set; } = default!;

    [Required]
    public ApplicationPage CurrentPage { get; set; } = default!;

    public string? CQCProviderID { get; set; } = default!;

    public string? CQCProviderName { get; set; } = default!;

    public string? CQCProviderAddress { get; set; } = default!;

    public string? CQCProviderPhoneNumber { get; set; } = default!;

    public string? CQCProviderWebsiteURL { get; set; } = default!;

    public string? CompanyNumber { get; set; } = default!;

    public bool? CompanyNumberCheck { get; set; } = default!;

    public bool? DirectorsCheck { get; set; } = default!;

    public bool? CorporateDirectorsCheck { get; set; } = default!;

    public string? Forename { get; set; } = default!;

    public string? Surname { get; set; } = default!;

    public string? JobTitle { get; set; } = default!;

    [MaxLength(254)]
    public string? Email { get; init; } = default!;

    public bool ElectronicCommunications { get; init; }

    public bool SubmitApplication { get; init; } = default!;

    [Length(5, 100)]
    public string? ReferenceId { get; set; } = default!;

    public bool? UltimateController { get; set; } = default!;

    public bool? DirectorsSatisfyG3FitAndProperRequirements { get; set; } = default!;

    public string? DirectorsSatisfyG3FitAndProperRequirementsIfNoWhy { get; set; } = default!;

    public bool? NewlyIncorporatedCompany { get; set; } = default!;

    public DateOnly? LastFinancialYear { get; set; } = default!;

    public DateOnly? NextFinancialYear { get; set; } = default!;

    public bool? OneOrMoreParentCompanies { get; set; } = default!;
}
