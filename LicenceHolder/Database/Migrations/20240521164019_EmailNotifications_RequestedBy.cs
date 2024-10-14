using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations;

/// <inheritdoc />
public partial class EmailNotifications_RequestedBy : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<int>(
            name: "RequestedById",
            table: "EmailNotification",
            type: "int",
            nullable: true);

        migrationBuilder.CreateIndex(
            name: "IX_EmailNotification_RequestedById",
            table: "EmailNotification",
            column: "RequestedById");

        migrationBuilder.AddForeignKey(
            name: "FK_EmailNotification_User_RequestedById",
            table: "EmailNotification",
            column: "RequestedById",
            principalTable: "User",
            principalColumn: "Id");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_EmailNotification_User_RequestedById",
            table: "EmailNotification");

        migrationBuilder.DropIndex(
            name: "IX_EmailNotification_RequestedById",
            table: "EmailNotification");

        migrationBuilder.DropColumn(
            name: "RequestedById",
            table: "EmailNotification");
    }
}
