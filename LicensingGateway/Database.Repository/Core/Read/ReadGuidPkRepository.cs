using Database.Entites.Core;
using Microsoft.EntityFrameworkCore;
using static Database.Repository.Core.Read.IReadGuidPkRepository;

namespace Database.Repository.Core.Read;
public abstract class ReadGuidPkRepository<TEntity, TEntityId>(ILicensingGatewayDbContext dbContext) : IReadGuidPkRepository<TEntity, TEntityId>
        where TEntity : class, IEntity<TEntityId>
        where TEntityId : struct
{
    protected readonly DbContext rootDBContext = dbContext.DbContext;

    protected readonly ILicensingGatewayDbContext licensingGatewayDbContext = dbContext;

    public async Task<TEntity?> GetByIdAsync(TEntityId id, CancellationToken cancellationToken = default)
    {
        return await rootDBContext.Set<TEntity>().FindAsync([id, cancellationToken], cancellationToken: cancellationToken);
    }
}
