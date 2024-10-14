using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations
{
    /// <inheritdoc />
    public partial class Tasks_Users_And_ChangeRequests : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Company_Organisation_OrganisationId",
                table: "Company");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateCreated",
                table: "Organisation",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "Postcode",
                table: "Company",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(15)",
                oldMaxLength: 15);

            migrationBuilder.AlterColumn<DateOnly>(
                name: "FinancialYearEnd",
                table: "Company",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateCreated",
                table: "Company",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.CreateTable(
                name: "ChangeRequestStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChangeRequestStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ChangeRequestType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChangeRequestType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Licence",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    LicenceNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    DateIssued = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateCancelled = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Licence", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Licence_Company",
                        column: x => x.CompanyId,
                        principalTable: "Company",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TaskStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserRole",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRole", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ChangeRequest",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    TypeId = table.Column<int>(type: "int", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    DateLastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FinancialYearEnd = table.Column<DateOnly>(type: "date", nullable: true),
                    AddressLine1 = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    AddressLine2 = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    City = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    County = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Postcode = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChangeRequest", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChangeRequest_ChangeRequestStatus_StatusId",
                        column: x => x.StatusId,
                        principalTable: "ChangeRequestStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChangeRequest_ChangeRequestType_TypeId",
                        column: x => x.TypeId,
                        principalTable: "ChangeRequestType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChangeRequest_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TaskForAnnualCertificate",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    TaskStatusId = table.Column<int>(type: "int", nullable: false),
                    DateDue = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateCompleted = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    DateLastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SharePointLocation = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskForAnnualCertificate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskForAnnualCertificate_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Company",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TaskForAnnualCertificate_TaskStatus_TaskStatusId",
                        column: x => x.TaskStatusId,
                        principalTable: "TaskStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TaskForFinancialMonitoring",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrganisationId = table.Column<int>(type: "int", nullable: false),
                    TaskStatusId = table.Column<int>(type: "int", nullable: false),
                    DateDue = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateCompleted = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    DateLastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SharePointLocation = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskForFinancialMonitoring", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskForFinancialMonitoring_Organisation_OrganisationId",
                        column: x => x.OrganisationId,
                        principalTable: "Organisation",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TaskForFinancialMonitoring_TaskStatus_TaskStatusId",
                        column: x => x.TaskStatusId,
                        principalTable: "TaskStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrganisationId = table.Column<int>(type: "int", nullable: false),
                    OktaId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UserRoleId = table.Column<int>(type: "int", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateLastModified = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                    table.ForeignKey(
                        name: "FK_User_Organisation",
                        column: x => x.OrganisationId,
                        principalTable: "Organisation",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_User_UserRole",
                        column: x => x.UserRoleId,
                        principalTable: "UserRole",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChangeRequest_CompanyId",
                table: "ChangeRequest",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_ChangeRequest_StatusId",
                table: "ChangeRequest",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_ChangeRequest_TypeId",
                table: "ChangeRequest",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Licence_CompanyId",
                table: "Licence",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskForAnnualCertificate_CompanyId",
                table: "TaskForAnnualCertificate",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskForAnnualCertificate_TaskStatusId",
                table: "TaskForAnnualCertificate",
                column: "TaskStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskForFinancialMonitoring_OrganisationId",
                table: "TaskForFinancialMonitoring",
                column: "OrganisationId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskForFinancialMonitoring_TaskStatusId",
                table: "TaskForFinancialMonitoring",
                column: "TaskStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_User_OrganisationId",
                table: "User",
                column: "OrganisationId");

            migrationBuilder.CreateIndex(
                name: "IX_User_UserRoleId",
                table: "User",
                column: "UserRoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Company_Organisation_OrganisationId",
                table: "Company",
                column: "OrganisationId",
                principalTable: "Organisation",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Company_Organisation_OrganisationId",
                table: "Company");

            migrationBuilder.DropTable(
                name: "ChangeRequest");

            migrationBuilder.DropTable(
                name: "Licence");

            migrationBuilder.DropTable(
                name: "TaskForAnnualCertificate");

            migrationBuilder.DropTable(
                name: "TaskForFinancialMonitoring");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "ChangeRequestStatus");

            migrationBuilder.DropTable(
                name: "ChangeRequestType");

            migrationBuilder.DropTable(
                name: "TaskStatus");

            migrationBuilder.DropTable(
                name: "UserRole");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateCreated",
                table: "Organisation",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.AlterColumn<string>(
                name: "Postcode",
                table: "Company",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250);

            migrationBuilder.AlterColumn<DateTime>(
                name: "FinancialYearEnd",
                table: "Company",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateCreated",
                table: "Company",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.AddForeignKey(
                name: "FK_Company_Organisation_OrganisationId",
                table: "Company",
                column: "OrganisationId",
                principalTable: "Organisation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
