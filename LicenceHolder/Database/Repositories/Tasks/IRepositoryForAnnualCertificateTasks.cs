using Database.Repositories.Core;

namespace Database.Repositories.Tasks;

public interface IRepositoryForAnnualCertificateTasks : IReadWriteRepository<Entites.TaskForAnnualCertificate>
{
    Task<string> GetCompanyName(int taskId);

    Task<int> GetOrganisationId(int taskId);
}
