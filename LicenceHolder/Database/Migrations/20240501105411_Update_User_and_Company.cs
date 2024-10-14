using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations;

/// <inheritdoc />
public partial class Update_User_and_Company : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<string>(
            name: "OktaId",
            table: "User",
            type: "nvarchar(50)",
            maxLength: 50,
            nullable: true,
            oldClrType: typeof(string),
            oldType: "nvarchar(50)",
            oldMaxLength: 50);

        migrationBuilder.AddColumn<string>(
            name: "CQCProviderID",
            table: "Company",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: "");

        migrationBuilder.AddColumn<bool>(
            name: "IsCrsOrHardToReplace",
            table: "Company",
            type: "bit",
            nullable: false,
            defaultValue: false);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "CQCProviderID",
            table: "Company");

        migrationBuilder.DropColumn(
            name: "IsCrsOrHardToReplace",
            table: "Company");

        migrationBuilder.AlterColumn<string>(
            name: "OktaId",
            table: "User",
            type: "nvarchar(50)",
            maxLength: 50,
            nullable: false,
            defaultValue: "",
            oldClrType: typeof(string),
            oldType: "nvarchar(50)",
            oldMaxLength: 50,
            oldNullable: true);
    }
}
