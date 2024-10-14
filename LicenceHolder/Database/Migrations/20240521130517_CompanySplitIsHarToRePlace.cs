using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations;

/// <inheritdoc />
public partial class CompanySplitIsHarToRePlace : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.RenameColumn(
            name: "IsCrsOrHardToReplace",
            table: "Company",
            newName: "IsHardToReplace");

        migrationBuilder.AddColumn<bool>(
            name: "IsCrs",
            table: "Company",
            type: "bit",
            nullable: false,
            defaultValue: false);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "IsCrs",
            table: "Company");

        migrationBuilder.RenameColumn(
            name: "IsHardToReplace",
            table: "Company",
            newName: "IsCrsOrHardToReplace");
    }
}
