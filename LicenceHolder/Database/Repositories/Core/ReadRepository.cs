using Database.Contexts;
using Database.Entites.Core;
using Microsoft.EntityFrameworkCore;

namespace Database.Repositories.Core
{
    public abstract class ReadRepository<TEntity, TEntityId>(LicenceHolderDbContext dbContext) : IReadRepository<TEntity, TEntityId>
        where TEntity : class, IBaseIdentity<TEntityId>
        where TEntityId : struct
    {
        protected readonly LicenceHolderDbContext dbContext = dbContext;

        public Task<bool> CanConnectAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<TEntity>> GetAllAsync()
        {
            return dbContext.Set<TEntity>().ToListAsync();
        }

        public async Task<TEntity?> GetByIdAsync(TEntityId id, CancellationToken cancellationToken = default)
        {
            return await dbContext.Set<TEntity>().FindAsync(new object?[] { id, cancellationToken }, cancellationToken: cancellationToken);
        }
    }
}
