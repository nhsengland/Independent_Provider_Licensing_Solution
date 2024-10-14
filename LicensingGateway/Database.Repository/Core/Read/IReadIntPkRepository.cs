using Database.Entites.Core;

namespace Database.Repository.Core.Read;

public interface IReadIntPkRepository
{
    public interface IReadIntPkRepository<TEntity> : IReadIntPkRepository<TEntity, int>
        where TEntity : class, IEntity<int>
    {
    }

    public interface IReadIntPkRepository<TEntity, TEntityId>
        where TEntity : class, IEntity<TEntityId>
        where TEntityId : struct
    {
        Task<TEntity?> GetByIdAsync(TEntityId id, CancellationToken cancellationToken = default);

        Task<List<TEntity>> GetAll();

        Task<bool> CanConnectAsync();
    }
}
