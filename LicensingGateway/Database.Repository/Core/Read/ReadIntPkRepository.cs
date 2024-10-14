using Database.Entites.Core;
using Microsoft.EntityFrameworkCore;
using static Database.Repository.Core.Read.IReadIntPkRepository;

namespace Database.Repository.Core.Read;
public abstract class ReadIntPkRepository<TEntity, TEntityId>(ILicensingGatewayDbContext dbContext) : IReadIntPkRepository<TEntity, TEntityId>
        where TEntity : class, IEntity<TEntityId>
        where TEntityId : struct
{
    protected readonly DbContext rootDBContext = dbContext.DbContext;

    protected readonly ILicensingGatewayDbContext licensingGatewayDbContext = dbContext;

    public async Task<bool> CanConnectAsync()
    {
        if (await rootDBContext.Database.CanConnectAsync())
        {
            return true;
        }

        return false;
    }

    public Task<List<TEntity>> GetAll()
    {
        return rootDBContext.Set<TEntity>().ToListAsync();
    }

    public async Task<TEntity?> GetByIdAsync(TEntityId id, CancellationToken cancellationToken = default)
    {
        return await rootDBContext.Set<TEntity>().FindAsync([id, cancellationToken], cancellationToken: cancellationToken);
    }
}
