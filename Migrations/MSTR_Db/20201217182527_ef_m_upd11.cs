using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RhemaCMS.Migrations
{
    public partial class ef_m_upd11 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "MSTRCountry",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedByUserId",
                table: "MSTRCountry",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastMod",
                table: "MSTRCountry",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LastModByUserId",
                table: "MSTRCountry",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "MSTRCountry",
                maxLength: 100,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Created",
                table: "MSTRCountry");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "MSTRCountry");

            migrationBuilder.DropColumn(
                name: "LastMod",
                table: "MSTRCountry");

            migrationBuilder.DropColumn(
                name: "LastModByUserId",
                table: "MSTRCountry");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "MSTRCountry");
        }
    }
}
