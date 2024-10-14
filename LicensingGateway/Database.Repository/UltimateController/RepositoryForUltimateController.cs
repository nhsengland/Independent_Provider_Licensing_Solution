using Database.Repository.Core.ReadWrite;
using Domain.Models.Database.DTO;
using Microsoft.EntityFrameworkCore;

namespace Database.Repository.UltimateController;
public class RepositoryForUltimateController(ILicensingGatewayDbContext licensingGatewayDbContext) : ReadWriteIntPkRepository<Entites.UltimateController>(licensingGatewayDbContext), IRepositoryForUltimateController
{
    public async Task<int> Create(int applicationId)
    {
        var ultimateController = new Entites.UltimateController
        {
            ApplicationId = applicationId,
            Name = string.Empty
        };

        licensingGatewayDbContext.UltimateController.Add(ultimateController);

        await licensingGatewayDbContext.DbContext.SaveChangesAsync();

        return ultimateController.Id;
    }

    public async Task Delete(int applicationId, int ultimateControllerId)
    {
        await licensingGatewayDbContext.UltimateController.Where(e => e.ApplicationId == applicationId && e.Id == ultimateControllerId).ExecuteDeleteAsync();
    }

    public async Task Delete(int applicationId)
    {
        await licensingGatewayDbContext.UltimateController.Where(e => e.ApplicationId == applicationId).ExecuteDeleteAsync();
    }

    public async Task<bool> Exists(int applicationId, int ultimateControllerId)
    {
        return await licensingGatewayDbContext.UltimateController.Where(e => e.ApplicationId == applicationId && e.Id == ultimateControllerId).AnyAsync();
    }

    public async Task<string> Get(int applicationId, int ultimateControllerId)
    {
        var name = await licensingGatewayDbContext.UltimateController.Where(e => e.ApplicationId == applicationId && e.Id == ultimateControllerId)
            .Select(e => e.Name).FirstOrDefaultAsync();

        return name ?? string.Empty;
    }

    public async Task<List<UltimateControllerDTO>> GetAll(int applicationId)
    {
        return await licensingGatewayDbContext.UltimateController.Where(e => e.ApplicationId == applicationId)
            .Select(e => new UltimateControllerDTO() {
                Name = e.Name,
                Id = e.Id
            })
            .ToListAsync();
    }

    public async Task Set(int applicationId, int ultimateControllerId, string name)
    {
        await licensingGatewayDbContext.UltimateController.Where(e => e.ApplicationId == applicationId && e.Id == ultimateControllerId)
            .ExecuteUpdateAsync(setters => setters
                    .SetProperty(e => e.Name, name)
                );
    }
}
