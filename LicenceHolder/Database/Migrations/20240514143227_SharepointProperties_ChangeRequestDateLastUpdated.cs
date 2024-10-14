using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations;

/// <inheritdoc />
public partial class SharepointProperties_ChangeRequestDateLastUpdated : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "SharePointLocation",
            table: "Organisation",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: "");

        migrationBuilder.AddColumn<bool>(
            name: "IsActive",
            table: "Company",
            type: "bit",
            nullable: false,
            defaultValue: false);

        migrationBuilder.AddColumn<string>(
            name: "SharePointLocation",
            table: "Company",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: "");

        migrationBuilder.AlterColumn<DateTime>(
            name: "DateLastUpdated",
            table: "ChangeRequest",
            type: "datetime2",
            nullable: false,
            defaultValueSql: "GETUTCDATE()",
            oldClrType: typeof(DateTime),
            oldType: "datetime2");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "SharePointLocation",
            table: "Organisation");

        migrationBuilder.DropColumn(
            name: "IsActive",
            table: "Company");

        migrationBuilder.DropColumn(
            name: "SharePointLocation",
            table: "Company");

        migrationBuilder.AlterColumn<DateTime>(
            name: "DateLastUpdated",
            table: "ChangeRequest",
            type: "datetime2",
            nullable: false,
            oldClrType: typeof(DateTime),
            oldType: "datetime2",
            oldDefaultValueSql: "GETUTCDATE()");
    }
}
