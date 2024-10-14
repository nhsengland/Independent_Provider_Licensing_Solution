using Domain.Logic.Forms.PreApplication.DTO;
using Domain.Logic.Integration.CQC.Models;

namespace Domain.Logic.Forms.PreApplication;

public interface IPreApplicationOrchestration
{
    Task<string[]> GetCQCProviderRequlatedActivites(string cqcProviderId);

    Task<CQCProviderInformation?> OrchestrateQCVProvider(string CQCProviderID);

    List<(string key, string value)> EvaluateContactFormForValidationFailures(
        string forename,
        string surname,
        string jobTitle,
        string phoneNumber,
        string email,
        string emailConfirmation);

    Task<string> SubmitPreApplication(PreApplicationDTO preApplicationDTO);
}
