using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations;

/// <inheritdoc />
public partial class CQCProviderAlreadyHasLicence : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("INSERT INTO ApplicationPage(PageName) Values('Submitted')");

        migrationBuilder.Sql("UPDATE ApplicationPage Set PageName = 'ApplicationCode' where Id = 1");
        migrationBuilder.Sql("UPDATE ApplicationPage Set PageName = 'CQCProviderID' where Id = 2");
        migrationBuilder.Sql("UPDATE ApplicationPage Set PageName = 'CQCProviderConfirmation' where Id = 3");
        migrationBuilder.Sql("UPDATE ApplicationPage Set PageName = 'CQCProviderNotFound' where Id = 4");
        migrationBuilder.Sql("UPDATE ApplicationPage Set PageName = 'CQCProviderHasActiveLicence' where Id = 5");
        migrationBuilder.Sql("UPDATE ApplicationPage Set PageName = 'CQCProviderChange' where Id = 6");
        migrationBuilder.Sql("UPDATE ApplicationPage Set PageName = 'CQCProviderChangeName' where Id = 7");
        migrationBuilder.Sql("UPDATE ApplicationPage Set PageName = 'CQCProviderChangeAddress' where Id = 8");
        migrationBuilder.Sql("UPDATE ApplicationPage Set PageName = 'CQCProviderChangePhoneNumber' where Id = 9");
        migrationBuilder.Sql("UPDATE ApplicationPage Set PageName = 'CQCProviderChangeWebsite' where Id = 10");
        migrationBuilder.Sql("UPDATE ApplicationPage Set PageName = 'CompanyNumberCheck' where Id = 11");
        migrationBuilder.Sql("UPDATE ApplicationPage Set PageName = 'CompanyNumber' where Id = 12");
        migrationBuilder.Sql("UPDATE ApplicationPage Set PageName = 'NewlyIncorporatedCompany' where Id = 13");
        migrationBuilder.Sql("UPDATE ApplicationPage Set PageName = 'LastFinancialYear' where Id = 14");
        migrationBuilder.Sql("UPDATE ApplicationPage Set PageName = 'NextFinancialYear' where Id = 15");
        migrationBuilder.Sql("UPDATE ApplicationPage Set PageName = 'DirectorCheck' where Id = 16");
        migrationBuilder.Sql("UPDATE ApplicationPage Set PageName = 'Directors' where Id = 17");
        migrationBuilder.Sql("UPDATE ApplicationPage Set PageName = 'CorporateDirectorCheck' where Id = 18");
        migrationBuilder.Sql("UPDATE ApplicationPage Set PageName = 'CorporateBodies' where Id = 19");
        migrationBuilder.Sql("UPDATE ApplicationPage Set PageName = 'CorporateBodyName' where Id = 20");
        migrationBuilder.Sql("UPDATE ApplicationPage Set PageName = 'CorporateBodyIndividualsOrCompany' where Id = 21");
        migrationBuilder.Sql("UPDATE ApplicationPage Set PageName = 'CorporateDirectors' where Id = 22");
        migrationBuilder.Sql("UPDATE ApplicationPage Set PageName = 'ParentCompaniesCheck' where Id = 23");
        migrationBuilder.Sql("UPDATE ApplicationPage Set PageName = 'ParentCompanies' where Id = 24");
        migrationBuilder.Sql("UPDATE ApplicationPage Set PageName = 'ParentCompanyName' where Id = 25");
        migrationBuilder.Sql("UPDATE ApplicationPage Set PageName = 'ParentCompanyDirectorsOrEquivalents' where Id = 26");
        migrationBuilder.Sql("UPDATE ApplicationPage Set PageName = 'ParentCompanyDirectors' where Id = 27");
        migrationBuilder.Sql("UPDATE ApplicationPage Set PageName = 'DirectorsSatisfyG3FitAndProperRequirements' where Id = 28");
        migrationBuilder.Sql("UPDATE ApplicationPage Set PageName = 'UltimateController' where Id = 29");
        migrationBuilder.Sql("UPDATE ApplicationPage Set PageName = 'UltimateControllers' where Id = 30");
        migrationBuilder.Sql("UPDATE ApplicationPage Set PageName = 'Review' where Id = 31");
        migrationBuilder.Sql("UPDATE ApplicationPage Set PageName = 'Submitted' where Id = 32");


        migrationBuilder.Sql("update Application set CurrentPageId = CurrentPageId + 1 where CurrentPageId > 4");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("update Application set CurrentPageId = CurrentPageId - 1 where CurrentPageId > 4");

        migrationBuilder.Sql("delete from ApplicationPage where Id = 32");

        migrationBuilder.Sql("DBCC CHECKIDENT('[ApplicationPage]', RESEED, 31)");

        migrationBuilder.Sql("UPDATE ApplicationPage Set PageName = 'ApplicationCode' where Id = 1");
        migrationBuilder.Sql("UPDATE ApplicationPage Set PageName = 'CQCProviderID' where Id = 2");
        migrationBuilder.Sql("UPDATE ApplicationPage Set PageName = 'CQCProviderConfirmation' where Id = 3");
        migrationBuilder.Sql("UPDATE ApplicationPage Set PageName = 'CQCProviderNotFound' where Id = 4");
        migrationBuilder.Sql("UPDATE ApplicationPage Set PageName = 'CQCProviderChange' where Id = 5");
        migrationBuilder.Sql("UPDATE ApplicationPage Set PageName = 'CQCProviderChangeName' where Id = 6");
        migrationBuilder.Sql("UPDATE ApplicationPage Set PageName = 'CQCProviderChangeAddress' where Id = 7");
        migrationBuilder.Sql("UPDATE ApplicationPage Set PageName = 'CQCProviderChangePhoneNumber' where Id = 8");
        migrationBuilder.Sql("UPDATE ApplicationPage Set PageName = 'CQCProviderChangeWebsite' where Id = 9");
        migrationBuilder.Sql("UPDATE ApplicationPage Set PageName = 'CompanyNumberCheck' where Id = 10");
        migrationBuilder.Sql("UPDATE ApplicationPage Set PageName = 'CompanyNumber' where Id = 11");
        migrationBuilder.Sql("UPDATE ApplicationPage Set PageName = 'NewlyIncorporatedCompany' where Id = 12");
        migrationBuilder.Sql("UPDATE ApplicationPage Set PageName = 'LastFinancialYear' where Id = 13");
        migrationBuilder.Sql("UPDATE ApplicationPage Set PageName = 'NextFinancialYear' where Id = 14");
        migrationBuilder.Sql("UPDATE ApplicationPage Set PageName = 'DirectorCheck' where Id = 15");
        migrationBuilder.Sql("UPDATE ApplicationPage Set PageName = 'Directors' where Id = 16");
        migrationBuilder.Sql("UPDATE ApplicationPage Set PageName = 'CorporateDirectorCheck' where Id = 17");
        migrationBuilder.Sql("UPDATE ApplicationPage Set PageName = 'CorporateBodies' where Id = 18");
        migrationBuilder.Sql("UPDATE ApplicationPage Set PageName = 'CorporateBodyName' where Id = 19");
        migrationBuilder.Sql("UPDATE ApplicationPage Set PageName = 'CorporateBodyIndividualsOrCompany' where Id = 20");
        migrationBuilder.Sql("UPDATE ApplicationPage Set PageName = 'CorporateDirectors' where Id = 21");
        migrationBuilder.Sql("UPDATE ApplicationPage Set PageName = 'ParentCompaniesCheck' where Id = 22");
        migrationBuilder.Sql("UPDATE ApplicationPage Set PageName = 'ParentCompanies' where Id = 23");
        migrationBuilder.Sql("UPDATE ApplicationPage Set PageName = 'ParentCompanyName' where Id = 24");
        migrationBuilder.Sql("UPDATE ApplicationPage Set PageName = 'ParentCompanyDirectorsOrEquivalents' where Id = 25");
        migrationBuilder.Sql("UPDATE ApplicationPage Set PageName = 'ParentCompanyDirectors' where Id = 26");
        migrationBuilder.Sql("UPDATE ApplicationPage Set PageName = 'DirectorsSatisfyG3FitAndProperRequirements' where Id = 27");
        migrationBuilder.Sql("UPDATE ApplicationPage Set PageName = 'UltimateController' where Id = 28");
        migrationBuilder.Sql("UPDATE ApplicationPage Set PageName = 'UltimateControllers' where Id = 29");
        migrationBuilder.Sql("UPDATE ApplicationPage Set PageName = 'Review' where Id = 30");
        migrationBuilder.Sql("UPDATE ApplicationPage Set PageName = 'Submitted' where Id = 31");
    }
}
