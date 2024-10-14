using Database.Repositories.Core;

namespace Database.Repositories.License;

public interface IRepositoryForLicense : IReadWriteRepository<Entites.Licence>
{
    Task<string> GetLicenseName(int id);

    Task<string> GetLicenseNumber(int id);

    Task<int> GetOrganisationId(int id);

    Task<Entites.Licence> GetWithCompanyAsync(int id);
}
