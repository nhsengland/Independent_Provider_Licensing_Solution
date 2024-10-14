using Domain.Logic.Forms.Application.Models;
using Domain.Models.Database;
using Domain.Models.Database.DTO;
using Domain.Models.Forms.Application;
using Domain.Models.Forms.Rules;
using Domain.Models.ViewModels.Application;

namespace Domain.Logic.Forms.Application;
public interface IApplicationOrchestration
{
    Task<int> CreateUltimateContorller(int applicationId);

    Task DeleteUltimateContorller(int applicationId, int ultimateControllerId);

    RuleOutcomeDTO EvaluateLastFinancialYearEnd(
        int? day,
        int? month,
        int? year);

    RuleOutcomeDTO EvaluateNextFinancialYearEnd(
        int? day,
        int? month,
        int? year);

    Task<RuleOutcomeDTO> OrchestrateLastFinancialYearEndAsync(
        int id,
        ApplicationDateDTO input);

    Task<RuleOutcomeDTO> OrchestrateNextFinancialYearEndAsync(
        int id,
        ApplicationDateDTO input);

    Task<List<UltimateControllerDTO>> GetUltimateContorllers(int applicationId);

    Task<string> GetApplicationCode(int id);

    Task<ApplicationReviewData> GetApplicationReviewData(int id);

    Task<ReviewApplicationResponsesViewModel> GetApplicationResponses(int id);

    Task<ApplicationPage> GetCurrentPage(int id);

    Task<string> GetCompanyNumber(int id);

    Task<string> GetCompanyNumberCheck(int id);

    Task<string> GetDirectorsCheck(int id);

    Task<string> GetCorporateDirectorsCheck(int id);

    Task<string> GetContactEmailAddress(int id);

    Task<string> GetCQCProviderAddress(int id);

    Task<CQCProviderDetailsDTO> GetCQCProviderDetails(int id);

    Task<(bool exists, CQCProviderDetailsDTO providerDetails)> GetCQCProviderDetailsIfExists(string id);

    Task<string> GetCQCProviderID(int id);

    Task<string> GetCQCProviderName(int id);

    Task<string> GetCQCProviderPhoneNumber(int id);

    Task<string> GetCQCProviderWebsiteURL(int id);

    Task<string> GetDirectorsSatisfyG3FitAndProperRequirements(int id);

    Task<string> GetDirectorsSatisfyG3FitAndProperRequirementsIfNoWhy(int id);

    Task<ApplicationDTO> GetApplicationDTO(int id);
    Task<string> GetNewlyIncorporatedCompany(int id);

    Task<string> GetOneOrMoreParentCompanies(int id);

    Task<string> GetReferenceId(int id);

    Task<bool> GetSubmitApplication(int id);

    Task<string> GetUltimateController(int id);

    Task<string> GetUltimateControllerName(int applicationId, int ultimateControllerId);

    Task<ApplicationDateDTO> GetFinancialYearEndLastFromSessionOrDatabase(int id);

    Task<ApplicationDateDTO> GetFinancialYearEndNextFromSessionOrDatabase(int id);

    Task<bool> IsCrsOrHardToReplace(int id);

    DateOnly? EvaluateDate(string date);

    Task<(bool providerExists, bool providerHasActiveLicence)> OrchestrateCQCProvider(int id, string cqcProviderId);

    Task<string> SaveAndExitAsync(
        int id,
        string applicationURL);

    Task SeedApplication(
        int applicationId,
        int? preApplicationId,
        int applicationCodeId);

    Task SetCurrentPage(int id, Domain.Models.Database.ApplicationPage applicationPage);

    Task SetContactDetails(int id, ContactDetailsDTO contactDetails);

    Task SetDirectorsCheck(int id, string response);

    Task SetCorporateDirectorsCheck(int id, string response);

    Task SetCompanyNumber(int id, string response);

    Task SetCompanyNumberCheck(int id, string response);

    Task SetCQCProviderAddress(int id, string response);

    Task SetCQCProviderDetails(int id, CQCProviderDetailsWithoutIdDTO details);

    Task SetCQCProviderID(int id, string response);

    Task SetCQCProviderName(int id, string response);

    Task SetCQCProviderPhoneNumber(int id, string response);

    Task SetCQCProviderWebsiteURL(int id, string response);

    Task SetDirectorsSatisfyG3FitAndProperRequirements(int id, string response);

    Task SetDirectorsSatisfyG3FitAndProperRequirementsIfNoWhy(int id, string response);

    Task SetLastFinancialYear(int id, DateOnly? date);

    Task SetNewlyIncorporatedCompany(int id, string response);

    Task SetNextFinancialYear(int id, DateOnly? date);

    Task SetOneOrMoreParentCompanies(int id, string response);

    Task SetUltimateController(int id, string response);

    Task SetUltimateControllerName(int applicationId, int ultimateControllerId, string response);

    Task SubmitApplication(int id);

    Task<Domain.Models.Database.ApplicationPage> OrchestrateNextPagePostDirectorCheck(
        int id,
        string directorCheck,
        bool atReviewStage);

    Domain.Models.Database.ApplicationPage DetermineNextPageAfterParentCompaniesCheck(string response, bool reviewPage);

    Task<Domain.Models.Database.ApplicationPage> DetermineNextPageAfterDirectorsSection(int id);

    Task<ApplicationCodeOrchestrationResult> OrchestrateApplicationCodeAsync(string code, string userId);

    Task<bool> UltimateControllerExists(int applicationId, int ultimateControllerId);
}
