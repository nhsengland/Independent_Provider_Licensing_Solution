using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Licensing.Gateway.Migrations;

/// <inheritdoc />
public partial class PreApplication_MainApplication_Changes : Migration
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

        migrationBuilder.AddColumn<string>(
            name: "CQCProviderPhoneNumber",
            table: "PreApplication",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: "");

        migrationBuilder.AddColumn<string>(
            name: "DirectorsIndividualsOrCorporate",
            table: "Application",
            type: "nvarchar(max)",
            nullable: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "CQCProviderPhoneNumber",
            table: "PreApplication");

        migrationBuilder.DropColumn(
            name: "DirectorsIndividualsOrCorporate",
            table: "Application");
    }
}
