using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations;

/// <inheritdoc />
public partial class Amend_FMTasksDueDate_UserEmailUnique : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<string>(
            name: "Email",
            table: "User",
            type: "nvarchar(450)",
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)");

        migrationBuilder.AlterColumn<DateOnly>(
            name: "DateDue",
            table: "TaskForFinancialMonitoring",
            type: "date",
            nullable: true,
            oldClrType: typeof(DateOnly),
            oldType: "date");

        migrationBuilder.CreateIndex(
            name: "IX_User_Email",
            table: "User",
            column: "Email",
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropIndex(
            name: "IX_User_Email",
            table: "User");

        migrationBuilder.AlterColumn<string>(
            name: "Email",
            table: "User",
            type: "nvarchar(max)",
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(450)");

        migrationBuilder.AlterColumn<DateOnly>(
            name: "DateDue",
            table: "TaskForFinancialMonitoring",
            type: "date",
            nullable: false,
            defaultValue: new DateOnly(1, 1, 1),
            oldClrType: typeof(DateOnly),
            oldType: "date",
            oldNullable: true);
    }
}
