using Database.Entites.Core;
using static Database.Repository.Core.Read.IReadIntPkRepository;

namespace Database.Repository.Core.ReadWrite;
public interface IReadWriteIntPkRepository<TEntity> : IReadWriteIntPkRepository<TEntity, int>, IReadIntPkRepository<TEntity>
       where TEntity : class, IEntity<int>
{
}

public interface IReadWriteIntPkRepository<TEntity, TEntityId>
    where TEntity : class, IEntity<TEntityId>
    where TEntityId : struct
{
    Task<TEntityId> AddAsync(TEntity entity, CancellationToken cancellationToken = default);

    Task Add(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

    Task DeleteAsync(int id, CancellationToken cancellationToken = default);

    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
