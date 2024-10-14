using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Licensing.Gateway.Migrations;

/// <inheritdoc />
public partial class MainApplication_RemoveColumns_ResetApplicationPage : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("DELETE FROM [EmailNotification]", true);

        migrationBuilder.Sql("DELETE FROM [Application]", true);

        migrationBuilder.Sql("DELETE FROM [ApplicationPage]", true);

        migrationBuilder.Sql("DBCC CHECKIDENT('[ApplicationPage]', RESEED, 0)", true);

        foreach (var page in Enum.GetValues(typeof(Domain.Models.Database.ApplicationPage)))
        {
            migrationBuilder.Sql($"INSERT INTO [ApplicationPage] (PageName) VALUES ('{page}')");
        }

        migrationBuilder.DropColumn(
            name: "DirectorsIndividualsOrCorporate",
            table: "Application");

        migrationBuilder.DropColumn(
            name: "DirectorsReadAndUnderstoodG3FitAndProper",
            table: "Application");

        migrationBuilder.DropColumn(
            name: "LicenceConditionConfirmation",
            table: "Application");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "DirectorsIndividualsOrCorporate",
            table: "Application",
            type: "nvarchar(max)",
            nullable: true);

        migrationBuilder.AddColumn<bool>(
            name: "DirectorsReadAndUnderstoodG3FitAndProper",
            table: "Application",
            type: "bit",
            nullable: true);

        migrationBuilder.AddColumn<bool>(
            name: "LicenceConditionConfirmation",
            table: "Application",
            type: "bit",
            nullable: true);
    }
}
