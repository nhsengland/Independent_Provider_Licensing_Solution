using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Licensing.Gateway.Migrations;

/// <inheritdoc />
public partial class MainApplication_DeleteColumns : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "RegisteredWithCQC",
            table: "Application");

        migrationBuilder.DropColumn(
            name: "RegulatedByCQC",
            table: "Application");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<bool>(
            name: "RegisteredWithCQC",
            table: "Application",
            type: "bit",
            nullable: true);

        migrationBuilder.AddColumn<bool>(
            name: "RegulatedByCQC",
            table: "Application",
            type: "bit",
            nullable: true);
    }
}
