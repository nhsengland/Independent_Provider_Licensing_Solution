using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Database.Migrations;

/// <inheritdoc />
public partial class EmailNotifications : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "EmailNotificationType",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Type = table.Column<string>(type: "nvarchar(max)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_EmailNotificationType", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "EmailNotification",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                HasBeenSent = table.Column<bool>(type: "bit", nullable: false),
                DateSent = table.Column<DateTime>(type: "datetime2", nullable: true),
                TypeId = table.Column<int>(type: "int", nullable: false),
                UserId = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_EmailNotification", x => x.Id);
                table.ForeignKey(
                    name: "FK_EmailNotification_EmailNotificationType_TypeId",
                    column: x => x.TypeId,
                    principalTable: "EmailNotificationType",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_EmailNotification_User_UserId",
                    column: x => x.UserId,
                    principalTable: "User",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.InsertData(
            table: "EmailNotificationType",
            columns: new[] { "Id", "Type" },
            values: new object[,]
            {
                { 1, "InviteUser" },
                { 2, "ReSendInvite" }
            });

        migrationBuilder.CreateIndex(
            name: "IX_EmailNotification_TypeId",
            table: "EmailNotification",
            column: "TypeId");

        migrationBuilder.CreateIndex(
            name: "IX_EmailNotification_UserId",
            table: "EmailNotification",
            column: "UserId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "EmailNotification");

        migrationBuilder.DropTable(
            name: "EmailNotificationType");
    }
}
