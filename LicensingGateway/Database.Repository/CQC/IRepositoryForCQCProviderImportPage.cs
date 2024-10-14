using Database.Entites;
using Database.Repository.Core.ReadWrite;

namespace Database.Repository.CQC;
public interface IRepositoryForCQCProviderImportPage : IReadWriteIntPkRepository<CQCProviderImportPage>
{
    Task AddAsync(Guid guid, int pageId, string ProviderId);

    Task<string[]> GetProviderIdsForImport(Guid guid, int pageNumber);

    Task Update(int status, string id, Guid guid);
}
