﻿// <auto-generated />
using System;
using Database.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Database.Migrations
{
    [DbContext(typeof(LicenceHolderDbContext))]
    [Migration("20240819091330_PowerAppSetting_FeedbackRecordDate")]
    partial class PowerAppSetting_FeedbackRecordDate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Database.Entites.ChangeRequest", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CompanyId")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateCreated")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<DateTime>("DateLastUpdated")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<DateOnly?>("FinancialYearEnd")
                        .HasColumnType("date");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RaisedById")
                        .HasColumnType("int");

                    b.Property<int>("StatusId")
                        .HasColumnType("int");

                    b.Property<int>("TypeId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.HasIndex("RaisedById");

                    b.HasIndex("StatusId");

                    b.HasIndex("TypeId");

                    b.ToTable("ChangeRequest");
                });

            modelBuilder.Entity("Database.Entites.ChangeRequestStatus", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ChangeRequestStatus");

                    b.HasData(
                        new
                        {
                            Id = 100,
                            Status = "Pending"
                        },
                        new
                        {
                            Id = 200,
                            Status = "Approved"
                        },
                        new
                        {
                            Id = 300,
                            Status = "Rejected"
                        });
                });

            modelBuilder.Entity("Database.Entites.ChangeRequestType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ChangeRequestType");

                    b.HasData(
                        new
                        {
                            Id = 100,
                            Type = "Address"
                        },
                        new
                        {
                            Id = 200,
                            Type = "FinancialYearEnd"
                        },
                        new
                        {
                            Id = 300,
                            Type = "Name"
                        });
                });

            modelBuilder.Entity("Database.Entites.Company", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CQCProviderID")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateCreated")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<DateTime>("DateLastUpdated")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2");

                    b.Property<DateOnly>("FinancialYearEnd")
                        .HasColumnType("date");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsCrs")
                        .HasColumnType("bit");

                    b.Property<bool>("IsHardToReplace")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<int>("OrganisationId")
                        .HasColumnType("int");

                    b.Property<string>("SharePointLocation")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("OrganisationId");

                    b.ToTable("Company");
                });

            modelBuilder.Entity("Database.Entites.EmailNotification", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DateCreated")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<DateTime?>("DateSent")
                        .HasColumnType("datetime2");

                    b.Property<bool>("HasBeenSent")
                        .HasColumnType("bit");

                    b.Property<int?>("RequestedById")
                        .HasColumnType("int");

                    b.Property<int>("TypeId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RequestedById");

                    b.HasIndex("TypeId");

                    b.HasIndex("UserId");

                    b.ToTable("EmailNotification");
                });

            modelBuilder.Entity("Database.Entites.EmailNotificationType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("EmailNotificationType");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Type = "InviteUser"
                        },
                        new
                        {
                            Id = 2,
                            Type = "ReSendInvite"
                        });
                });

            modelBuilder.Entity("Database.Entites.Feedback", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DateGenerated")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<string>("HowToImprove")
                        .HasMaxLength(1200)
                        .HasColumnType("nvarchar(1200)");

                    b.Property<int>("SatisfactionId")
                        .HasColumnType("int");

                    b.Property<int>("TypeId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SatisfactionId");

                    b.HasIndex("TypeId");

                    b.ToTable("Feedback");
                });

            modelBuilder.Entity("Database.Entites.FeedbackSatisfaction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("FeedbackSatisfaction");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "VeryDissatisfied"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Dissatisfied"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Neutral"
                        },
                        new
                        {
                            Id = 4,
                            Name = "Satisfied"
                        },
                        new
                        {
                            Id = 5,
                            Name = "VerySatisfied"
                        });
                });

            modelBuilder.Entity("Database.Entites.FeedbackType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("FeedbackType");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Type = "Other"
                        },
                        new
                        {
                            Id = 2,
                            Type = "Licence"
                        },
                        new
                        {
                            Id = 3,
                            Type = "Messages"
                        },
                        new
                        {
                            Id = 4,
                            Type = "Team"
                        },
                        new
                        {
                            Id = 5,
                            Type = "YourProfile"
                        },
                        new
                        {
                            Id = 6,
                            Type = "Tasks"
                        });
                });

            modelBuilder.Entity("Database.Entites.Licence", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CompanyId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("DateCancelled")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateIssued")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("LicenceNumber")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime?>("PublishedLicenceDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("PublishedLicenceUrl")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.ToTable("Licence");
                });

            modelBuilder.Entity("Database.Entites.Message", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Body")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("From")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsRead")
                        .HasColumnType("bit");

                    b.Property<int>("MessageTypeId")
                        .HasColumnType("int");

                    b.Property<int>("OrganisationId")
                        .HasColumnType("int");

                    b.Property<DateTime>("SendDateTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("MessageTypeId");

                    b.HasIndex("OrganisationId");

                    b.ToTable("Message");
                });

            modelBuilder.Entity("Database.Entites.MessageType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("MessageType");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Type = "Inbound"
                        },
                        new
                        {
                            Id = 2,
                            Type = "Outbound"
                        });
                });

            modelBuilder.Entity("Database.Entites.Organisation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DateCreated")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<DateTime>("DateLastUpdated")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsListed")
                        .HasColumnType("bit");

                    b.Property<Guid?>("NHSEUserEntraId1")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("NHSEUserEntraId2")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("NHSEUserEntraId3")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("NHSEUserEntraId4")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("NHSEUserEntraId5")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("NHSEUserEntraId6")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<string>("SharePointLocation")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Organisation");
                });

            modelBuilder.Entity("Database.Entites.TaskForAnnualCertificate", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CompanyId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("DateCompleted")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateCreated")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<DateOnly>("DateDue")
                        .HasColumnType("date");

                    b.Property<DateTime>("DateLastModified")
                        .HasColumnType("datetime2");

                    b.Property<bool>("InPortalNotificationSent")
                        .HasColumnType("bit");

                    b.Property<string>("SharePointLocation")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TaskName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TaskStatusId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.HasIndex("TaskStatusId");

                    b.ToTable("TaskForAnnualCertificate");
                });

            modelBuilder.Entity("Database.Entites.TaskForFinancialMonitoring", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("DateCompleted")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateCreated")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<DateOnly?>("DateDue")
                        .HasColumnType("date");

                    b.Property<DateTime>("DateLastModified")
                        .HasColumnType("datetime2");

                    b.Property<bool>("InPortalNotificationSent")
                        .HasColumnType("bit");

                    b.Property<int>("OrganisationId")
                        .HasColumnType("int");

                    b.Property<string>("SharePointLocation")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TaskName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TaskStatusId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("OrganisationId");

                    b.HasIndex("TaskStatusId");

                    b.ToTable("TaskForFinancialMonitoring");
                });

            modelBuilder.Entity("Database.Entites.TaskStatus", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("TaskStatus");

                    b.HasData(
                        new
                        {
                            Id = 100,
                            Status = "InComplete"
                        },
                        new
                        {
                            Id = 200,
                            Status = "Completed"
                        },
                        new
                        {
                            Id = 300,
                            Status = "ThereIsAProblem"
                        });
                });

            modelBuilder.Entity("Database.Entites.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DateCreated")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<DateTime>("DateLastModified")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("EmailIsVerified")
                        .HasColumnType("bit");

                    b.Property<Guid?>("EntraId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Firstname")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Lastname")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("OktaId")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("OrganisationId")
                        .HasColumnType("int");

                    b.Property<int>("UserRoleId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("OrganisationId");

                    b.HasIndex("UserRoleId");

                    b.HasIndex(new[] { "Email" }, "IX_User_Email")
                        .IsUnique();

                    b.HasIndex(new[] { "OktaId" }, "IX_User_OktaId");

                    b.ToTable("User");
                });

            modelBuilder.Entity("Database.Entites.UserRole", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("UserRole");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Role = "Level1"
                        },
                        new
                        {
                            Id = 2,
                            Role = "Level2"
                        });
                });

            modelBuilder.Entity("Database.Entites.ChangeRequest", b =>
                {
                    b.HasOne("Database.Entites.Company", "Company")
                        .WithMany()
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Database.Entites.User", "RaisedBy")
                        .WithMany()
                        .HasForeignKey("RaisedById")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Database.Entites.ChangeRequestStatus", "Status")
                        .WithMany()
                        .HasForeignKey("StatusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Database.Entites.ChangeRequestType", "Type")
                        .WithMany()
                        .HasForeignKey("TypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Company");

                    b.Navigation("RaisedBy");

                    b.Navigation("Status");

                    b.Navigation("Type");
                });

            modelBuilder.Entity("Database.Entites.Company", b =>
                {
                    b.HasOne("Database.Entites.Organisation", "Organisation")
                        .WithMany("Companies")
                        .HasForeignKey("OrganisationId")
                        .IsRequired()
                        .HasConstraintName("FK_Company_Organisation_OrganisationId");

                    b.Navigation("Organisation");
                });

            modelBuilder.Entity("Database.Entites.EmailNotification", b =>
                {
                    b.HasOne("Database.Entites.User", "RequestedBy")
                        .WithMany()
                        .HasForeignKey("RequestedById");

                    b.HasOne("Database.Entites.EmailNotificationType", "Type")
                        .WithMany()
                        .HasForeignKey("TypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Database.Entites.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("RequestedBy");

                    b.Navigation("Type");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Database.Entites.Feedback", b =>
                {
                    b.HasOne("Database.Entites.FeedbackSatisfaction", "Satisfaction")
                        .WithMany()
                        .HasForeignKey("SatisfactionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Database.Entites.FeedbackType", "Type")
                        .WithMany()
                        .HasForeignKey("TypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Satisfaction");

                    b.Navigation("Type");
                });

            modelBuilder.Entity("Database.Entites.Licence", b =>
                {
                    b.HasOne("Database.Entites.Company", "Company")
                        .WithMany("Licences")
                        .HasForeignKey("CompanyId")
                        .IsRequired()
                        .HasConstraintName("FK_Licence_Company");

                    b.Navigation("Company");
                });

            modelBuilder.Entity("Database.Entites.Message", b =>
                {
                    b.HasOne("Database.Entites.MessageType", "MessageType")
                        .WithMany()
                        .HasForeignKey("MessageTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Database.Entites.Organisation", "Organisation")
                        .WithMany()
                        .HasForeignKey("OrganisationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MessageType");

                    b.Navigation("Organisation");
                });

            modelBuilder.Entity("Database.Entites.TaskForAnnualCertificate", b =>
                {
                    b.HasOne("Database.Entites.Company", "Company")
                        .WithMany("Tasks")
                        .HasForeignKey("CompanyId")
                        .IsRequired();

                    b.HasOne("Database.Entites.TaskStatus", "TaskStatus")
                        .WithMany()
                        .HasForeignKey("TaskStatusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Company");

                    b.Navigation("TaskStatus");
                });

            modelBuilder.Entity("Database.Entites.TaskForFinancialMonitoring", b =>
                {
                    b.HasOne("Database.Entites.Organisation", "Organisation")
                        .WithMany("Tasks")
                        .HasForeignKey("OrganisationId")
                        .IsRequired();

                    b.HasOne("Database.Entites.TaskStatus", "TaskStatus")
                        .WithMany()
                        .HasForeignKey("TaskStatusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Organisation");

                    b.Navigation("TaskStatus");
                });

            modelBuilder.Entity("Database.Entites.User", b =>
                {
                    b.HasOne("Database.Entites.Organisation", "Organisation")
                        .WithMany("Users")
                        .HasForeignKey("OrganisationId")
                        .IsRequired()
                        .HasConstraintName("FK_User_Organisation");

                    b.HasOne("Database.Entites.UserRole", "UserRole")
                        .WithMany("Users")
                        .HasForeignKey("UserRoleId")
                        .IsRequired()
                        .HasConstraintName("FK_User_UserRole");

                    b.Navigation("Organisation");

                    b.Navigation("UserRole");
                });

            modelBuilder.Entity("Database.Entites.Company", b =>
                {
                    b.Navigation("Licences");

                    b.Navigation("Tasks");
                });

            modelBuilder.Entity("Database.Entites.Organisation", b =>
                {
                    b.Navigation("Companies");

                    b.Navigation("Tasks");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("Database.Entites.UserRole", b =>
                {
                    b.Navigation("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
