using Database.Contexts;
using Database.Repositories.Core;

namespace Database.Repositories.ChangeRequests;

public class RepositoryForChangeRequests(LicenceHolderDbContext dbContext) : ReadWriteRepository<Entites.ChangeRequest>(dbContext), IRepositoryForChangeRequests
{
}
