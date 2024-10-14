using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Licensing.Gateway.Migrations;

/// <inheritdoc />
public partial class main_application_v1 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "CRSCode",
            table: "PreApplication");

        migrationBuilder.DropColumn(
            name: "CRSRequest",
            table: "PreApplication");

        migrationBuilder.AlterColumn<string>(
            name: "Turnover",
            table: "PreApplication",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: "",
            oldClrType: typeof(string),
            oldType: "nvarchar(max)",
            oldNullable: true);

        migrationBuilder.AlterColumn<string>(
            name: "IsHealthCareService",
            table: "PreApplication",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: "",
            oldClrType: typeof(string),
            oldType: "nvarchar(max)",
            oldNullable: true);

        migrationBuilder.AlterColumn<string>(
            name: "IsExclusive",
            table: "PreApplication",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: "",
            oldClrType: typeof(string),
            oldType: "nvarchar(max)",
            oldNullable: true);

        migrationBuilder.AddColumn<int>(
            name: "NumberOfAttemptsToImport",
            table: "CQCProviderImportPage",
            type: "int",
            nullable: false,
            defaultValue: 0);

        migrationBuilder.AddColumn<int>(
            name: "StatusCode",
            table: "CQCProviderImportPage",
            type: "int",
            nullable: true);

        migrationBuilder.CreateTable(
            name: "Application",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                DateGenerated = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                DateModified = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                PreApplicationID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                CQCProviderID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                CompanyNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                LegalForm = table.Column<string>(type: "nvarchar(max)", nullable: true),
                RegulatedByCQC = table.Column<string>(type: "nvarchar(max)", nullable: true),
                RegisteredWithCQC = table.Column<string>(type: "nvarchar(max)", nullable: true),
                Forename = table.Column<string>(type: "nvarchar(max)", nullable: true),
                Surname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                JobTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                Email = table.Column<string>(type: "nvarchar(254)", maxLength: 254, nullable: true),
                SubmitApplication = table.Column<bool>(type: "bit", nullable: false),
                ReferenceId = table.Column<string>(type: "nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Application", x => x.Id);
            });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Application");

        migrationBuilder.DropColumn(
            name: "NumberOfAttemptsToImport",
            table: "CQCProviderImportPage");

        migrationBuilder.DropColumn(
            name: "StatusCode",
            table: "CQCProviderImportPage");

        migrationBuilder.AlterColumn<string>(
            name: "Turnover",
            table: "PreApplication",
            type: "nvarchar(max)",
            nullable: true,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)");

        migrationBuilder.AlterColumn<string>(
            name: "IsHealthCareService",
            table: "PreApplication",
            type: "nvarchar(max)",
            nullable: true,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)");

        migrationBuilder.AlterColumn<string>(
            name: "IsExclusive",
            table: "PreApplication",
            type: "nvarchar(max)",
            nullable: true,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)");

        migrationBuilder.AddColumn<string>(
            name: "CRSCode",
            table: "PreApplication",
            type: "nvarchar(max)",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "CRSRequest",
            table: "PreApplication",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: "");
    }
}
