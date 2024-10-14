using Database.Entites;
using Database.Repositories.Core;

namespace Database.Repositories.Tasks;

public interface IRepositoryForFinancialMonitoringTasks : IReadWriteRepository<Entites.TaskForFinancialMonitoring>
{
    Task<string> GetOrganisationName(int taskId);

    Task<int> GetOrganisationId(int taskId);

    Task<TaskForFinancialMonitoring?> GetLatestTaskForOrganisation(int id);
}
