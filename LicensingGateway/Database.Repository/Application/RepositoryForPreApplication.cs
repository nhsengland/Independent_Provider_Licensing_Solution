using Database.Entites;
using Database.Repository.Core.ReadWrite;
using Microsoft.EntityFrameworkCore;

namespace Database.Repository.Application;

public class RepositoryForPreApplication(ILicensingGatewayDbContext licensingGatewayDbContext) : ReadWriteIntPkRepository<PreApplication>(licensingGatewayDbContext), IRepositoryForPreApplication
{
    public async Task UpdateAsync(int id, string referenceId)
    {
        await licensingGatewayDbContext.PreApplication.Where(e => e.Id == id)
            .ExecuteUpdateAsync(b =>
                b.SetProperty(e => e.ReferenceId, referenceId)
            );
    }
}
