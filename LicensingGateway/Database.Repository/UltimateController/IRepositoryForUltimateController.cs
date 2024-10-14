using Database.Repository.Core.ReadWrite;
using Domain.Models.Database.DTO;

namespace Database.Repository.UltimateController;
public interface IRepositoryForUltimateController : IReadWriteIntPkRepository<Entites.UltimateController>
{
    Task<int> Create(int applicationId);

    Task Delete(int applicationId, int ultimateControllerId);

    Task Delete(int applicationId);

    Task<bool> Exists(int applicationId, int ultimateControllerId);

    Task<string> Get(int applicationId, int ultimateControllerId);

    Task<List<UltimateControllerDTO>> GetAll(int applicationId);

    Task Set(int applicationId, int ultimateControllerId, string name);
}
