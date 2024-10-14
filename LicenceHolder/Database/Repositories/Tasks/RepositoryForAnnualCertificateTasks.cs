using Database.Contexts;
using Database.Repositories.Core;
using Domain.Objects.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Database.Repositories.Tasks;

public class RepositoryForAnnualCertificateTasks(LicenceHolderDbContext dbContext) : ReadWriteRepository<Entites.TaskForAnnualCertificate>(dbContext), IRepositoryForAnnualCertificateTasks
{
    public async Task<string> GetCompanyName(int taskId)
    {
        var companyName = await dbContext.TasksForAnnualCertificate.Where(t => t.Id == taskId).Select(t => t.Company.Name).FirstOrDefaultAsync();

        if (string.IsNullOrWhiteSpace(companyName))
        {
            throw new NotFoundException($"Task with id {taskId} not found");
        }

        return companyName;
    }

    public async Task<int> GetOrganisationId(int taskId)
    {
        var companyId = await dbContext.TasksForAnnualCertificate.Where(t => t.Id == taskId).Select(t => t.Company.OrganisationId).FirstOrDefaultAsync();

        if(companyId == 0)
        {
            throw new NotFoundException($"Task with id {taskId} not found");
        }

        return companyId;
    }
}
