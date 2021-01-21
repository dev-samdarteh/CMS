using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RhemaCMS.Migrations
{
    public partial class ef_m_upd10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<string>(
                name: "CtryAlpha2Code",
                table: "MSTRCountry",
                maxLength: 3,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(2)",
                oldMaxLength: 2,
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CtryAlpha2Code",
                table: "MSTRCountry",
                type: "nvarchar(2)",
                maxLength: 2,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 3,
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "MSTRCountry",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedByUserId",
                table: "MSTRCountry",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastMod",
                table: "MSTRCountry",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LastModByUserId",
                table: "MSTRCountry",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "MSTRCountry",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }
    }
}
