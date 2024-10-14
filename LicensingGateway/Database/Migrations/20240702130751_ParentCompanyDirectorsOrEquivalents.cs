using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Licensing.Gateway.Migrations;

/// <inheritdoc />
public partial class ParentCompanyDirectorsOrEquivalents : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("update [ApplicationPage] set PageName = 'ParentCompanyDirectorsOrEquivalents' where Id = 25");
        migrationBuilder.Sql("update [ApplicationPage] set PageName = 'ParentCompanyDirectors' where Id = 26");
        migrationBuilder.Sql("update [ApplicationPage] set PageName = 'DirectorsSatisfyG3FitAndProperRequirements' where Id = 27");
        migrationBuilder.Sql("update [ApplicationPage] set PageName = 'UltimateController' where Id = 28");
        migrationBuilder.Sql("update [ApplicationPage] set PageName = 'UltimateControllers' where Id = 29");
        migrationBuilder.Sql("update [ApplicationPage] set PageName = 'Review' where Id = 30");
        migrationBuilder.Sql("insert into [ApplicationPage] (PageName) values ('Submitted')");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("update [ApplicationPage] set PageName = 'ParentCompanyDirectors' where Id = 25");
        migrationBuilder.Sql("update [ApplicationPage] set PageName = 'DirectorsSatisfyG3FitAndProperRequirements' where Id = 26");
        migrationBuilder.Sql("update [ApplicationPage] set PageName = 'UltimateController' where Id = 27");
        migrationBuilder.Sql("update [ApplicationPage] set PageName = 'UltimateControllers' where Id = 28");
        migrationBuilder.Sql("update [ApplicationPage] set PageName = 'Review' where Id = 29");
        migrationBuilder.Sql("update [ApplicationPage] set PageName = 'Submitted' where Id = 30");
        migrationBuilder.Sql("delete from ApplicationPage where Id = 31");
        migrationBuilder.Sql("DBCC CHECKIDENT('[ApplicationPage]', RESEED, 30)");
    }
}
