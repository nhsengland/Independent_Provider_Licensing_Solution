using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations;

/// <inheritdoc />
public partial class Notifications : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Message");

        migrationBuilder.CreateTable(
            name: "Notification",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                OrganisationId = table.Column<int>(type: "int", nullable: false),
                Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Body = table.Column<string>(type: "nvarchar(max)", nullable: false),
                From = table.Column<string>(type: "nvarchar(max)", nullable: false),
                SendDateTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                IsRead = table.Column<bool>(type: "bit", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Notification", x => x.Id);
                table.ForeignKey(
                    name: "FK_Notification_Organisation_OrganisationId",
                    column: x => x.OrganisationId,
                    principalTable: "Organisation",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_Notification_OrganisationId",
            table: "Notification",
            column: "OrganisationId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Notification");

        migrationBuilder.CreateTable(
            name: "Message",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                CompanyId = table.Column<int>(type: "int", nullable: false),
                Body = table.Column<string>(type: "nvarchar(max)", nullable: false),
                From = table.Column<string>(type: "nvarchar(max)", nullable: false),
                IsRead = table.Column<bool>(type: "bit", nullable: false),
                SendDateTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                Title = table.Column<string>(type: "nvarchar(max)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Message", x => x.Id);
                table.ForeignKey(
                    name: "FK_Message_Company_CompanyId",
                    column: x => x.CompanyId,
                    principalTable: "Company",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_Message_CompanyId",
            table: "Message",
            column: "CompanyId");
    }
}
