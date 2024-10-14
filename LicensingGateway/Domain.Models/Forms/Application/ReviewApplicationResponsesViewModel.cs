using Domain.Models.Database.DTO;

namespace Domain.Models.ViewModels.Application;

public record ReviewApplicationResponsesViewModel
{
    public List<ReviewApplicationResponseViewModel> CompanyDetails { get; set; } = [];
    public ReviewApplicationResponseViewModel DirectorCheck { get; set; } = default!;
    public List<DirectorDTO> Directors { get; set; } = [];
    public ReviewApplicationResponseViewModel CorporateDirectorCheck { get; set; } = default!;
    public List<DirectorGroupDTO> CorporateDirectorGroups { get; set; } = [];
    public bool ShowParentCompanyDirectorSection { get; set; } = false;
    public ReviewApplicationResponseViewModel ParentCompanyDirectorCheck { get; set; } = default!;
    public List<DirectorGroupDTO> ParentCompanyGroups { get; set; } = [];
    public List<ReviewApplicationResponseViewModel> FinalChecks { get; set; } = [];

    public ReviewApplicationResponseViewModel UltimateControllerCheck { get; set; } = default!;

    public List<UltimateControllerDTO> UltimateControllers { get; set; } = [];

    public string Controller { get; set; } = default!;

    public string Action { get; set; } = default!;

    public bool IsCrsOrHardToReplace { get; set; }

    public bool IsReviewPage { get; set; } = false;
}
