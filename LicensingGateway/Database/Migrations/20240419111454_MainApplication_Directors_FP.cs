using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Licensing.Gateway.Migrations;

/// <inheritdoc />
public partial class MainApplication_Directors_FP : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("DELETE FROM [Application]", true);

        migrationBuilder.Sql("DELETE FROM [ApplicationPage]", true);

        migrationBuilder.Sql("DBCC CHECKIDENT('[ApplicationPage]', RESEED, 0)", true);

        foreach (var page in Enum.GetValues(typeof(Domain.Models.Database.ApplicationPage)))
        {
            migrationBuilder.Sql($"INSERT INTO [ApplicationPage] (PageName) VALUES ('{page}')");
        }

        migrationBuilder.RenameColumn(
            name: "DirectorsFitAndProper",
            table: "Application",
            newName: "DirectorsSatisfyG3FitAndProperRequirements");

        migrationBuilder.AddColumn<string>(
            name: "NoPreApplication_CQCProviderName",
            table: "ApplicationCode",
            type: "nvarchar(max)",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "NoPreApplication_Email",
            table: "ApplicationCode",
            type: "nvarchar(max)",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "NoPreApplication_FirstName",
            table: "ApplicationCode",
            type: "nvarchar(max)",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "NoPreApplication_LastName",
            table: "ApplicationCode",
            type: "nvarchar(max)",
            nullable: true);

        migrationBuilder.AddColumn<bool>(
            name: "DirectorsReadAndUnderstoodG3FitAndProper",
            table: "Application",
            type: "bit",
            nullable: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "NoPreApplication_CQCProviderName",
            table: "ApplicationCode");

        migrationBuilder.DropColumn(
            name: "NoPreApplication_Email",
            table: "ApplicationCode");

        migrationBuilder.DropColumn(
            name: "NoPreApplication_FirstName",
            table: "ApplicationCode");

        migrationBuilder.DropColumn(
            name: "NoPreApplication_LastName",
            table: "ApplicationCode");

        migrationBuilder.DropColumn(
            name: "DirectorsReadAndUnderstoodG3FitAndProper",
            table: "Application");

        migrationBuilder.RenameColumn(
            name: "DirectorsSatisfyG3FitAndProperRequirements",
            table: "Application",
            newName: "DirectorsFitAndProper");
    }
}
