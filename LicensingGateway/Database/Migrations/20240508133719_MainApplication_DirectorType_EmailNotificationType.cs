using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Database.Licensing.Gateway.Migrations;

/// <inheritdoc />
public partial class MainApplication_DirectorType_EmailNotificationType : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("DELETE FROM [EmailNotification]", true);

        migrationBuilder.Sql("DELETE FROM [Director]", true);

        migrationBuilder.Sql("DELETE FROM [Application]", true);

        migrationBuilder.Sql("DELETE FROM [ApplicationPage]", true);

        migrationBuilder.Sql("DBCC CHECKIDENT('[ApplicationPage]', RESEED, 0)", true);

        foreach (var page in Enum.GetValues(typeof(Domain.Models.Database.ApplicationPage)))
        {
            migrationBuilder.Sql($"INSERT INTO [ApplicationPage] (PageName) VALUES ('{page}')");
        }

        migrationBuilder.DropColumn(
            name: "Organisation",
            table: "Director");

        migrationBuilder.DropColumn(
            name: "Role",
            table: "Director");

        migrationBuilder.DropColumn(
            name: "PreviousApplication",
            table: "Application");

        migrationBuilder.DropColumn(
            name: "PreviousLicenseLicenseNumber",
            table: "Application");

        migrationBuilder.DropColumn(
            name: "PreviousLicenseProviderName",
            table: "Application");

        migrationBuilder.RenameColumn(
            name: "PreviousLicense",
            table: "Application",
            newName: "CorporateDirectorsCheck");

        migrationBuilder.AddColumn<string>(
            name: "ApplicationURL",
            table: "EmailNotification",
            type: "nvarchar(max)",
            nullable: true);

        migrationBuilder.AddColumn<int>(
            name: "TypeId",
            table: "EmailNotification",
            type: "int",
            nullable: false,
            defaultValue: 0);

        migrationBuilder.AddColumn<int>(
            name: "Type",
            table: "Director",
            type: "int",
            nullable: false,
            defaultValue: 0);

        migrationBuilder.AddColumn<int>(
            name: "TypeId",
            table: "Director",
            type: "int",
            nullable: false,
            defaultValue: 0);

        migrationBuilder.CreateTable(
            name: "DirectorType",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Type = table.Column<string>(type: "nvarchar(max)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_DirectorType", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "EmailNotificationType",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Type = table.Column<string>(type: "nvarchar(max)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_EmailNotificationType", x => x.Id);
            });

        migrationBuilder.InsertData(
            table: "DirectorType",
            columns: new[] { "Id", "Type" },
            values: new object[,]
            {
                { 1, "Board" },
                { 2, "Corporate" }
            });

        migrationBuilder.InsertData(
            table: "EmailNotificationType",
            columns: new[] { "Id", "Type" },
            values: new object[,]
            {
                { 1, "PreApplication" },
                { 2, "Application" },
                { 3, "MainApplicationSaveAndExit" }
            });

        migrationBuilder.CreateIndex(
            name: "IX_EmailNotification_TypeId",
            table: "EmailNotification",
            column: "TypeId");

        migrationBuilder.AddForeignKey(
            name: "FK_EmailNotification_EmailNotificationType_TypeId",
            table: "EmailNotification",
            column: "TypeId",
            principalTable: "EmailNotificationType",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_EmailNotification_EmailNotificationType_TypeId",
            table: "EmailNotification");

        migrationBuilder.DropTable(
            name: "DirectorType");

        migrationBuilder.DropTable(
            name: "EmailNotificationType");

        migrationBuilder.DropIndex(
            name: "IX_EmailNotification_TypeId",
            table: "EmailNotification");

        migrationBuilder.DropColumn(
            name: "ApplicationURL",
            table: "EmailNotification");

        migrationBuilder.DropColumn(
            name: "TypeId",
            table: "EmailNotification");

        migrationBuilder.DropColumn(
            name: "Type",
            table: "Director");

        migrationBuilder.DropColumn(
            name: "TypeId",
            table: "Director");

        migrationBuilder.RenameColumn(
            name: "CorporateDirectorsCheck",
            table: "Application",
            newName: "PreviousLicense");

        migrationBuilder.AddColumn<string>(
            name: "Organisation",
            table: "Director",
            type: "nvarchar(max)",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "Role",
            table: "Director",
            type: "nvarchar(max)",
            nullable: true);

        migrationBuilder.AddColumn<bool>(
            name: "PreviousApplication",
            table: "Application",
            type: "bit",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "PreviousLicenseLicenseNumber",
            table: "Application",
            type: "nvarchar(max)",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "PreviousLicenseProviderName",
            table: "Application",
            type: "nvarchar(max)",
            nullable: true);
    }
}
