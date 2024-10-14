using Database.Entites;
using Database.Repository.Core.ReadWrite;

namespace Database.Repository.CQC;
public interface IRepositoryForCQCProvider : IReadWriteIntPkRepository<CQCProvider>
{
    Task<CQCProviderRegulatedActivity> AquireActivity(string code, string name);

    Task DeleteProvidersRegulatedActivites(int providerId);

    Task<string[]> GetProvidersRegulatedActivities(int providerId);

    Task<int?> GetIdIfExistsAsync(string providerId);
}
