using Domain.Models.Database;
using Domain.Models.ViewModels.Application;

namespace Domain.Logic.Forms.Application.Page;
public class NextPageOrchestor : INextPageOrchestor
{
    public void EvaluateApplicationResponses(
        ReviewApplicationResponsesViewModel model)
    {
        if (model.IsReviewPage)
        {
            model.Controller = ApplicationControllerConstants.Controller_Name_Application;
            model.Action = ApplicationControllerConstants.Controller_Application_Method_Review;
            return;
        }

        foreach (var detail in model.CompanyDetails)
        {
            if (!detail.IsComplete)
            {
                model.Controller = detail.Controller;
                model.Action = detail.Page.ToString();
                return;
            }
        }

        if (!model.DirectorCheck.IsComplete)
        {
            model.Controller = model.DirectorCheck.Controller;
            model.Action = model.DirectorCheck.Page.ToString();
            return;
        }

        if (!model.CorporateDirectorCheck.IsComplete)
        {
            model.Controller = model.CorporateDirectorCheck.Controller;
            model.Action = model.CorporateDirectorCheck.Page.ToString();
            return;
        }

        if (!model.ParentCompanyDirectorCheck.IsComplete)
        {
            model.Controller = model.ParentCompanyDirectorCheck.Controller;
            model.Action = model.ParentCompanyDirectorCheck.Page.ToString();
            return;
        }

        foreach (var check in model.FinalChecks)
        {
            if (!check.IsComplete)
            {
                model.Controller = check.Controller;
                model.Action = check.Page.ToString();
                return;
            }
        }
    }

    public ApplicationPage NextPageAfterCompanyNumberCheck(
        string response,
        bool sendToReview)
    {
        if (sendToReview)
        {
            return ApplicationPage.Review;
        }

        if (response == ApplicationFormConstants.Yes)
        {
            return ApplicationPage.CompanyNumber;
        }

        return ApplicationPage.NewlyIncorporatedCompany;
    }

    public ApplicationPage NextPageAfterCorporateDirector(
        int numberOfDirectors,
        int numberOfCorporateDirecrors,
        int numberOfCorporateDirecrorGroups)
    {
        if (numberOfCorporateDirecrorGroups > 0 && numberOfCorporateDirecrors == 0)
        {
            return ApplicationPage.ParentCompaniesCheck;
        }

        if (numberOfDirectors > 0 || numberOfCorporateDirecrors > 0)
        {
            return ApplicationPage.DirectorsSatisfyG3FitAndProperRequirements;
        }

        return ApplicationPage.ParentCompaniesCheck;
    }

    public ApplicationPage NextPageAfterCQCProviderConfirmation(
        string response,
        bool sendToReview)
    {
        if (sendToReview)
        {
            return ApplicationPage.Review;
        }

        if (response == ApplicationFormConstants.Yes)
        {
            return ApplicationPage.CompanyNumberCheck;
        }

        return ApplicationPage.CQCProviderChange;
    }

    public ApplicationPage NextPageAfterNewlyIncorporatedCompany(string response, bool sendToReview)
    {
        if (sendToReview)
        {
            return ApplicationPage.Review;
        }

        if (response == ApplicationFormConstants.Yes)
        {
            return ApplicationPage.NextFinancialYear;
        }

        return ApplicationPage.LastFinancialYear;
    }
}
