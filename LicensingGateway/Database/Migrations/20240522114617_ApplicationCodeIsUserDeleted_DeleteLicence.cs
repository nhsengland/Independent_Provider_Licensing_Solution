using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Licensing.Gateway.Migrations;

/// <inheritdoc />
public partial class ApplicationCodeIsUserDeleted_DeleteLicence : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "License");

        migrationBuilder.AddColumn<bool>(
            name: "IsUserDeleted",
            table: "ApplicationCode",
            type: "bit",
            nullable: false,
            defaultValue: false);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "IsUserDeleted",
            table: "ApplicationCode");

        migrationBuilder.CreateTable(
            name: "License",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                DateGenerated = table.Column<DateOnly>(type: "date", nullable: false),
                LicenseKey = table.Column<string>(type: "nvarchar(max)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_License", x => x.Id);
            });
    }
}
