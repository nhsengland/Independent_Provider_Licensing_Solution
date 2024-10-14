using Database.Contexts;
using Database.Repositories.Core;
using Domain.Objects.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Database.Repositories.EmailNotification;

public class RepositoryForEmailNotification(LicenceHolderDbContext dbContext) : ReadWriteRepository<Entites.EmailNotification>(dbContext), IRepositoryForEmailNotification
{
    public async Task<DateTime?> GetDateOfLatestEmailNotification(int userId, CancellationToken cancellationToken)
    {
        return await dbContext.EmailNotification
            .Where(e => e.UserId == userId)
            .OrderByDescending(e => e.DateCreated)
            .Select(e => e.DateCreated)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);
    }

    public async Task<bool> HasBeenSent(int id, CancellationToken cancellationToken)
    {
        return await dbContext.EmailNotification.AnyAsync(e => e.Id == id && e.HasBeenSent, cancellationToken: cancellationToken);
    }

    public async Task MarkAsSent(int id, CancellationToken cancellationToken)
    {
        await dbContext.EmailNotification.Where(e => e.Id == id)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(b => b.DateSent, DateTime.UtcNow)
                .SetProperty(b => b.HasBeenSent, true)
, cancellationToken: cancellationToken);
    }

    public async Task<string> RequestedByFullName(int id, CancellationToken cancellationToken)
    {
        return await dbContext.EmailNotification
            .Where(e => e.Id == id)
            .Select(e => $"{e.RequestedBy!.Firstname} {e.RequestedBy.Lastname}")
            .FirstOrDefaultAsync(cancellationToken: cancellationToken) ?? throw new NotFoundException($"Email notification doesn't exist: {id}");
    }
}
