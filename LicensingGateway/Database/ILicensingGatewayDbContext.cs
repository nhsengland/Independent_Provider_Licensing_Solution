using Database.Entites;
using Microsoft.EntityFrameworkCore;

namespace Database;
public interface ILicensingGatewayDbContext
{
    DbContext DbContext { get; }

    DbSet<ApplicationCode> ApplicationCode { get; set; }

    DbSet<Application> Application { get; set; }

    DbSet<Director> Director { get; set; }

    DbSet<DirectorGroup> DirectorGroup { get; set; }

    DbSet<UltimateController> UltimateController { get; set; }

    DbSet<EmailNotification> EmailNotification { get; set; }

    DbSet<PreApplication> PreApplication { get; set; }

    DbSet<CQCProvider> CQCProvider { get; set; }

    DbSet<CQCProviderImportInstance> CQCProviderImportInstance { get; set; }

    DbSet<CQCProviderImportPage> CQCProviderImportPage { get; set; }

    DbSet<CQCProviderRegulatedActivity> CQCProviderRegulatedActivity { get; set; }

    DbSet<CQCProviderToRegulatedActivities> CQCProviderToRegulatedActivities { get; set; }
}
