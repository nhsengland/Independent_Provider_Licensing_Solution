using Database.Entites.Core;
using static Database.Repository.Core.Read.IReadGuidPkRepository;

namespace Database.Repository.Core.ReadWrite;
public interface IReadWriteGuidRepository<TEntity> : IReadWriteGuidRepository<TEntity, Guid>, IReadGuidPkRepository<TEntity>
       where TEntity : class, IEntity<Guid>
{
}

public interface IReadWriteGuidRepository<TEntity, TEntityId>
    where TEntity : class, IEntity<TEntityId>
    where TEntityId : struct
{
    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);

    Task AddAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

    Task<TEntity> AquireAsync(Guid guid, CancellationToken cancellationToken = default);

    Task DeleteAsync(Guid guid, CancellationToken cancellationToken = default);

    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
