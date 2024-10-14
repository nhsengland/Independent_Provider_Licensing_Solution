using Database.Entites.Core;

namespace Database.Repository.Core.Read;

public interface IReadGuidPkRepository
{
    public interface IReadGuidPkRepository<TEntity> : IReadGuidPkRepository<TEntity, Guid>
        where TEntity : class, IEntity<Guid>
    {
    }

    public interface IReadGuidPkRepository<TEntity, TEntityId>
        where TEntity : class, IEntity<TEntityId>
        where TEntityId : struct
    {
        Task<TEntity?> GetByIdAsync(TEntityId id, CancellationToken cancellationToken = default);
    }
}
