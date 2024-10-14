using Domain.Logic.Features.Tasks.Requests;
using Domain.Objects.ViewModels.Tasks;

namespace Domain.Logic.Features.Tasks;

public interface ITaskControllerHandler
{
    Task<TaskOverviewViewModel> Get(AnnualCertificateTaskRequest request, CancellationToken cancellationToken);

    Task<TaskOverviewViewModel> Get(FinancialMonitoringTaskRequest request, CancellationToken cancellationToken);

    Task<bool> IsUsersRoleLevel2(string userOktaId, CancellationToken cancellationToken);

    Task UpdateStatusOfTask(
        AnnualCertificateTaskRequest request,
        Domain.Objects.Database.TaskStatus status,
        CancellationToken cancellationToken);

    Task UpdateStatusOfTask(
        FinancialMonitoringTaskRequest request,
        Domain.Objects.Database.TaskStatus status,
        CancellationToken cancellationToken);
}
