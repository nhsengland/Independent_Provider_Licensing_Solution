using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations;

/// <inheritdoc />
public partial class UpdateTaskStatusCompleted : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.UpdateData(
            table: "TaskStatus",
            keyColumn: "Id",
            keyValue: 200,
            column: "Status",
            value: "Completed");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.UpdateData(
            table: "TaskStatus",
            keyColumn: "Id",
            keyValue: 200,
            column: "Status",
            value: "Complete");
    }
}
