using Database.Repository.Core.ReadWrite;
using Domain.Models.Database.DTO;

namespace Database.Repository.ApplicationCode;
public interface IRespositoryForApplicationCode : IReadWriteIntPkRepository<Entites.ApplicationCode>
{
    Task<ApplicationCodeDTO?> GetApplicationCodeDetailsAsync(string code, string userId);

    Task<int?> GetPreApplicationId(int id);

    Task<bool> IsCrsOrHardToReplace(int id);
}
