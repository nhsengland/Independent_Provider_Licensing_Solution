using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations;

/// <inheritdoc />
public partial class ChangeRequestCompanyName : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "Name",
            table: "ChangeRequest",
            type: "nvarchar(max)",
            nullable: true);

        migrationBuilder.InsertData(
            table: "ChangeRequestType",
            columns: new[] { "Id", "Type" },
            values: new object[] { 300, "Name" });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DeleteData(
            table: "ChangeRequestType",
            keyColumn: "Id",
            keyValue: 300);

        migrationBuilder.DropColumn(
            name: "Name",
            table: "ChangeRequest");
    }
}
