using Database.Entites.Core;
using Database.Repository.Core.Read;

namespace Database.Repository.Core.ReadWrite;
public abstract class ReadWriteIntPkRepository<TEntity>(ILicensingGatewayDbContext dbContext) : ReadIntPkRepository<TEntity, int>(dbContext), IReadWriteIntPkRepository<TEntity, int>
        where TEntity : class, IEntity<int>
{
    public async Task<int> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        rootDBContext.Set<TEntity>().Add(entity);

        await rootDBContext.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }

    public async Task Add(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        rootDBContext.Set<TEntity>().AddRange(entities);

        await rootDBContext.SaveChangesAsync(cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await rootDBContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await rootDBContext.FindAsync<TEntity>(id) ?? throw new EntryPointNotFoundException($"Couldn't find entity with id {id}");

        rootDBContext.Remove(entity);

        await rootDBContext.SaveChangesAsync(cancellationToken);
    }
}
