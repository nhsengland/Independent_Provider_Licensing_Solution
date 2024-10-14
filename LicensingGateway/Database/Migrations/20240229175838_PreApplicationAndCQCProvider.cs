using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Licensing.Gateway.Migrations;

/// <inheritdoc />
public partial class PreApplicationAndCQCProvider : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "CQCProvider",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Postcode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                CQCProviderID = table.Column<string>(type: "nvarchar(max)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_CQCProvider", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "PreApplication",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                DateGenerated = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                ReferenceId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                CQCProviderID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                CQCProviderName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                CQCProviderAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                CRSRequest = table.Column<string>(type: "nvarchar(max)", nullable: false),
                CRSCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                IsHealthCareService = table.Column<string>(type: "nvarchar(max)", nullable: true),
                IsExclusive = table.Column<string>(type: "nvarchar(max)", nullable: true),
                Turnover = table.Column<string>(type: "nvarchar(max)", nullable: true),
                FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                JobTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Email = table.Column<string>(type: "nvarchar(max)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_PreApplication", x => x.Id);
            });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "CQCProvider");

        migrationBuilder.DropTable(
            name: "PreApplication");
    }
}
