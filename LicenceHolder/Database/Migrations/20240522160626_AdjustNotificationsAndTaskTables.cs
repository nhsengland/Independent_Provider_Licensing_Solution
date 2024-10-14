using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations;

/// <inheritdoc />
public partial class AdjustNotificationsAndTaskTables : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("DELETE FROM [ChangeRequest]", true);

        migrationBuilder.AddColumn<string>(
            name: "TaskName",
            table: "TaskForFinancialMonitoring",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: "");

        migrationBuilder.AddColumn<string>(
            name: "TaskName",
            table: "TaskForAnnualCertificate",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: "");

        migrationBuilder.AddColumn<int>(
            name: "RaisedById",
            table: "ChangeRequest",
            type: "int",
            nullable: false,
            defaultValue: 0);

        migrationBuilder.CreateIndex(
            name: "IX_ChangeRequest_RaisedById",
            table: "ChangeRequest",
            column: "RaisedById");

        migrationBuilder.AddForeignKey(
            name: "FK_ChangeRequest_User_RaisedById",
            table: "ChangeRequest",
            column: "RaisedById",
            principalTable: "User",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_ChangeRequest_User_RaisedById",
            table: "ChangeRequest");

        migrationBuilder.DropIndex(
            name: "IX_ChangeRequest_RaisedById",
            table: "ChangeRequest");

        migrationBuilder.DropColumn(
            name: "TaskName",
            table: "TaskForFinancialMonitoring");

        migrationBuilder.DropColumn(
            name: "TaskName",
            table: "TaskForAnnualCertificate");

        migrationBuilder.DropColumn(
            name: "RaisedById",
            table: "ChangeRequest");
    }
}
