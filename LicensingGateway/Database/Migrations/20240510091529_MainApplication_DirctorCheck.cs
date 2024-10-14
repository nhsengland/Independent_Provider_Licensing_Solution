using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Licensing.Gateway.Migrations;

/// <inheritdoc />
public partial class MainApplication_DirctorCheck : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("DELETE FROM [EmailNotification]", true);

        migrationBuilder.Sql("DELETE FROM [Director]", true);

        migrationBuilder.Sql("DELETE FROM [Application]", true);

        migrationBuilder.Sql("DELETE FROM [ApplicationPage]", true);

        migrationBuilder.Sql("DBCC CHECKIDENT('[ApplicationPage]', RESEED, 0)", true);

        foreach (var page in Enum.GetValues(typeof(Domain.Models.Database.ApplicationPage)))
        {
            migrationBuilder.Sql($"INSERT INTO [ApplicationPage] (PageName) VALUES ('{page}')");
        }

        migrationBuilder.AddColumn<bool>(
            name: "DirectorsCheck",
            table: "Application",
            type: "bit",
            nullable: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "DirectorsCheck",
            table: "Application");
    }
}
