using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations
{
    /// <inheritdoc />
    public partial class Company_Address : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddressLine1",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "AddressLine2",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "City",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "County",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "Postcode",
                table: "Company");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Company",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Company");

            migrationBuilder.AddColumn<string>(
                name: "AddressLine1",
                table: "Company",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AddressLine2",
                table: "Company",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Company",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "County",
                table: "Company",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Postcode",
                table: "Company",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "");
        }
    }
}
