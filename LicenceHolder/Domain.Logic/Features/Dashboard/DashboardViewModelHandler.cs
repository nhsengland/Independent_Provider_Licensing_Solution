using Database.Entites;
using Database.Repositories;
using Database.Repositories.Orchestrate;
using Database.Repositories.Tasks;
using Database.Repositories.User;
using Domain.Objects.ViewModels.Dashboard;
using Domain.Objects.ViewModels.Tasks;

namespace Domain.Logic.Features.Dashboard;

public class DashboardViewModelHandler(
    ICompanyRepository companyRepository,
    IRepositoryOrchestrator repositoryOrchestrator,
    IRepositoryForFinancialMonitoringTasks repositoryForFinancialMonitoringTasks,
    IRepositoryForUser repositoryForUser) : IDashboardViewModelHandler
{
    private readonly ICompanyRepository companyRepository = companyRepository;
    private readonly IRepositoryOrchestrator repositoryOrchestrator = repositoryOrchestrator;
    private readonly IRepositoryForFinancialMonitoringTasks repositoryForFinancialMonitoringTasks = repositoryForFinancialMonitoringTasks;
    private readonly IRepositoryForUser repositoryForUser = repositoryForUser;

    public async Task<DashboardViewModel> Get(
        GetDashboardViewModelQuery request,
        CancellationToken cancellationToken)
    {
        var dashboardViewModel = new DashboardViewModel();

        var organisation = await repositoryOrchestrator.GetOrganisationAsync(request.UserId, cancellationToken) ?? throw new InvalidOperationException("Organisation not found");

        dashboardViewModel.OrganisationName = organisation.Name;

        dashboardViewModel.IsCrsOrHardToReplaceOrganisation = await companyRepository.OrganisationHasCrsOrHardToReplaceCompanys(organisation.Id);
        
        dashboardViewModel.FinancialMonitoringTasks = Create(await repositoryForFinancialMonitoringTasks.GetLatestTaskForOrganisation(organisation.Id));

        var companies = await companyRepository.GetAllByOrganisationId(organisation.Id);

        foreach (var company in companies)
        {
            foreach (var licence in company.Licences)
            {
                if (licence.IsActive)
                {
                    dashboardViewModel.Companies.Add(new DashboardViewModel.Company
                    {
                        Address = company.Address,
                        FinancialYearEnd = company.FinancialYearEnd,
                        Name = company.Name,
                        LicenceId = licence.Id,
                        LicenceNumber = licence.LicenceNumber,
                        AnnualCertificateTask = company.Tasks.Count != 0 ? Create(company.Tasks.First()) : null,
                    });
                }
            }
        }

        dashboardViewModel.UserRole = await repositoryForUser.GetUserRole(request.UserId, cancellationToken);

        return dashboardViewModel;
    }

    private static TaskViewModel Create(TaskForAnnualCertificate t)
    {
        return new TaskViewModel
        {
            Id = t.Id,
            DueDate = t.DateDue,
            Status = (Objects.Database.TaskStatus)t.TaskStatusId,
        };
    }

    private static TaskViewModel? Create(TaskForFinancialMonitoring? t)
    {
        if(t == null)
        {
            return null;
        }

        return new TaskViewModel
        {
            Id = t.Id,
            OrganisationId = t.OrganisationId,
            DueDate = t.DateDue ?? throw new InvalidOperationException($"This financial monitoring task due date is null: {t.Id}"),
            Status = (Objects.Database.TaskStatus)t.TaskStatusId,
        };
    }
}
