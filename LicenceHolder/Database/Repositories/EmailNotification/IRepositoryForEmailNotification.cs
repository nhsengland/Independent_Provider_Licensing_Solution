using Database.Repositories.Core;

namespace Database.Repositories.EmailNotification;

public interface IRepositoryForEmailNotification : IReadWriteRepository<Entites.EmailNotification>
{
    Task<DateTime?> GetDateOfLatestEmailNotification(int userId, CancellationToken cancellationToken);

    Task<bool> HasBeenSent(int id, CancellationToken cancellationToken);

    Task MarkAsSent(int id, CancellationToken cancellationToken);

    Task<string> RequestedByFullName(int id, CancellationToken cancellationToken);
}
