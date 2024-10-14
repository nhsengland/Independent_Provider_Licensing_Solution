using Database.Entites;
using Database.Repository.Core.ReadWrite;
using Microsoft.EntityFrameworkCore;

namespace Database.Repository.CQC;
public class RepositoryForCQCProviderImportPage(
    ILicensingGatewayDbContext licensingGatewayDbContext,
    IRespositoryForCQCProviderImportInstance respositoryForCQCProviderImportInstance) : ReadWriteIntPkRepository<CQCProviderImportPage>(licensingGatewayDbContext), IRepositoryForCQCProviderImportPage
{
    private readonly IRespositoryForCQCProviderImportInstance respositoryForCQCProviderImportInstance = respositoryForCQCProviderImportInstance;

    public async Task AddAsync(Guid guid, int pageId, string ProviderId)
    {
        var instance = await GetInstance(guid);

        await licensingGatewayDbContext.CQCProviderImportPage.AddAsync(new CQCProviderImportPage
        {
            CQCProviderImportInstance = instance,
            CQCProviderID = ProviderId,
            PageNumber = pageId
        });

        await rootDBContext.SaveChangesAsync();
    }

    public async Task<string[]> GetProviderIdsForImport(Guid guid, int pageNumber)
    {
        var instance = await GetInstance(guid);

        return await licensingGatewayDbContext.CQCProviderImportPage.Where(e => e.CQCProviderImportInstance == instance && e.PageNumber == pageNumber).Select(e => e.CQCProviderID).ToArrayAsync();
    }

    public async Task Update(int status, string id, Guid guid)
    {
        var instance = await GetInstance(guid);

        var entity = await licensingGatewayDbContext.CQCProviderImportPage.Where(e => e.CQCProviderImportInstance == instance && e.CQCProviderID == id).FirstOrDefaultAsync();

        if (entity == null)
        {
            throw new Exception($"Entity not found - id: {id}, guid: {guid}");
        }

        entity.StatusCode = status;

        entity.NumberOfAttemptsToImport++;

        await licensingGatewayDbContext.DbContext.SaveChangesAsync();
    }

    private async Task<CQCProviderImportInstance> GetInstance(Guid guid)
    {
        var instance = await respositoryForCQCProviderImportInstance.GetByIdAsync(guid);

        if (instance == null)
        {
            throw new Exception($"Instance not found: {guid}");
        }

        return instance;
    }
}
