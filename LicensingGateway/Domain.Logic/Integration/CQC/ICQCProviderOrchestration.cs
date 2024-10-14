
using Domain.Logic.Integration.CQC.API.Models;
using Domain.Logic.Integration.CQC.Models;

namespace Domain.Logic.Integration.CQC;
public interface ICQCProviderOrchestration
{
    Task<string[]> GetProvidersRequlatedActivites(string cqcProviderId);

    Task<CQCProviderInformation?> GetProviderInformation(string CQCProviderID);

    Task CreateInstanceRecord(Guid guid);

    Task DeleteInstanceRecord(Guid guid);

    Task<ProvidersOutputModel> CQCGetAllProviders(ProvidersInputModel inputModel);

    Task<ProvidersThatHaveChangedOutputModel> GetProvidersThatHaveChanged(ProvidersInputModel inputModel);

    Task<string[]> GetProviderIdsForThisImport(ProvidersInputModel inputModel);

    Task ImportProvider(ProviderImportInputModel inputModel);
}
