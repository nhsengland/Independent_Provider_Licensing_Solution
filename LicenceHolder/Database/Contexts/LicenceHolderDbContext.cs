using Database.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Database.Contexts;

public class LicenceHolderDbContext(DbContextOptions<LicenceHolderDbContext> options) : DbContext(options)
{
    public virtual DbSet<ChangeRequest> ChangeRequests { get; set; }

    public virtual DbSet<Entites.ChangeRequestStatus> ChangeRequestStatuses { get; set; }

    public virtual DbSet<Entites.ChangeRequestType> ChangeRequestTypes { get; set; }

    public virtual DbSet<Company> Companies { get; set; }

    public virtual DbSet<EmailNotification> EmailNotification { get; set; }

    public virtual DbSet<Feedback> Feedback { get; set; }

    public virtual DbSet<Entites.Licence> Licences { get; set; }

    public DbSet<Message> Messages { get; set; }

    public virtual DbSet<Organisation> Organisations { get; set; }

    public virtual DbSet<TaskForAnnualCertificate> TasksForAnnualCertificate { get; set; }

    public virtual DbSet<TaskForFinancialMonitoring> TasksForFinancialMonitoring { get; set; }

    public virtual DbSet<Entites.TaskStatus> TaskStatus { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ChangeRequest>(entity =>
        {
            entity
                .Property(b => b.DateCreated)
                .HasDefaultValueSql("GETUTCDATE()");

            entity.Property(b => b.DateLastUpdated)
                .HasDefaultValueSql("GETUTCDATE()")
                .ValueGeneratedOnAddOrUpdate();
        });

        modelBuilder.Entity<ChangeRequestStatus>(entity =>
        {
            entity
               .Property(e => e.Status)
                .HasConversion(
                    v => v.ToString(),
                    v => (Domain.Objects.Database.ChangeRequestStatus)Enum.Parse(typeof(Domain.Objects.Database.ChangeRequestStatus), v)
                );

            entity.HasData(
                Enum.GetValues(typeof(Domain.Objects.Database.ChangeRequestStatus))
                    .Cast<Domain.Objects.Database.ChangeRequestStatus>()
                    .Select(crs => new ChangeRequestStatus
                    {
                        Status = crs,
                        Id = (int)crs
                    })
            );
        });

        modelBuilder.Entity<ChangeRequestType>(entity =>
        {
            entity
               .Property(e => e.Type)
                .HasConversion(
                    v => v.ToString(),
                    v => (Domain.Objects.Database.ChangeRequestType)Enum.Parse(typeof(Domain.Objects.Database.ChangeRequestType), v)
                );

            entity.HasData(
                Enum.GetValues(typeof(Domain.Objects.Database.ChangeRequestType))
                    .Cast<Domain.Objects.Database.ChangeRequestType>()
                    .Select(crt => new ChangeRequestType
                    {
                        Type = crt,
                        Id = (int)crt
                    })
            );
        });

        modelBuilder.Entity<Company>(entity =>
        {
            entity.HasOne(d => d.Organisation).WithMany(p => p.Companies)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Company_Organization");

            entity
                .Property(b => b.DateCreated)
                .HasDefaultValueSql("GETUTCDATE()");

            entity.Property(b => b.DateLastUpdated)
                .ValueGeneratedOnAddOrUpdate()
                .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Save);

            entity.HasOne(a => a.Organisation)
                .WithMany(a => a.Companies)
                .HasForeignKey(a => a.OrganisationId)
                .HasConstraintName("FK_Company_Organisation_OrganisationId");
        });

        modelBuilder.Entity<EmailNotification>(entity =>
        {
            entity
                .Property(b => b.DateCreated).ValueGeneratedOnAdd()
                .HasDefaultValueSql("GETUTCDATE()");
        });

        modelBuilder.Entity<EmailNotificationType>(entity =>
        {
            entity
               .Property(e => e.Type)
                .HasConversion(
                    v => v.ToString(),
                    v => (Domain.Objects.Database.EmailNotificationType)Enum.Parse(typeof(Domain.Objects.Database.EmailNotificationType), v)
                );

            entity.HasData(
                Enum.GetValues(typeof(Domain.Objects.Database.EmailNotificationType))
                    .Cast<Domain.Objects.Database.EmailNotificationType>()
                    .Select(crt => new EmailNotificationType
                    {
                        Type = crt,
                        Id = (int)crt
                    })
            );
        });

        modelBuilder.Entity<FeedbackSatisfaction>(entity =>
        {
            entity
               .Property(e => e.Name)
                .HasConversion(
                    v => v.ToString(),
                    v => (Domain.Objects.Database.FeedbackSatisfaction)Enum.Parse(typeof(Domain.Objects.Database.FeedbackSatisfaction), v)
                );

            entity.HasData(
                Enum.GetValues(typeof(Domain.Objects.Database.FeedbackSatisfaction))
                    .Cast<Domain.Objects.Database.FeedbackSatisfaction>()
                    .Select(ent => new FeedbackSatisfaction
                    {
                        Name = ent,
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
                    v => (Domain.Objects.Database.FeedbackType)Enum.Parse(typeof(Domain.Objects.Database.FeedbackType), v)
                );

            entity.HasData(
                Enum.GetValues(typeof(Domain.Objects.Database.FeedbackType))
                    .Cast<Domain.Objects.Database.FeedbackType>()
                    .Select(ent => new FeedbackType
                    {
                        Type = ent,
                        Id = (int)ent
                    })
            );
        });

        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.Property(b => b.DateGenerated)
                .HasDefaultValueSql("GETUTCDATE()");
        });

        modelBuilder.Entity<MessageType>(entity =>
        {
            entity
               .Property(e => e.Type)
                .HasConversion(
                    v => v.ToString(),
                    v => (Domain.Objects.Database.MessageType)Enum.Parse(typeof(Domain.Objects.Database.MessageType), v)
                );

            entity.HasData(
                Enum.GetValues(typeof(Domain.Objects.Database.MessageType))
                    .Cast<Domain.Objects.Database.MessageType>()
                    .Select(crt => new MessageType
                    {
                        Type = crt,
                        Id = (int)crt
                    })
            );
        });

        modelBuilder.Entity<Entites.Licence>(entity =>
        {
            entity.HasOne(d => d.Company).WithMany(p => p.Licences)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Licence_Company");
        });

        modelBuilder.Entity<Message>(entity =>
        {
            entity
                .Property(b => b.SendDateTime)
                .HasDefaultValueSql("GETUTCDATE()");
        });

        modelBuilder.Entity<Organisation>(entity =>
        {
            entity
                .Property(b => b.DateCreated)
                .HasDefaultValueSql("GETUTCDATE()");

            entity.Property(b => b.DateLastUpdated)
                .ValueGeneratedOnAddOrUpdate()
                .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Save);
        });

        modelBuilder.Entity<TaskForAnnualCertificate>(entity =>
        {
            entity
                .Property(b => b.DateCreated)
                .HasDefaultValueSql("GETUTCDATE()");

            entity.HasOne(d => d.Company).WithMany(p => p.Tasks)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<TaskForFinancialMonitoring>(entity =>
        {
            entity
                .Property(b => b.DateCreated)
                .HasDefaultValueSql("GETUTCDATE()");

            entity.HasOne(d => d.Organisation).WithMany(p => p.Tasks)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Entites.TaskStatus>(entity =>
        {
            entity
               .Property(e => e.Status)
                .HasConversion(
                    v => v.ToString(),
                    v => (Domain.Objects.Database.TaskStatus)Enum.Parse(typeof(Domain.Objects.Database.TaskStatus), v)
                );

            entity.HasData(
                Enum.GetValues(typeof(Domain.Objects.Database.TaskStatus))
                    .Cast<Domain.Objects.Database.TaskStatus>()
                    .Select(ts => new Entites.TaskStatus
                    {
                        Status = ts,
                        Id = (int)ts
                    })
            );
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity
                .Property(b => b.DateCreated)
                .HasDefaultValueSql("GETUTCDATE()");

            entity.HasOne(d => d.Organisation).WithMany(p => p.Users)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_User_Organisation");

            entity.HasOne(d => d.UserRole).WithMany(p => p.Users)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_User_UserRole");
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity
               .Property(e => e.Role)
                .HasConversion(
                    v => v.ToString(),
                    v => (Domain.Objects.Database.UserRole)Enum.Parse(typeof(Domain.Objects.Database.UserRole), v)
                );

            entity.HasData(
                Enum.GetValues(typeof(Domain.Objects.Database.UserRole))
                    .Cast<Domain.Objects.Database.UserRole>()
                    .Select(ur => new UserRole
                    {
                        Role = ur,
                        Id = (int)ur
                    })
            );
        });

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(LicenceHolderDbContext).Assembly);
    }
}
