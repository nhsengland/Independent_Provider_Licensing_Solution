using Database.Entites;

namespace Database.Repositories;

public interface ICompanyRepository
{
    Task<IList<Company>> GetAllByOrganisationId(int organisationId);

    Task<Company> GetAsync(int companyId);

    Task<string> GetCompanytName(int companyId);

    Task<bool> OrganisationHasCrsOrHardToReplaceCompanys(int organisationId);
}
