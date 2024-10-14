using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Licensing.Gateway.Migrations;

/// <inheritdoc />
public partial class CorporateAndParentCompanyDirectors : Migration
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
            name: "Type",
            table: "Director");

        migrationBuilder.RenameColumn(
            name: "TypeId",
            table: "Director",
            newName: "GroupId");

        migrationBuilder.AddColumn<bool>(
            name: "OneOrMoreParentCompanies",
            table: "Application",
            type: "bit",
            nullable: true);

        migrationBuilder.CreateTable(
            name: "DirectorGroup",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                ApplicationId = table.Column<int>(type: "int", nullable: false),
                Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                TypeId = table.Column<int>(type: "int", nullable: false),
                OneOrMoreIndividuals = table.Column<bool>(type: "bit", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_DirectorGroup", x => x.Id);
                table.ForeignKey(
                    name: "FK_DirectorGroup_Application_ApplicationId",
                    column: x => x.ApplicationId,
                    principalTable: "Application",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_DirectorGroup_DirectorType_TypeId",
                    column: x => x.TypeId,
                    principalTable: "DirectorType",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.InsertData(
            table: "DirectorType",
            columns: new[] { "Id", "Type" },
            values: new object[] { 3, "ParentCompany" });

        migrationBuilder.CreateIndex(
            name: "IX_Director_GroupId",
            table: "Director",
            column: "GroupId");

        migrationBuilder.CreateIndex(
            name: "IX_DirectorGroup_ApplicationId",
            table: "DirectorGroup",
            column: "ApplicationId");

        migrationBuilder.CreateIndex(
            name: "IX_DirectorGroup_TypeId",
            table: "DirectorGroup",
            column: "TypeId");

        migrationBuilder.AddForeignKey(
            name: "FK_Director_DirectorGroup_GroupId",
            table: "Director",
            column: "GroupId",
            principalTable: "DirectorGroup",
            principalColumn: "Id");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Director_DirectorGroup_GroupId",
            table: "Director");

        migrationBuilder.DropTable(
            name: "DirectorGroup");

        migrationBuilder.DropIndex(
            name: "IX_Director_GroupId",
            table: "Director");

        migrationBuilder.DeleteData(
            table: "DirectorType",
            keyColumn: "Id",
            keyValue: 3);

        migrationBuilder.DropColumn(
            name: "OneOrMoreParentCompanies",
            table: "Application");

        migrationBuilder.RenameColumn(
            name: "GroupId",
            table: "Director",
            newName: "TypeId");

        migrationBuilder.AddColumn<int>(
            name: "Type",
            table: "Director",
            type: "int",
            nullable: false,
            defaultValue: 0);
    }
}
