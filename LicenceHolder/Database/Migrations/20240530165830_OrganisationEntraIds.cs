using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations;

/// <inheritdoc />
public partial class OrganisationEntraIds : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<Guid>(
            name: "NHSEUserEntraId1",
            table: "Organisation",
            type: "uniqueidentifier",
            nullable: true);

        migrationBuilder.AddColumn<Guid>(
            name: "NHSEUserEntraId2",
            table: "Organisation",
            type: "uniqueidentifier",
            nullable: true);

        migrationBuilder.AddColumn<Guid>(
            name: "NHSEUserEntraId3",
            table: "Organisation",
            type: "uniqueidentifier",
            nullable: true);

        migrationBuilder.AddColumn<Guid>(
            name: "NHSEUserEntraId4",
            table: "Organisation",
            type: "uniqueidentifier",
            nullable: true);

        migrationBuilder.AddColumn<Guid>(
            name: "NHSEUserEntraId5",
            table: "Organisation",
            type: "uniqueidentifier",
            nullable: true);

        migrationBuilder.AddColumn<Guid>(
            name: "NHSEUserEntraId6",
            table: "Organisation",
            type: "uniqueidentifier",
            nullable: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "NHSEUserEntraId1",
            table: "Organisation");

        migrationBuilder.DropColumn(
            name: "NHSEUserEntraId2",
            table: "Organisation");

        migrationBuilder.DropColumn(
            name: "NHSEUserEntraId3",
            table: "Organisation");

        migrationBuilder.DropColumn(
            name: "NHSEUserEntraId4",
            table: "Organisation");

        migrationBuilder.DropColumn(
            name: "NHSEUserEntraId5",
            table: "Organisation");

        migrationBuilder.DropColumn(
            name: "NHSEUserEntraId6",
            table: "Organisation");
    }
}
