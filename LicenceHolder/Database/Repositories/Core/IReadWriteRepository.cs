using Database.Entites.Core;

namespace Database.Repositories.Core
{
    public interface IReadWriteRepository<TEntity> : IReadWriteRepository<TEntity, int>, IReadRepository<TEntity>
        where TEntity : class, IBaseIdentity<int>
    {
    }

    public interface IReadWriteRepository<TEntity, TEntityId>
        where TEntity : class, IBaseIdentity<TEntityId>
        where TEntityId : struct
    {
        Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);

        Task AddAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
