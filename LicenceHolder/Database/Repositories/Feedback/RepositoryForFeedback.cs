using Database.Contexts;
using Database.Repositories.Core;

namespace Database.Repositories.Feedback;

public class RepositoryForFeedback(LicenceHolderDbContext dbContext) : ReadWriteRepository<Entites.Feedback>(dbContext), IRepositoryForFeedback
{
}
