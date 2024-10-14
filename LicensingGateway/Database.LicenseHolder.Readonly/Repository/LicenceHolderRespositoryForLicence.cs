using Database.LicenceHolder.Readonly;
using Microsoft.EntityFrameworkCore;

namespace Database.LicenceHolder.Readonly.Repository;

public class LicenceHolderRespositoryForLicence(
    ReadonlyLicenceHolderDbContext readonlyLicenceHolderDbContext) : ILicenceHolderRespositoryForLicence
{
    private readonly ReadonlyLicenceHolderDbContext readonlyLicenceHolderDbContext = readonlyLicenceHolderDbContext;

    public Task<bool> HasActiveLicence(string cqcProviderId)
    {
        var company = readonlyLicenceHolderDbContext.Companies
            .Where(c => c.CQCProviderID == cqcProviderId);

        return company.AnyAsync(c => c.Licences.Any(l => l.IsActive));
    }
}
