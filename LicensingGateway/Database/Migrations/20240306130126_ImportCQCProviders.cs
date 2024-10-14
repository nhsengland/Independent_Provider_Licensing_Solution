using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Licensing.Gateway.Migrations;

/// <inheritdoc />
public partial class ImportCQCProviders : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "Address",
            table: "CQCProvider");

        migrationBuilder.AlterColumn<string>(
            name: "Postcode",
            table: "CQCProvider",
            type: "nvarchar(max)",
            nullable: true,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)");

        migrationBuilder.AlterColumn<string>(
            name: "CQCProviderID",
            table: "CQCProvider",
            type: "nvarchar(450)",
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)");

        migrationBuilder.AddColumn<string>(
            name: "Address_Line_1",
            table: "CQCProvider",
            type: "nvarchar(max)",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "Address_Line_2",
            table: "CQCProvider",
            type: "nvarchar(max)",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "Region",
            table: "CQCProvider",
            type: "nvarchar(max)",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "TownCity",
            table: "CQCProvider",
            type: "nvarchar(max)",
            nullable: true);

        migrationBuilder.CreateTable(
            name: "CQCProviderImportInstance",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_CQCProviderImportInstance", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "CQCProviderImportPage",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                CQCProviderImportInstanceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                PageNumber = table.Column<int>(type: "int", nullable: false),
                CQCProviderID = table.Column<string>(type: "nvarchar(max)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_CQCProviderImportPage", x => x.Id);
                table.ForeignKey(
                    name: "FK_CQCProviderImportPage_CQCProviderImportInstance_CQCProviderImportInstanceId",
                    column: x => x.CQCProviderImportInstanceId,
                    principalTable: "CQCProviderImportInstance",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_CQCProvider_CQCProviderID",
            table: "CQCProvider",
            column: "CQCProviderID");

        migrationBuilder.CreateIndex(
            name: "IX_CQCProviderImportPage_CQCProviderImportInstanceId",
            table: "CQCProviderImportPage",
            column: "CQCProviderImportInstanceId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "CQCProviderImportPage");

        migrationBuilder.DropTable(
            name: "CQCProviderImportInstance");

        migrationBuilder.DropIndex(
            name: "IX_CQCProvider_CQCProviderID",
            table: "CQCProvider");

        migrationBuilder.DropColumn(
            name: "Address_Line_1",
            table: "CQCProvider");

        migrationBuilder.DropColumn(
            name: "Address_Line_2",
            table: "CQCProvider");

        migrationBuilder.DropColumn(
            name: "Region",
            table: "CQCProvider");

        migrationBuilder.DropColumn(
            name: "TownCity",
            table: "CQCProvider");

        migrationBuilder.AlterColumn<string>(
            name: "Postcode",
            table: "CQCProvider",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: "",
            oldClrType: typeof(string),
            oldType: "nvarchar(max)",
            oldNullable: true);

        migrationBuilder.AlterColumn<string>(
            name: "CQCProviderID",
            table: "CQCProvider",
            type: "nvarchar(max)",
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(450)");

        migrationBuilder.AddColumn<string>(
            name: "Address",
            table: "CQCProvider",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: "");
    }
}
