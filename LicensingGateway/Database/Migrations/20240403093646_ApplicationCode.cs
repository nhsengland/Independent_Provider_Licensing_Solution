using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Licensing.Gateway.Migrations;

/// <inheritdoc />
public partial class ApplicationCode : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("DELETE FROM [EmailNotification]", true);

        migrationBuilder.Sql("DELETE FROM [Application]", true);

        migrationBuilder.RenameColumn(
            name: "ApplicationCode",
            table: "Application",
            newName: "UltimateControllerName");

        migrationBuilder.AddColumn<int>(
            name: "ApplicationCodeId",
            table: "Application",
            type: "int",
            nullable: false,
            defaultValue: 0);

        migrationBuilder.AddColumn<int>(
            name: "CurrentPageId",
            table: "Application",
            type: "int",
            nullable: false,
            defaultValue: 0);

        migrationBuilder.AddColumn<string>(
            name: "DirectorsFitAndProper",
            table: "Application",
            type: "nvarchar(max)",
            nullable: true);

        migrationBuilder.AddColumn<bool>(
            name: "ElectronicCommunications",
            table: "Application",
            type: "bit",
            nullable: false,
            defaultValue: false);

        migrationBuilder.AddColumn<DateOnly>(
            name: "LastFinancialYear",
            table: "Application",
            type: "date",
            nullable: true);

        migrationBuilder.AddColumn<DateOnly>(
            name: "NextFinancialYear",
            table: "Application",
            type: "date",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "PreviousApplication",
            table: "Application",
            type: "nvarchar(max)",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "UltimateController",
            table: "Application",
            type: "nvarchar(max)",
            nullable: true);

        migrationBuilder.CreateTable(
            name: "ApplicationCode",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Code = table.Column<string>(type: "nvarchar(450)", nullable: false),
                IsCRS = table.Column<bool>(type: "bit", nullable: false),
                IsHardToReplace = table.Column<bool>(type: "bit", nullable: false),
                PreApplicationId = table.Column<int>(type: "int", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ApplicationCode", x => x.Id);
                table.ForeignKey(
                    name: "FK_ApplicationCode_PreApplication_PreApplicationId",
                    column: x => x.PreApplicationId,
                    principalTable: "PreApplication",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateTable(
            name: "ApplicationPage",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                PageName = table.Column<string>(type: "nvarchar(50)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ApplicationPage", x => x.Id);
            });

        migrationBuilder.CreateIndex(
            name: "IX_Application_ApplicationCodeId",
            table: "Application",
            column: "ApplicationCodeId",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Application_CurrentPageId",
            table: "Application",
            column: "CurrentPageId");

        migrationBuilder.CreateIndex(
            name: "IX_ApplicationCode_Code",
            table: "ApplicationCode",
            column: "Code",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_ApplicationCode_PreApplicationId",
            table: "ApplicationCode",
            column: "PreApplicationId");

        migrationBuilder.AddForeignKey(
            name: "FK_Application_ApplicationCode_ApplicationCodeId",
            table: "Application",
            column: "ApplicationCodeId",
            principalTable: "ApplicationCode",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_Application_ApplicationPage_CurrentPageId",
            table: "Application",
            column: "CurrentPageId",
            principalTable: "ApplicationPage",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        foreach (var page in Enum.GetValues(typeof(Domain.Models.Database.ApplicationPage)))
        {
            migrationBuilder.Sql($"INSERT INTO [ApplicationPage] (PageName) VALUES ('{page}')");
        }
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Application_ApplicationCode_ApplicationCodeId",
            table: "Application");

        migrationBuilder.DropForeignKey(
            name: "FK_Application_ApplicationPage_CurrentPageId",
            table: "Application");

        migrationBuilder.DropTable(
            name: "ApplicationCode");

        migrationBuilder.DropTable(
            name: "ApplicationPage");

        migrationBuilder.DropIndex(
            name: "IX_Application_ApplicationCodeId",
            table: "Application");

        migrationBuilder.DropIndex(
            name: "IX_Application_CurrentPageId",
            table: "Application");

        migrationBuilder.DropColumn(
            name: "ApplicationCodeId",
            table: "Application");

        migrationBuilder.DropColumn(
            name: "CurrentPageId",
            table: "Application");

        migrationBuilder.DropColumn(
            name: "DirectorsFitAndProper",
            table: "Application");

        migrationBuilder.DropColumn(
            name: "ElectronicCommunications",
            table: "Application");

        migrationBuilder.DropColumn(
            name: "LastFinancialYear",
            table: "Application");

        migrationBuilder.DropColumn(
            name: "NextFinancialYear",
            table: "Application");

        migrationBuilder.DropColumn(
            name: "PreviousApplication",
            table: "Application");

        migrationBuilder.DropColumn(
            name: "UltimateController",
            table: "Application");

        migrationBuilder.RenameColumn(
            name: "UltimateControllerName",
            table: "Application",
            newName: "ApplicationCode");
    }
}
