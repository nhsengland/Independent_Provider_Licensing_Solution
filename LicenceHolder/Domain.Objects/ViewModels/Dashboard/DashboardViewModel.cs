using Domain.Objects.Database;
using Domain.Objects.ViewModels.Tasks;

namespace Domain.Objects.ViewModels.Dashboard;

public class DashboardViewModel
{
    public string OrganisationName { get; set; } = default!;

    public bool IsCrsOrHardToReplaceOrganisation { get; set; }

    public TaskViewModel? FinancialMonitoringTasks { get; set; }

    public List<Company> Companies { get; set; } = [];

    public UserRole UserRole { get; set; }

    public class Company
    {
        public string Name { get; set; } = default!;
        public DateOnly FinancialYearEnd { get; set; }
        public string Address { get; set; } = default!;

        public int LicenceId { get; set; }
        public string LicenceNumber { get; set; } = string.Empty;

        public TaskViewModel? AnnualCertificateTask { get; set; }
    }
}
