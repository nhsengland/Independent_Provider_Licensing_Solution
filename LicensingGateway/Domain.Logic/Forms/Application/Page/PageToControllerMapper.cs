using Domain.Models.Database;

namespace Domain.Logic.Forms.Application.Page;

public class PageToControllerMapper : IPageToControllerMapper
{
    public string Map(ApplicationPage page)
    {
        return page switch
        {
            ApplicationPage.DirectorCheck => ApplicationControllerConstants.Controller_Name_Director,
            ApplicationPage.Directors => ApplicationControllerConstants.Controller_Name_Director,
            ApplicationPage.DirectorsSatisfyG3FitAndProperRequirements => ApplicationControllerConstants.Controller_Name_Director,

            ApplicationPage.CorporateDirectorCheck => ApplicationControllerConstants.Controller_Name_CoporateDirector,
            ApplicationPage.CorporateBodies => ApplicationControllerConstants.Controller_Name_CoporateDirector,
            ApplicationPage.CorporateBodyName => ApplicationControllerConstants.Controller_Name_CoporateDirector,
            ApplicationPage.CorporateBodyIndividualsOrCompany => ApplicationControllerConstants.Controller_Name_CoporateDirector,
            ApplicationPage.CorporateDirectors => ApplicationControllerConstants.Controller_Name_CoporateDirector,

            ApplicationPage.ParentCompaniesCheck => ApplicationControllerConstants.Controller_Name_ParentCompanyDirector,
            ApplicationPage.ParentCompanies => ApplicationControllerConstants.Controller_Name_ParentCompanyDirector,
            ApplicationPage.ParentCompanyName => ApplicationControllerConstants.Controller_Name_ParentCompanyDirector,
            ApplicationPage.ParentCompanyDirectors => ApplicationControllerConstants.Controller_Name_ParentCompanyDirector,
            ApplicationPage.ParentCompanyDirectorsOrEquivalents => ApplicationControllerConstants.Controller_Name_ParentCompanyDirector,

            ApplicationPage.UltimateController => ApplicationControllerConstants.Controller_Name_UC,
            ApplicationPage.UltimateControllers => ApplicationControllerConstants.Controller_Name_UC,

            _ => ApplicationControllerConstants.Controller_Name_Application
        };
    }
}
