using Database.Contexts;
using Database.Entites.Core;

namespace Database.Repositories.Core;

public abstract class ReadWriteRepository<TEntity> : ReadRepository<TEntity, int>, IReadWriteRepository<TEntity, int>
        where TEntity : class, IBaseIdentity<int>
{
    protected ReadWriteRepository(LicenceHolderDbContext dbContext)
    : base(dbContext)
    {
    }

    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await dbContext.Set<TEntity>().AddAsync(entity);

        await SaveChangesAsync(cancellationToken);
    }

    public async Task AddAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        await dbContext.Set<TEntity>().AddRangeAsync(entities);

        await SaveChangesAsync(cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
