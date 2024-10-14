using Database.Entites.Core;
using Database.Repository.Core.Read;

namespace Database.Repository.Core.ReadWrite;
public abstract class ReadWriteGuidPkRepository<TEntity>(ILicensingGatewayDbContext dbContext) : ReadGuidPkRepository<TEntity, Guid>(dbContext), IReadWriteGuidRepository<TEntity, Guid>
        where TEntity : class, IEntity<Guid>
{
    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        rootDBContext.Set<TEntity>().Add(entity);

        await rootDBContext.SaveChangesAsync(cancellationToken);
    }

    public async Task AddAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        rootDBContext.Set<TEntity>().AddRange(entities);

        await rootDBContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<TEntity> AquireAsync(Guid guid, CancellationToken cancellationToken = default)
    {
        var entity = await rootDBContext.FindAsync<TEntity>(guid);

        if(entity == null)
        {
            await rootDBContext.AddAsync(guid, cancellationToken);

            await rootDBContext.SaveChangesAsync(cancellationToken);

#pragma warning disable CS8603 // Possible null reference return.
            return await rootDBContext.FindAsync<TEntity>(guid);
#pragma warning restore CS8603 // Possible null reference return.
        }

        return entity;
    }

    public async Task DeleteAsync(Guid guid, CancellationToken cancellationToken = default)
    {
        var entity = await rootDBContext.FindAsync<TEntity>(guid) ?? throw new EntryPointNotFoundException($"Couldn't find entity with id {guid}");

        rootDBContext.Remove(entity);

        await rootDBContext.SaveChangesAsync(cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await rootDBContext.SaveChangesAsync(cancellationToken);
    }
}
