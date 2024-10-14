using Database.Contexts;
using Database.Entites;
using Domain.Objects.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Database.Repositories;
public class CompanyRepository(LicenceHolderDbContext dbContext) : ICompanyRepository
{
    private readonly LicenceHolderDbContext dbContext = dbContext;

    public async Task<Company> GetAsync(int companyId)
    {
        var company = await dbContext.Companies.FirstOrDefaultAsync(a => a.Id == companyId);

        if(company == null)
        {
            throw new NotFoundException($"Company with id {companyId} not found");
        }

        return company;
    }

    public async Task<IList<Company>> GetAllByOrganisationId(int organisationId)
        => await dbContext.Companies
        .Where(a => a.OrganisationId == organisationId)
        .Include(c => c.Tasks.OrderByDescending(t => t.DateDue).Take(1))
        .Include(c => c.Licences)
        .ToListAsync();

    public Task<string> GetCompanytName(int companyId)
    {
        return dbContext.Companies.Where(a => a.Id == companyId).Select(a => a.Name).FirstAsync();
    }

    public Task<bool> OrganisationHasCrsOrHardToReplaceCompanys(int organisationId)
    {
        return dbContext.Companies.AnyAsync(a => a.OrganisationId == organisationId && (a.IsCrs || a.IsHardToReplace));
    }
}
