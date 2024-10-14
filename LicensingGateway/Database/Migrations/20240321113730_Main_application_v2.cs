using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Licensing.Gateway.Migrations;

/// <inheritdoc />
public partial class Main_application_v2 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.RenameColumn(
            name: "PreApplicationID",
            table: "Application",
            newName: "LicenceConditionConfirmation");

        migrationBuilder.AddColumn<string>(
            name: "ApplicationCode",
            table: "Application",
            type: "nvarchar(max)",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "CQCProviderAddress",
            table: "Application",
            type: "nvarchar(max)",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "CQCProviderName",
            table: "Application",
            type: "nvarchar(max)",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "CompanyNumberCheck",
            table: "Application",
            type: "nvarchar(max)",
            nullable: true);

        migrationBuilder.CreateTable(
            name: "Director",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                ApplicationId = table.Column<int>(type: "int", nullable: false),
                Forename = table.Column<string>(type: "nvarchar(max)", nullable: true),
                Surname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                DateOfBirth = table.Column<DateOnly>(type: "date", nullable: true),
                Role = table.Column<string>(type: "nvarchar(max)", nullable: true),
                Organisation = table.Column<string>(type: "nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Director", x => x.Id);
                table.ForeignKey(
                    name: "FK_Director_Application_ApplicationId",
                    column: x => x.ApplicationId,
                    principalTable: "Application",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_Director_ApplicationId",
            table: "Director",
            column: "ApplicationId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Director");

        migrationBuilder.DropColumn(
            name: "ApplicationCode",
            table: "Application");

        migrationBuilder.DropColumn(
            name: "CQCProviderAddress",
            table: "Application");

        migrationBuilder.DropColumn(
            name: "CQCProviderName",
            table: "Application");

        migrationBuilder.DropColumn(
            name: "CompanyNumberCheck",
            table: "Application");

        migrationBuilder.RenameColumn(
            name: "LicenceConditionConfirmation",
            table: "Application",
            newName: "PreApplicationID");
    }
}
