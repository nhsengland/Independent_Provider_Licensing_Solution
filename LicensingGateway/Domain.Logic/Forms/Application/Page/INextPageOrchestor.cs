using Domain.Models.ViewModels.Application;

namespace Domain.Logic.Forms.Application.Page;
public interface INextPageOrchestor
{
    void EvaluateApplicationResponses(ReviewApplicationResponsesViewModel model);

    Domain.Models.Database.ApplicationPage NextPageAfterCQCProviderConfirmation(string response, bool sendToReview);

    Domain.Models.Database.ApplicationPage NextPageAfterCompanyNumberCheck(string response, bool sendToReview);

    Domain.Models.Database.ApplicationPage NextPageAfterNewlyIncorporatedCompany(string response, bool sendToReview);

    Domain.Models.Database.ApplicationPage NextPageAfterCorporateDirector(
        int numberOfDirectors,
        int numberOfCorporateDirecrors,
        int numberOfCorporateDirecrorGroups);
}
