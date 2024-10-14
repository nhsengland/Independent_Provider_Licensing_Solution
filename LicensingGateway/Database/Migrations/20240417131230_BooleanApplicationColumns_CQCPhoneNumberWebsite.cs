using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Licensing.Gateway.Migrations;

/// <inheritdoc />
public partial class BooleanApplicationColumns_CQCPhoneNumberWebsite : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("DELETE FROM [EmailNotification]", true);

        migrationBuilder.Sql("DELETE FROM [Application]", true);

        migrationBuilder.Sql("DELETE FROM [PreApplication]", true);

        migrationBuilder.Sql("DELETE FROM [ApplicationPage]", true);

        migrationBuilder.Sql("DBCC CHECKIDENT('[ApplicationPage]', RESEED, 0)", true);

        foreach (var page in Enum.GetValues(typeof(Domain.Models.Database.ApplicationPage)))
        {
            migrationBuilder.Sql($"INSERT INTO [ApplicationPage] (PageName) VALUES ('{page}')");
        }

        migrationBuilder.AlterColumn<bool>(
            name: "IsHealthCareService",
            table: "PreApplication",
            type: "bit",
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)");

        migrationBuilder.AlterColumn<bool>(
            name: "IsExclusive",
            table: "PreApplication",
            type: "bit",
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)");

        migrationBuilder.AddColumn<string>(
            name: "PhoneNumber",
            table: "PreApplication",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: "");

        migrationBuilder.AddColumn<string>(
            name: "PhoneNumber",
            table: "CQCProvider",
            type: "nvarchar(max)",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "WebsiteURL",
            table: "CQCProvider",
            type: "nvarchar(max)",
            nullable: true);

        migrationBuilder.AlterColumn<bool>(
            name: "UltimateController",
            table: "Application",
            type: "bit",
            nullable: true,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)",
            oldNullable: true);

        migrationBuilder.AlterColumn<bool>(
            name: "RegulatedByCQC",
            table: "Application",
            type: "bit",
            nullable: true,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)",
            oldNullable: true);

        migrationBuilder.AlterColumn<bool>(
            name: "RegisteredWithCQC",
            table: "Application",
            type: "bit",
            nullable: true,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)",
            oldNullable: true);

        migrationBuilder.AlterColumn<bool>(
            name: "PreviousApplication",
            table: "Application",
            type: "bit",
            nullable: true,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)",
            oldNullable: true);

        migrationBuilder.AlterColumn<bool>(
            name: "LicenceConditionConfirmation",
            table: "Application",
            type: "bit",
            nullable: true,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)",
            oldNullable: true);

        migrationBuilder.AlterColumn<bool>(
            name: "DirectorsFitAndProper",
            table: "Application",
            type: "bit",
            nullable: true,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)",
            oldNullable: true);

        migrationBuilder.AlterColumn<bool>(
            name: "CompanyNumberCheck",
            table: "Application",
            type: "bit",
            nullable: true,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)",
            oldNullable: true);

        migrationBuilder.AddColumn<string>(
            name: "CQCProviderPhoneNumber",
            table: "Application",
            type: "nvarchar(max)",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "CQCProviderWebsiteURL",
            table: "Application",
            type: "nvarchar(max)",
            nullable: true);

        migrationBuilder.AddColumn<bool>(
            name: "PreviousLicense",
            table: "Application",
            type: "bit",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "PreviousLicenseLicenseNumber",
            table: "Application",
            type: "nvarchar(max)",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "PreviousLicenseProviderName",
            table: "Application",
            type: "nvarchar(max)",
            nullable: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "PhoneNumber",
            table: "PreApplication");

        migrationBuilder.DropColumn(
            name: "PhoneNumber",
            table: "CQCProvider");

        migrationBuilder.DropColumn(
            name: "WebsiteURL",
            table: "CQCProvider");

        migrationBuilder.DropColumn(
            name: "CQCProviderPhoneNumber",
            table: "Application");

        migrationBuilder.DropColumn(
            name: "CQCProviderWebsiteURL",
            table: "Application");

        migrationBuilder.DropColumn(
            name: "PreviousLicense",
            table: "Application");

        migrationBuilder.DropColumn(
            name: "PreviousLicenseLicenseNumber",
            table: "Application");

        migrationBuilder.DropColumn(
            name: "PreviousLicenseProviderName",
            table: "Application");

        migrationBuilder.AlterColumn<string>(
            name: "IsHealthCareService",
            table: "PreApplication",
            type: "nvarchar(max)",
            nullable: false,
            oldClrType: typeof(bool),
            oldType: "bit");

        migrationBuilder.AlterColumn<string>(
            name: "IsExclusive",
            table: "PreApplication",
            type: "nvarchar(max)",
            nullable: false,
            oldClrType: typeof(bool),
            oldType: "bit");

        migrationBuilder.AlterColumn<string>(
            name: "UltimateController",
            table: "Application",
            type: "nvarchar(max)",
            nullable: true,
            oldClrType: typeof(bool),
            oldType: "bit",
            oldNullable: true);

        migrationBuilder.AlterColumn<string>(
            name: "RegulatedByCQC",
            table: "Application",
            type: "nvarchar(max)",
            nullable: true,
            oldClrType: typeof(bool),
            oldType: "bit",
            oldNullable: true);

        migrationBuilder.AlterColumn<string>(
            name: "RegisteredWithCQC",
            table: "Application",
            type: "nvarchar(max)",
            nullable: true,
            oldClrType: typeof(bool),
            oldType: "bit",
            oldNullable: true);

        migrationBuilder.AlterColumn<string>(
            name: "PreviousApplication",
            table: "Application",
            type: "nvarchar(max)",
            nullable: true,
            oldClrType: typeof(bool),
            oldType: "bit",
            oldNullable: true);

        migrationBuilder.AlterColumn<string>(
            name: "LicenceConditionConfirmation",
            table: "Application",
            type: "nvarchar(max)",
            nullable: true,
            oldClrType: typeof(bool),
            oldType: "bit",
            oldNullable: true);

        migrationBuilder.AlterColumn<string>(
            name: "DirectorsFitAndProper",
            table: "Application",
            type: "nvarchar(max)",
            nullable: true,
            oldClrType: typeof(bool),
            oldType: "bit",
            oldNullable: true);

        migrationBuilder.AlterColumn<string>(
            name: "CompanyNumberCheck",
            table: "Application",
            type: "nvarchar(max)",
            nullable: true,
            oldClrType: typeof(bool),
            oldType: "bit",
            oldNullable: true);
    }
}
