using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Database.Migrations;

/// <inheritdoc />
public partial class Messages_And_SmallChange : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Notification");

        migrationBuilder.AddColumn<DateTime>(
            name: "PublishedLicenceDate",
            table: "Licence",
            type: "datetime2",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "PublishedLicenceUrl",
            table: "Licence",
            type: "nvarchar(max)",
            nullable: true);

        migrationBuilder.CreateTable(
            name: "MessageType",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Type = table.Column<string>(type: "nvarchar(max)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_MessageType", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Message",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                OrganisationId = table.Column<int>(type: "int", nullable: false),
                Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Body = table.Column<string>(type: "nvarchar(max)", nullable: false),
                From = table.Column<string>(type: "nvarchar(max)", nullable: false),
                SendDateTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                IsRead = table.Column<bool>(type: "bit", nullable: false),
                MessageTypeId = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Message", x => x.Id);
                table.ForeignKey(
                    name: "FK_Message_MessageType_MessageTypeId",
                    column: x => x.MessageTypeId,
                    principalTable: "MessageType",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_Message_Organisation_OrganisationId",
                    column: x => x.OrganisationId,
                    principalTable: "Organisation",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.InsertData(
            table: "MessageType",
            columns: new[] { "Id", "Type" },
            values: new object[,]
            {
                { 1, "Inbound" },
                { 2, "Outbound" }
            });

        migrationBuilder.CreateIndex(
            name: "IX_Message_MessageTypeId",
            table: "Message",
            column: "MessageTypeId");

        migrationBuilder.CreateIndex(
            name: "IX_Message_OrganisationId",
            table: "Message",
            column: "OrganisationId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Message");

        migrationBuilder.DropTable(
            name: "MessageType");

        migrationBuilder.DropColumn(
            name: "PublishedLicenceDate",
            table: "Licence");

        migrationBuilder.DropColumn(
            name: "PublishedLicenceUrl",
            table: "Licence");

        migrationBuilder.CreateTable(
            name: "Notification",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                OrganisationId = table.Column<int>(type: "int", nullable: false),
                Body = table.Column<string>(type: "nvarchar(max)", nullable: false),
                From = table.Column<string>(type: "nvarchar(max)", nullable: false),
                IsRead = table.Column<bool>(type: "bit", nullable: false),
                SendDateTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                Title = table.Column<string>(type: "nvarchar(max)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Notification", x => x.Id);
                table.ForeignKey(
                    name: "FK_Notification_Organisation_OrganisationId",
                    column: x => x.OrganisationId,
                    principalTable: "Organisation",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_Notification_OrganisationId",
            table: "Notification",
            column: "OrganisationId");
    }
}
