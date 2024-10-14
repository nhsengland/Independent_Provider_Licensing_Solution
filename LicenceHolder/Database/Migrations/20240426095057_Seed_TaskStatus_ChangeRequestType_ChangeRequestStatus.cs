using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Database.Migrations
{
    /// <inheritdoc />
    public partial class Seed_TaskStatus_ChangeRequestType_ChangeRequestStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ChangeRequestStatus",
                columns: new[] { "Id", "Status" },
                values: new object[,]
                {
                    { 100, "Pending" },
                    { 200, "Approved" },
                    { 300, "Rejected" }
                });

            migrationBuilder.InsertData(
                table: "ChangeRequestType",
                columns: new[] { "Id", "Type" },
                values: new object[,]
                {
                    { 100, "Address" },
                    { 200, "FinancialYearEnd" }
                });

            migrationBuilder.InsertData(
                table: "TaskStatus",
                columns: new[] { "Id", "Status" },
                values: new object[,]
                {
                    { 100, "InComplete" },
                    { 200, "Complete" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ChangeRequestStatus",
                keyColumn: "Id",
                keyValue: 100);

            migrationBuilder.DeleteData(
                table: "ChangeRequestStatus",
                keyColumn: "Id",
                keyValue: 200);

            migrationBuilder.DeleteData(
                table: "ChangeRequestStatus",
                keyColumn: "Id",
                keyValue: 300);

            migrationBuilder.DeleteData(
                table: "ChangeRequestType",
                keyColumn: "Id",
                keyValue: 100);

            migrationBuilder.DeleteData(
                table: "ChangeRequestType",
                keyColumn: "Id",
                keyValue: 200);

            migrationBuilder.DeleteData(
                table: "TaskStatus",
                keyColumn: "Id",
                keyValue: 100);

            migrationBuilder.DeleteData(
                table: "TaskStatus",
                keyColumn: "Id",
                keyValue: 200);
        }
    }
}
