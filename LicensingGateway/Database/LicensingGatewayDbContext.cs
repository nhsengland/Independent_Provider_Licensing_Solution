using Database.Entites;
using Microsoft.EntityFrameworkCore;

namespace Database;

public class LicensingGatewayDbContext(DbContextOptions<LicensingGatewayDbContext> options) : DbContext(options), ILicensingGatewayDbContext
{
    public DbSet<ApplicationCode> ApplicationCode { get; set; } = default!;

    public DbSet<Application> Application { get; set; } = default!;

    public DbSet<PreApplication> PreApplication { get; set; } = default!;

    public DbSet<CQCProvider> CQCProvider { get; set; } = default!;

    public DbSet<CQCProviderImportInstance> CQCProviderImportInstance { get; set; } = default!;

    public DbSet<CQCProviderImportPage> CQCProviderImportPage { get; set; } = default!;

    public DbSet<CQCProviderRegulatedActivity> CQCProviderRegulatedActivity { get; set; } = default!;

    public DbSet<CQCProviderToRegulatedActivities> CQCProviderToRegulatedActivities { get; set; } = default!;

    public DbContext DbContext => this;

    public DbSet<Director> Director { get; set; } = default!;

    public DbSet<DirectorGroup> DirectorGroup { get; set; } = default!;

    public DbSet<DirectorType> DirectorType { get; set; } = default!;

    public DbSet<EmailNotification> EmailNotification { get; set; } = default!;

    public DbSet<EmailNotificationType> EmailNotificationType { get; set; } = default!;

    public DbSet<Feedback> Feedback { get; set; } = default!;

    public DbSet<UltimateController> UltimateController { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PreApplication>()
            .Property(b => b.DateGenerated)
            .HasDefaultValueSql("GETUTCDATE()")
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<Application>()
            .Property(b => b.DateGenerated)
            .HasDefaultValueSql("GETUTCDATE()")
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<Application>()
            .Property(b => b.DateModified)
            .HasDefaultValueSql("GETUTCDATE()")
            .ValueGeneratedOnAddOrUpdate();

        modelBuilder.Entity<EmailNotification>()
            .Property(b => b.DateGenerated)
            .HasDefaultValueSql("GETUTCDATE()")
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<EmailNotificationType>(entity =>
        {
            entity
               .Property(e => e.Type)
                .HasConversion(
                    v => v.ToString(),
                    v => (Domain.Models.Database.EmailNotificationType)Enum.Parse(typeof(Domain.Models.Database.EmailNotificationType), v)
                );

            entity.HasData(
                Enum.GetValues(typeof(Domain.Models.Database.EmailNotificationType))
                    .Cast<Domain.Models.Database.EmailNotificationType>()
                    .Select(ent => new EmailNotificationType
                    {
                        Type = ent,
                        Id = (int)ent
                    })
            );
        });

        modelBuilder.Entity<FeedbackType>(entity =>
        {
            entity
               .Property(e => e.Type)
                .HasConversion(
                    v => v.ToString(),
                    v => (Domain.Models.Database.FeedbackType)Enum.Parse(typeof(Domain.Models.Database.FeedbackType), v)
                );

            entity.HasData(
                Enum.GetValues(typeof(Domain.Models.Database.FeedbackType))
                    .Cast<Domain.Models.Database.FeedbackType>()
                    .Select(ent => new FeedbackType
                    {
                        Type = ent,
                        Id = (int)ent
                    })
            );
        });

        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.Property(e => e.DateGenerated).HasDefaultValueSql("GETUTCDATE()")
            .ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<FeedbackSatisfaction>(entity =>
        {
            entity
               .Property(e => e.Name)
                .HasConversion(
                    v => v.ToString(),
                    v => (Domain.Models.Database.FeedbackSatisfaction)Enum.Parse(typeof(Domain.Models.Database.FeedbackSatisfaction), v)
                );

            entity.HasData(
                Enum.GetValues(typeof(Domain.Models.Database.FeedbackSatisfaction))
                    .Cast<Domain.Models.Database.FeedbackSatisfaction>()
                    .Select(ent => new FeedbackSatisfaction
                    {
                        Name = ent,
                        Id = (int)ent
                    })
            );
        });

        modelBuilder.Entity<Director>(entity =>
        {
            entity.HasOne(e => e.Group)
                .WithMany(e => e.Directors)
                .OnDelete(DeleteBehavior.ClientCascade);
        });

        modelBuilder.Entity<DirectorType>(entity =>
        {
            entity
               .Property(e => e.Type)
                .HasConversion(
                    v => v.ToString(),
                    v => (Domain.Models.Database.DirectorType)Enum.Parse(typeof(Domain.Models.Database.DirectorType), v)
                );

            entity.HasData(
                Enum.GetValues(typeof(Domain.Models.Database.DirectorType))
                    .Cast<Domain.Models.Database.DirectorType>()
                    .Select(dt => new DirectorType
                    {
                        Type = dt,
                        Id = (int)dt
                    })
            );
        });
    }
}
