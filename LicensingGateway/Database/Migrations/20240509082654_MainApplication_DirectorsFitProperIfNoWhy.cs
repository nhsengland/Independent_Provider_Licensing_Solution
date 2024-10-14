using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Licensing.Gateway.Migrations;

/// <inheritdoc />
public partial class MainApplication_DirectorsFitProperIfNoWhy : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "DirectorsSatisfyG3FitAndProperRequirementsIfNoWhy",
            table: "Application",
            type: "nvarchar(max)",
            nullable: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "DirectorsSatisfyG3FitAndProperRequirementsIfNoWhy",
            table: "Application");
    }
}
