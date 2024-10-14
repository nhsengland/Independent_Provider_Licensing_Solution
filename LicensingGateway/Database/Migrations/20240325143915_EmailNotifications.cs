using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Licensing.Gateway.Migrations;

/// <inheritdoc />
public partial class EmailNotifications : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "EmailNotification",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                DateGenerated = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                PreApplicationId = table.Column<int>(type: "int", nullable: true),
                ApplicationId = table.Column<int>(type: "int", nullable: true),
                HasBeenSent = table.Column<bool>(type: "bit", nullable: false),
                DateSent = table.Column<DateTime>(type: "datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_EmailNotification", x => x.Id);
                table.ForeignKey(
                    name: "FK_EmailNotification_Application_ApplicationId",
                    column: x => x.ApplicationId,
                    principalTable: "Application",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_EmailNotification_PreApplication_PreApplicationId",
                    column: x => x.PreApplicationId,
                    principalTable: "PreApplication",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateIndex(
            name: "IX_EmailNotification_ApplicationId",
            table: "EmailNotification",
            column: "ApplicationId");

        migrationBuilder.CreateIndex(
            name: "IX_EmailNotification_PreApplicationId",
            table: "EmailNotification",
            column: "PreApplicationId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "EmailNotification");
    }
}
