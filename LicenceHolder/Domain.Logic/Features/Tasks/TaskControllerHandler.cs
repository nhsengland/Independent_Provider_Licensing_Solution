using Database.Repositories;
using Database.Repositories.License;
using Database.Repositories.Tasks;
using Database.Repositories.User;
using Domain.Logic.Factories;
using Domain.Logic.Features.Messages;
using Domain.Logic.Features.Messages.Requests;
using Domain.Logic.Features.Tasks.Requests;
using Domain.Objects.Exceptions;
using Domain.Objects.ViewModels.Tasks;

namespace Domain.Logic.Features.Tasks;

public class TaskControllerHandler(
    IRepositoryForUser repositoryForUser,
    IRepositoryForLicense repositoryForLicense,
    IRepositoryForAnnualCertificateTasks repositoryForAnnualCertificateTasks,
    IRepositoryForFinancialMonitoringTasks repositoryForFinancialMonitoringTasks,
    ICompanyRepository companyRepository,
    IMessagesHandler messagesHandler,
    IDateTimeFactory dateTimeFactory) : ITaskControllerHandler
{
    private readonly IRepositoryForUser repositoryForUser = repositoryForUser;
    private readonly IRepositoryForLicense repositoryForLicense = repositoryForLicense;
    private readonly IRepositoryForAnnualCertificateTasks repositoryForAnnualCertificateTasks = repositoryForAnnualCertificateTasks;
    private readonly IRepositoryForFinancialMonitoringTasks repositoryForFinancialMonitoringTasks = repositoryForFinancialMonitoringTasks;
    private readonly ICompanyRepository companyRepository = companyRepository;
    private readonly IMessagesHandler messagesHandler = messagesHandler;
    private readonly IDateTimeFactory dateTimeFactory = dateTimeFactory;

    public async Task<TaskOverviewViewModel> Get(AnnualCertificateTaskRequest request, CancellationToken cancellationToken)
    {
        await EnsureRequestParametersAreInTheSameOrganisation(request, cancellationToken);

        var task = await repositoryForAnnualCertificateTasks.GetByIdAsync(request.TaskId, cancellationToken) ?? throw new NotFoundException($"Task with id {request.TaskId} not found");

        var organisationId = await repositoryForAnnualCertificateTasks.GetOrganisationId(request.TaskId);

        return new TaskOverviewViewModel()
        {
            Name = await repositoryForAnnualCertificateTasks.GetCompanyName(request.TaskId),
            LicenseNumber = await repositoryForLicense.GetLicenseNumber(request.LicenseId),
            Status = (Objects.Database.TaskStatus)task.TaskStatusId,
            DueDate = task.DateDue,
            SharePointURL = task.SharePointLocation,
            OrganisationIsCrsOrHardToReplace = await companyRepository.OrganisationHasCrsOrHardToReplaceCompanys(organisationId),
        };
    }

    public async Task<TaskOverviewViewModel> Get(FinancialMonitoringTaskRequest request, CancellationToken cancellationToken)
    {
        await EnsureRequestParametersAreInTheSameOrganisation(request, cancellationToken);

        var task = await repositoryForFinancialMonitoringTasks.GetByIdAsync(request.TaskId, cancellationToken) ?? throw new NotFoundException($"Task with id {request.TaskId} not found");

        return new TaskOverviewViewModel()
        {
            Name = await repositoryForFinancialMonitoringTasks.GetOrganisationName(request.TaskId),
            Status = (Objects.Database.TaskStatus)task.TaskStatusId,
            DueDate = task.DateDue ?? throw new InvalidOperationException($"This financial monitoring task due date is null: {task.Id}"),
            SharePointURL = task.SharePointLocation,
        };
    }

    public async Task<bool> IsUsersRoleLevel2(string userOktaId, CancellationToken cancellationToken)
    {
        var role = await repositoryForUser.GetUserRole(userOktaId, cancellationToken);

        if (role == Domain.Objects.Database.UserRole.Level2)
        {
            return true;
        }

        return false;
    }

    public async Task UpdateStatusOfTask(
        AnnualCertificateTaskRequest request,
        Objects.Database.TaskStatus status,
        CancellationToken cancellationToken)
    {
        var date = dateTimeFactory.Create();

        await EnsureRequestParametersAreInTheSameOrganisation(request, cancellationToken);

        var task = await repositoryForAnnualCertificateTasks.GetByIdAsync(request.TaskId, cancellationToken) ?? throw new NotFoundException($"Task not found: {request.TaskId}, User {request.UserOktaId}, Licence id: {request.LicenseId}");

        task.TaskStatusId = (int)status;
        task.DateCompleted = date;
        task.DateLastModified = date;

        await repositoryForAnnualCertificateTasks.SaveChangesAsync(cancellationToken);

        await messagesHandler.SendAsync(new UpdateAnnualCertificateTaskStatusRequest() {
            OrganisationId = await repositoryForLicense.GetOrganisationId(request.LicenseId),
            LicenceName = await repositoryForLicense.GetLicenseName(request.LicenseId),
            RequestorName = await repositoryForUser.GetUserFullName(request.UserOktaId, cancellationToken),
            RequestedOn = date,
        }, cancellationToken);
    }

    public async Task UpdateStatusOfTask(
        FinancialMonitoringTaskRequest request,
        Objects.Database.TaskStatus status,
        CancellationToken cancellationToken)
    {
        if(await IsUsersRoleLevel2(request.UserOktaId, cancellationToken))
        {
            var date = dateTimeFactory.Create();
            await EnsureRequestParametersAreInTheSameOrganisation(request, cancellationToken);

            var task = await repositoryForFinancialMonitoringTasks.GetByIdAsync(request.TaskId, cancellationToken) ?? throw new NotFoundException($"Task not found: {request.TaskId}, User {request.UserOktaId}");

            task.TaskStatusId = (int)status;
            task.DateCompleted = date;
            task.DateLastModified = date;

            await repositoryForAnnualCertificateTasks.SaveChangesAsync(cancellationToken);

            await messagesHandler.SendAsync(new UpdateFinancialMonitorngTaskStatusRequest()
            {
                OrganisationId = request.OrganisationId,
                RequestorName = await repositoryForUser.GetUserFullName(request.UserOktaId, cancellationToken),
                RequestedOn = date,
            }, cancellationToken);
        }
    }

    private async Task EnsureRequestParametersAreInTheSameOrganisation(AnnualCertificateTaskRequest request, CancellationToken cancellationToken)
    {
        var userId = await repositoryForUser.GetIdAsync(request.UserOktaId, cancellationToken);

        var taskOrganisationId = await repositoryForAnnualCertificateTasks.GetOrganisationId(request.TaskId);

        var isUserInOrganisation = await repositoryForUser.UserExistsInOrganisation(userId, taskOrganisationId, cancellationToken);

        if (!isUserInOrganisation)
        {
            throw new NotAuthorisedException($"User {userId} and task are not in the same organisation {taskOrganisationId}");
        }

        await EnsureUserHasAccessToLicense(request.LicenseId, userId, cancellationToken);
    }

    private async Task EnsureRequestParametersAreInTheSameOrganisation(FinancialMonitoringTaskRequest request, CancellationToken cancellationToken)
    {
        var userId = await repositoryForUser.GetIdAsync(request.UserOktaId, cancellationToken);

        var taskOrganisationId = await repositoryForFinancialMonitoringTasks.GetOrganisationId(request.TaskId);

        var isUserInOrganisation = await repositoryForUser.UserExistsInOrganisation(userId, taskOrganisationId, cancellationToken);

        if (!isUserInOrganisation)
        {
            throw new NotAuthorisedException($"User {userId} and task are not in the same organisation {taskOrganisationId}");
        }
    }

    private async Task EnsureUserHasAccessToLicense(int licenseId, int userId, CancellationToken cancellationToken)
    {
        var licenseOrganisationId = await repositoryForLicense.GetOrganisationId(licenseId);

        var isUserInOrganisationLicenseCheck = await repositoryForUser.UserExistsInOrganisation(userId, licenseOrganisationId, cancellationToken);

        if (!isUserInOrganisationLicenseCheck)
        {
            throw new NotAuthorisedException($"User {userId} and license are not in the same organisation {licenseOrganisationId}");
        }
    }
}
