using Database.Contexts;
using Database.Repositories.Core;
using Domain.Objects.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Database.Repositories.License;

public class RepositoryForLicense(LicenceHolderDbContext dbContext) : ReadWriteRepository<Entites.Licence>(dbContext), IRepositoryForLicense
{
    public async Task<string> GetLicenseName(int id)
    {
        var licenceWithCompany = await GetWithCompanyAsync(id) ?? throw new NotFoundException($"License with id {id} not found");

        return licenceWithCompany.Company.Name;
    }

    public async Task<string> GetLicenseNumber(int id)
    {
        var licenseNumber = await dbContext.Licences.Where(l => l.Id == id).Select(l => l.LicenceNumber).FirstOrDefaultAsync();

        return licenseNumber ?? throw new NotFoundException($"License with id {id} not found");
    }

    public async Task<int> GetOrganisationId(int id)
    {
        var organisationId = await dbContext.Licences.Where(l => l.Id == id).Select(l => l.Company.OrganisationId).FirstOrDefaultAsync();

        if (organisationId == 0)
        {
            throw new NotFoundException($"License with id {id} not found");
        }

        return organisationId;
    }

    public async Task<Entites.Licence> GetWithCompanyAsync(int id)
    {
        var license = await dbContext.Licences
            .Include(l => l.Company)
            .FirstAsync(l => l.Id == id);

        return license ?? throw new NotFoundException($"License with id {id} not found");
    }
}
