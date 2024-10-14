using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Licensing.Gateway.Migrations;

/// <inheritdoc />
public partial class Application_Remove_LegalForm_Amend_UlitimateController : Migration
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

        migrationBuilder.DropColumn(
            name: "LegalForm",
            table: "Application");

        migrationBuilder.DropColumn(
            name: "UltimateControllerName",
            table: "Application");

        migrationBuilder.CreateTable(
            name: "UltimateController",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                ApplicationId = table.Column<int>(type: "int", nullable: false),
                Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_UltimateController", x => x.Id);
                table.ForeignKey(
                    name: "FK_UltimateController_Application_ApplicationId",
                    column: x => x.ApplicationId,
                    principalTable: "Application",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_UltimateController_ApplicationId",
            table: "UltimateController",
            column: "ApplicationId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "UltimateController");

        migrationBuilder.AddColumn<string>(
            name: "LegalForm",
            table: "Application",
            type: "nvarchar(max)",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "UltimateControllerName",
            table: "Application",
            type: "nvarchar(max)",
            nullable: true);
    }
}
