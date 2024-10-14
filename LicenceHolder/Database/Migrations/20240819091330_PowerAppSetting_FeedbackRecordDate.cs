using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations;

/// <inheritdoc />
public partial class PowerAppSetting_FeedbackRecordDate : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<bool>(
            name: "IsListed",
            table: "Organisation",
            type: "bit",
            nullable: false,
            defaultValue: false);

        migrationBuilder.AlterColumn<DateTime>(
            name: "DateGenerated",
            table: "Feedback",
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
            name: "IsListed",
            table: "Organisation");

        migrationBuilder.AlterColumn<DateTime>(
            name: "DateGenerated",
            table: "Feedback",
            type: "datetime2",
            nullable: false,
            oldClrType: typeof(DateTime),
            oldType: "datetime2",
            oldDefaultValueSql: "GETUTCDATE()");
    }
}
