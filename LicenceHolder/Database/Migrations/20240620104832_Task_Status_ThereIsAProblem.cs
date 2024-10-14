using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations;

/// <inheritdoc />
public partial class Task_Status_ThereIsAProblem : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.InsertData(
            table: "TaskStatus",
            columns: new[] { "Id", "Status" },
            values: new object[] { 300, "ThereIsAProblem" });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DeleteData(
            table: "TaskStatus",
            keyColumn: "Id",
            keyValue: 300);
    }
}
