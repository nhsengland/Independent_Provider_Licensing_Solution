using Database.Contexts;
using Database.Entites;
using Microsoft.EntityFrameworkCore;

namespace Database.Repositories;

internal class OrganisationRepository(LicenceHolderDbContext dbContext) : IOrganisationRepository
{
    private readonly LicenceHolderDbContext dbContext = dbContext;

    public async Task<Organisation> GetByUserIdAsync(int userId, CancellationToken cancellationToken)
    {
        var user = await dbContext.Users.Include(u => u.Organisation).FirstAsync(a => a.Id == userId, cancellationToken: cancellationToken);

        return user.Organisation;
    }

    public async Task<string> GetOrganisationNameAsync(int organisationId, CancellationToken cancellationToken)
    {
        return await dbContext.Organisations.Where(o => o.Id == organisationId).Select(o => o.Name).FirstAsync(cancellationToken);
    }

    public async Task<List<Entites.User>> GetOrganisationUsersAsync(int userId, CancellationToken cancellationToken)
    {
        var user = await dbContext.Users.FirstAsync(a => a.Id == userId, cancellationToken);

        return await dbContext.Users.Where(u => u.OrganisationId == user.OrganisationId && u.IsDeleted == false).ToListAsync(cancellationToken);
    }

    public async Task<string> GetUsersOrganisationNameAsync(int userId, CancellationToken cancellationToken)
    {
        var organisationName = await dbContext.Users.Where(u => u.Id == userId).Select(u => u.Organisation.Name).FirstAsync(cancellationToken);

        return organisationName ?? throw new Exception("Organisation not found");
    }
}
