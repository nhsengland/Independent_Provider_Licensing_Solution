using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Licensing.Gateway.Migrations;

/// <inheritdoc />
public partial class CQCProviderRegulatedActivites : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<bool>(
            name: "ConfirmationOfRegulatedActivities",
            table: "PreApplication",
            type: "bit",
            nullable: false,
            defaultValue: false);

        migrationBuilder.AddColumn<string>(
            name: "RegulatedActivities",
            table: "PreApplication",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: "");

        migrationBuilder.CreateTable(
            name: "CQCProviderRegulatedActivity",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Code = table.Column<string>(type: "nvarchar(450)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_CQCProviderRegulatedActivity", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "CQCProviderToRegulatedActivities",
            columns: table => new
            {
                CQCProviderId = table.Column<int>(type: "int", nullable: false),
                ActivityId = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_CQCProviderToRegulatedActivities", x => new { x.CQCProviderId, x.ActivityId });
                table.ForeignKey(
                    name: "FK_CQCProviderToRegulatedActivities_CQCProviderRegulatedActivity_ActivityId",
                    column: x => x.ActivityId,
                    principalTable: "CQCProviderRegulatedActivity",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_CQCProviderToRegulatedActivities_CQCProvider_CQCProviderId",
                    column: x => x.CQCProviderId,
                    principalTable: "CQCProvider",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_CQCProviderRegulatedActivities_Code",
            table: "CQCProviderRegulatedActivity",
            column: "Code");

        migrationBuilder.CreateIndex(
            name: "IX_CQCProviderToRegulatedActivities_ActivityId",
            table: "CQCProviderToRegulatedActivities",
            column: "ActivityId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "CQCProviderToRegulatedActivities");

        migrationBuilder.DropTable(
            name: "CQCProviderRegulatedActivity");

        migrationBuilder.DropColumn(
            name: "ConfirmationOfRegulatedActivities",
            table: "PreApplication");

        migrationBuilder.DropColumn(
            name: "RegulatedActivities",
            table: "PreApplication");
    }
}
