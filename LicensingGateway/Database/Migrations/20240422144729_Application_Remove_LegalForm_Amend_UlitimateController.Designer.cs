// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Database.Licensing.Gateway.Migrations
{
    [DbContext(typeof(LicensingGatewayDbContext))]
    [Migration("20240422144729_Application_Remove_LegalForm_Amend_UlitimateController")]
    partial class Application_Remove_LegalForm_Amend_UlitimateController
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Database.Entites.Application", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ApplicationCodeId")
                        .HasColumnType("int");

                    b.Property<string>("CQCProviderAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CQCProviderID")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CQCProviderName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CQCProviderPhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CQCProviderWebsiteURL")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CompanyNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("CompanyNumberCheck")
                        .HasColumnType("bit");

                    b.Property<int>("CurrentPageId")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateGenerated")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<DateTime>("DateModified")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<string>("DirectorsIndividualsOrCorporate")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("DirectorsReadAndUnderstoodG3FitAndProper")
                        .HasColumnType("bit");

                    b.Property<bool?>("DirectorsSatisfyG3FitAndProperRequirements")
                        .HasColumnType("bit");

                    b.Property<bool>("ElectronicCommunications")
                        .HasColumnType("bit");

                    b.Property<string>("Email")
                        .HasMaxLength(254)
                        .HasColumnType("nvarchar(254)");

                    b.Property<string>("Forename")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("JobTitle")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateOnly?>("LastFinancialYear")
                        .HasColumnType("date");

                    b.Property<bool?>("LicenceConditionConfirmation")
                        .HasColumnType("bit");

                    b.Property<DateOnly?>("NextFinancialYear")
                        .HasColumnType("date");

                    b.Property<bool?>("PreviousApplication")
                        .HasColumnType("bit");

                    b.Property<bool?>("PreviousLicense")
                        .HasColumnType("bit");

                    b.Property<string>("PreviousLicenseLicenseNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PreviousLicenseProviderName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ReferenceId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("RegisteredWithCQC")
                        .HasColumnType("bit");

                    b.Property<bool?>("RegulatedByCQC")
                        .HasColumnType("bit");

                    b.Property<bool>("SubmitApplication")
                        .HasColumnType("bit");

                    b.Property<string>("Surname")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("UltimateController")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("ApplicationCodeId")
                        .IsUnique();

                    b.HasIndex("CurrentPageId");

                    b.ToTable("Application");
                });

            modelBuilder.Entity("Database.Entites.ApplicationCode", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("IsCRS")
                        .HasColumnType("bit");

                    b.Property<bool>("IsHardToReplace")
                        .HasColumnType("bit");

                    b.Property<string>("NoPreApplication_CQCProviderName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NoPreApplication_Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NoPreApplication_FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NoPreApplication_LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OktaUserId")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int?>("PreApplicationId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("Code")
                        .IsUnique();

                    b.HasIndex("PreApplicationId");

                    b.ToTable("ApplicationCode");
                });

            modelBuilder.Entity("Database.Entites.ApplicationPage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("PageName")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("ApplicationPage");
                });

            modelBuilder.Entity("Database.Entites.CQCProvider", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Address_Line_1")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Address_Line_2")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CQCProviderID")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Postcode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Region")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TownCity")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("WebsiteURL")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "CQCProviderID" }, "IX_CQCProvider_CQCProviderID");

                    b.ToTable("CQCProvider");
                });

            modelBuilder.Entity("Database.Entites.CQCProviderImportInstance", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("CQCProviderImportInstance");
                });

            modelBuilder.Entity("Database.Entites.CQCProviderImportPage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CQCProviderID")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("CQCProviderImportInstanceId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("NumberOfAttemptsToImport")
                        .HasColumnType("int");

                    b.Property<int>("PageNumber")
                        .HasColumnType("int");

                    b.Property<int?>("StatusCode")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CQCProviderImportInstanceId");

                    b.ToTable("CQCProviderImportPage");
                });

            modelBuilder.Entity("Database.Entites.Director", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ApplicationId")
                        .HasColumnType("int");

                    b.Property<DateOnly?>("DateOfBirth")
                        .HasColumnType("date");

                    b.Property<string>("Forename")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Organisation")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Surname")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ApplicationId");

                    b.ToTable("Director");
                });

            modelBuilder.Entity("Database.Entites.EmailNotification", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("ApplicationId")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateGenerated")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<DateTime?>("DateSent")
                        .HasColumnType("datetime2");

                    b.Property<bool>("HasBeenSent")
                        .HasColumnType("bit");

                    b.Property<int?>("PreApplicationId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ApplicationId");

                    b.HasIndex("PreApplicationId");

                    b.ToTable("EmailNotification");
                });

            modelBuilder.Entity("Database.Entites.License", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateOnly>("DateGenerated")
                        .HasColumnType("date");

                    b.Property<string>("LicenseKey")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("License");
                });

            modelBuilder.Entity("Database.Entites.PreApplication", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CQCProviderAddress")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CQCProviderID")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CQCProviderName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CQCProviderPhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateGenerated")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsExclusive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsHealthCareService")
                        .HasColumnType("bit");

                    b.Property<string>("JobTitle")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ReferenceId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Turnover")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("PreApplication");
                });

            modelBuilder.Entity("Database.Entites.UltimateController", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ApplicationId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ApplicationId");

                    b.ToTable("UltimateController");
                });

            modelBuilder.Entity("Database.Entites.Application", b =>
                {
                    b.HasOne("Database.Entites.ApplicationCode", "ApplicationCode")
                        .WithOne("Application")
                        .HasForeignKey("Database.Entites.Application", "ApplicationCodeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Database.Entites.ApplicationPage", "CurrentPage")
                        .WithMany()
                        .HasForeignKey("CurrentPageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ApplicationCode");

                    b.Navigation("CurrentPage");
                });

            modelBuilder.Entity("Database.Entites.ApplicationCode", b =>
                {
                    b.HasOne("Database.Entites.PreApplication", "PreApplication")
                        .WithMany()
                        .HasForeignKey("PreApplicationId");

                    b.Navigation("PreApplication");
                });

            modelBuilder.Entity("Database.Entites.CQCProviderImportPage", b =>
                {
                    b.HasOne("Database.Entites.CQCProviderImportInstance", "CQCProviderImportInstance")
                        .WithMany()
                        .HasForeignKey("CQCProviderImportInstanceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CQCProviderImportInstance");
                });

            modelBuilder.Entity("Database.Entites.Director", b =>
                {
                    b.HasOne("Database.Entites.Application", "Application")
                        .WithMany()
                        .HasForeignKey("ApplicationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Application");
                });

            modelBuilder.Entity("Database.Entites.EmailNotification", b =>
                {
                    b.HasOne("Database.Entites.Application", "Application")
                        .WithMany()
                        .HasForeignKey("ApplicationId");

                    b.HasOne("Database.Entites.PreApplication", "PreApplication")
                        .WithMany()
                        .HasForeignKey("PreApplicationId");

                    b.Navigation("Application");

                    b.Navigation("PreApplication");
                });

            modelBuilder.Entity("Database.Entites.UltimateController", b =>
                {
                    b.HasOne("Database.Entites.Application", "Application")
                        .WithMany()
                        .HasForeignKey("ApplicationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Application");
                });

            modelBuilder.Entity("Database.Entites.ApplicationCode", b =>
                {
                    b.Navigation("Application");
                });
#pragma warning restore 612, 618
        }
    }
}
