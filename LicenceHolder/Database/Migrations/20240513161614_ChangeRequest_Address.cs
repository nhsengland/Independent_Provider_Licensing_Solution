using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations;

/// <inheritdoc />
public partial class ChangeRequest_Address : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "AddressLine1",
            table: "ChangeRequest");

        migrationBuilder.DropColumn(
            name: "AddressLine2",
            table: "ChangeRequest");

        migrationBuilder.DropColumn(
            name: "City",
            table: "ChangeRequest");

        migrationBuilder.DropColumn(
            name: "County",
            table: "ChangeRequest");

        migrationBuilder.DropColumn(
            name: "Postcode",
            table: "ChangeRequest");

        migrationBuilder.AddColumn<string>(
            name: "Address",
            table: "ChangeRequest",
            type: "nvarchar(max)",
            nullable: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "Address",
            table: "ChangeRequest");

        migrationBuilder.AddColumn<string>(
            name: "AddressLine1",
            table: "ChangeRequest",
            type: "nvarchar(250)",
            maxLength: 250,
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "AddressLine2",
            table: "ChangeRequest",
            type: "nvarchar(250)",
            maxLength: 250,
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "City",
            table: "ChangeRequest",
            type: "nvarchar(250)",
            maxLength: 250,
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "County",
            table: "ChangeRequest",
            type: "nvarchar(250)",
            maxLength: 250,
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "Postcode",
            table: "ChangeRequest",
            type: "nvarchar(250)",
            maxLength: 250,
            nullable: true);
    }
}
