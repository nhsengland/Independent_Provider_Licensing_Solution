using Database.Entites.Core;

namespace Database.Repositories.Core;

public interface IReadRepository<TEntity> : IReadRepository<TEntity, int>
    where TEntity : class, IBaseIdentity<int>
{
}

public interface IReadRepository<TEntity, TEntityId>
        where TEntity : class, IBaseIdentity<TEntityId>
        where TEntityId : struct
{
    Task<TEntity?> GetByIdAsync(TEntityId id, CancellationToken cancellationToken = default);

    Task<List<TEntity>> GetAllAsync();

    Task<bool> CanConnectAsync();
}
