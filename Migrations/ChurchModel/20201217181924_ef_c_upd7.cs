using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RhemaCMS.Migrations.ChurchModel
{
    public partial class ef_c_upd7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Created",
                table: "Country");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "Country");

            migrationBuilder.DropColumn(
                name: "LastMod",
                table: "Country");

            migrationBuilder.DropColumn(
                name: "LastModByUserId",
                table: "Country");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Country");

            migrationBuilder.AlterColumn<string>(
                name: "CtryAlpha2Code",
                table: "Country",
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
                table: "Country",
                type: "nvarchar(2)",
                maxLength: 2,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 3,
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "Country",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedByUserId",
                table: "Country",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastMod",
                table: "Country",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LastModByUserId",
                table: "Country",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Country",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }
    }
}
