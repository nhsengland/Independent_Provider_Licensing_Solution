using Database.Contexts;
using Database.Entites;
using Database.Repositories.Core;
using Domain.Objects.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Database.Repositories.Tasks;

public class RepositoryForFinancialMonitoringTasks(LicenceHolderDbContext dbContext) : ReadWriteRepository<Entites.TaskForFinancialMonitoring>(dbContext), IRepositoryForFinancialMonitoringTasks
{
    public async Task<int> GetOrganisationId(int taskId)
    {
        var companyId = await dbContext.TasksForFinancialMonitoring.Where(t => t.Id == taskId).Select(t => t.OrganisationId).FirstOrDefaultAsync();

        if(companyId == 0)
        {
            throw new NotFoundException($"Task with id {taskId} not found");
        }

        return companyId;
    }

    public async Task<TaskForFinancialMonitoring?> GetLatestTaskForOrganisation(int id)
    {
        return await dbContext.TasksForFinancialMonitoring
            .Where(t => t.OrganisationId == id && t.DateDue != null)
            .OrderByDescending(t => t.DateDue)
            .Take(1)
            .FirstOrDefaultAsync();
    }

    public async Task<string> GetOrganisationName(int taskId)
    {
        return await dbContext.TasksForFinancialMonitoring
            .Where(t => t.Id == taskId)
            .Select(t => t.Organisation.Name)
            .FirstOrDefaultAsync() ?? throw new NotFoundException($"Unable to find organisation name for task id {taskId}");
    }
}
