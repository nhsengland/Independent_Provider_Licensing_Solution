using Database.Entites;
using Database.Repository.Core.ReadWrite;
using Domain.Models.Database.DTO;
using Microsoft.EntityFrameworkCore;

namespace Database.Repository.CQC;
public class RepositoryForCQCProvider(ILicensingGatewayDbContext licensingGatewayDbContext) : ReadWriteIntPkRepository<CQCProvider>(licensingGatewayDbContext), IRepositoryForCQCProvider
{
    public async Task<CQCProviderRegulatedActivity> AquireActivity(string code, string name)
    {
        var activity = await licensingGatewayDbContext.CQCProviderRegulatedActivity.Where(cqc => cqc.Code == code).FirstOrDefaultAsync();

        if (activity == null)
        {
            activity = new CQCProviderRegulatedActivity
            {
                Code = code,
                Name = name
            };

            await licensingGatewayDbContext.CQCProviderRegulatedActivity.AddAsync(activity);

            await SaveChangesAsync();
        }

        return activity;
    }

    public async Task DeleteProvidersRegulatedActivites(int providerId)
    {
        await licensingGatewayDbContext.CQCProviderToRegulatedActivities.Where(cqc => cqc.CQCProviderId == providerId).ExecuteDeleteAsync();
    }

    public async Task<int?> GetIdIfExistsAsync(string providerId)
    {
        var provider = await licensingGatewayDbContext.CQCProvider.Where(x => x.CQCProviderID == providerId).Select(e => new CQCProviderDTO { Id = e.Id }).FirstOrDefaultAsync();

        if (provider == null)
        {
            return null;
        }

        return provider.Id;
    }

    public async Task<string[]> GetProvidersRegulatedActivities(int providerId)
    {
        return await licensingGatewayDbContext.CQCProviderToRegulatedActivities.Where(cqc => cqc.CQCProviderId == providerId).Select(cqc => cqc.Activity.Name).ToArrayAsync();
    }
}
