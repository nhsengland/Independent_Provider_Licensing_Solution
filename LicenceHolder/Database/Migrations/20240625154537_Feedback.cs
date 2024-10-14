using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Database.Migrations;

/// <inheritdoc />
public partial class Feedback : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "FeedbackSatisfaction",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_FeedbackSatisfaction", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "FeedbackType",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Type = table.Column<string>(type: "nvarchar(max)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_FeedbackType", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Feedback",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                DateGenerated = table.Column<DateTime>(type: "datetime2", nullable: false),
                SatisfactionId = table.Column<int>(type: "int", nullable: false),
                HowToImprove = table.Column<string>(type: "nvarchar(1200)", maxLength: 1200, nullable: true),
                TypeId = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Feedback", x => x.Id);
                table.ForeignKey(
                    name: "FK_Feedback_FeedbackSatisfaction_SatisfactionId",
                    column: x => x.SatisfactionId,
                    principalTable: "FeedbackSatisfaction",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_Feedback_FeedbackType_TypeId",
                    column: x => x.TypeId,
                    principalTable: "FeedbackType",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.InsertData(
            table: "FeedbackSatisfaction",
            columns: new[] { "Id", "Name" },
            values: new object[,]
            {
                { 1, "VeryDissatisfied" },
                { 2, "Dissatisfied" },
                { 3, "Neutral" },
                { 4, "Satisfied" },
                { 5, "VerySatisfied" }
            });

        migrationBuilder.InsertData(
            table: "FeedbackType",
            columns: new[] { "Id", "Type" },
            values: new object[,]
            {
                { 1, "Other" },
                { 2, "Licence" },
                { 3, "Messages" },
                { 4, "Team" },
                { 5, "YourProfile" },
                { 6, "Tasks" }
            });

        migrationBuilder.CreateIndex(
            name: "IX_Feedback_SatisfactionId",
            table: "Feedback",
            column: "SatisfactionId");

        migrationBuilder.CreateIndex(
            name: "IX_Feedback_TypeId",
            table: "Feedback",
            column: "TypeId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Feedback");

        migrationBuilder.DropTable(
            name: "FeedbackSatisfaction");

        migrationBuilder.DropTable(
            name: "FeedbackType");
    }
}
