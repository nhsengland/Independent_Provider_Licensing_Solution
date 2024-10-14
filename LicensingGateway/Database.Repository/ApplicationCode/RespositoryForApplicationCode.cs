using Database.Repository.Core.ReadWrite;
using Domain.Models.Database.DTO;
using Microsoft.EntityFrameworkCore;

namespace Database.Repository.ApplicationCode;
public class RespositoryForApplicationCode(ILicensingGatewayDbContext licensingGatewayDbContext) : ReadWriteIntPkRepository<Entites.ApplicationCode>(licensingGatewayDbContext), IRespositoryForApplicationCode
{
    public async Task<ApplicationCodeDTO?> GetApplicationCodeDetailsAsync(
        string code,
        string userId)
    {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        var entity = await licensingGatewayDbContext.ApplicationCode.Where(e => e.Code == code && e.OktaUserId == userId)
            .Include(e => e.Application)
            .ThenInclude(e => e.CurrentPage)
            .FirstOrDefaultAsync();
#pragma warning restore CS8602 // Dereference of a possibly null reference.

        if (entity == null)
        {
            return null;
        }

        return new ApplicationCodeDTO
        {
            ApplicationCodeId = entity.Id,
            CurrentPage = entity.Application?.CurrentPage?.PageName,
            ApplicationId = entity.Application?.Id,
            PreApplicationId = entity.PreApplicationId,
            IsHardToFindOrHardToReplace = entity.IsHardToReplace || entity.IsCRS,
        };
    }

    public async Task<int?> GetPreApplicationId(int id)
    {
        return await licensingGatewayDbContext.ApplicationCode.Where(e => e.Id == id).Select(e => e.PreApplicationId).FirstOrDefaultAsync();
    }

    public async Task<bool> IsCrsOrHardToReplace(int id)
    {
        return await licensingGatewayDbContext.ApplicationCode.Where(e => e.Id == id && (e.IsCRS == true || e.IsHardToReplace == true))
            .AnyAsync();
    }
}
