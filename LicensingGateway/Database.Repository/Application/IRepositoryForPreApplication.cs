using Database.Entites;
using Database.Repository.Core.ReadWrite;

namespace Database.Repository.Application;
public interface IRepositoryForPreApplication : IReadWriteIntPkRepository<PreApplication>
{
    Task UpdateAsync(int id, string referenceId);
}
