using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RhemaCMS.Migrations
{
    public partial class ef_m_upd6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MSTRAppGlobalOwner_MSTRCountry_CountryId",
                table: "MSTRAppGlobalOwner");

            migrationBuilder.DropForeignKey(
                name: "FK_MSTRChurchBody_MSTRCountry_CountryId",
                table: "MSTRChurchBody");

            migrationBuilder.DropForeignKey(
                name: "FK_MSTRContactInfo_MSTRCountry_CountryId",
                table: "MSTRContactInfo");

            migrationBuilder.DropForeignKey(
                name: "FK_MSTRCountryRegion_MSTRCountry_CountryId",
                table: "MSTRCountryRegion");

            migrationBuilder.DropIndex(
                name: "IX_MSTRCountryRegion_CountryId",
                table: "MSTRCountryRegion");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MSTRCountry",
                table: "MSTRCountry");

            migrationBuilder.DropIndex(
                name: "IX_MSTRContactInfo_CountryId",
                table: "MSTRContactInfo");

            migrationBuilder.DropIndex(
                name: "IX_MSTRChurchBody_CountryId",
                table: "MSTRChurchBody");

            migrationBuilder.DropIndex(
                name: "IX_MSTRAppGlobalOwner_CountryId",
                table: "MSTRAppGlobalOwner");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "MSTRCountryRegion");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "MSTRCountry");

            migrationBuilder.DropColumn(
                name: "Acronym",
                table: "MSTRCountry");

            migrationBuilder.DropColumn(
                name: "Currency",
                table: "MSTRCountry");

            migrationBuilder.DropColumn(
                name: "SharingStatus",
                table: "MSTRCountry");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "MSTRContactInfo");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "MSTRChurchBody");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "MSTRAppGlobalOwner");

            migrationBuilder.AddColumn<string>(
                name: "CtryAlpha3Code",
                table: "MSTRCountryRegion",
                maxLength: 3,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CtryAlpha3Code",
                table: "MSTRCountry",
                maxLength: 3,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CtryAlpha2Code",
                table: "MSTRCountry",
                maxLength: 2,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Curr3LISOSymbol",
                table: "MSTRCountry",
                maxLength: 3,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CurrEngName",
                table: "MSTRCountry",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CurrLocName",
                table: "MSTRCountry",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CurrSymbol",
                table: "MSTRCountry",
                maxLength: 1,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CtryAlpha3Code",
                table: "MSTRContactInfo",
                maxLength: 3,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CtryAlpha3Code",
                table: "MSTRChurchBody",
                maxLength: 3,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CtryAlpha3Code",
                table: "MSTRAppGlobalOwner",
                maxLength: 3,
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_MSTRCountry",
                table: "MSTRCountry",
                column: "CtryAlpha3Code");

            migrationBuilder.CreateTable(
                name: "MSTRCountryCustom",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppGlobalOwnerId = table.Column<int>(nullable: true),
                    ChurchBodyId = table.Column<int>(nullable: true),
                    CtryAlpha3Code = table.Column<string>(maxLength: 3, nullable: true),
                    IsDisplay = table.Column<bool>(nullable: false),
                    IsDefaultCountry = table.Column<bool>(nullable: false),
                    IsChurchCountry = table.Column<bool>(nullable: false),
                    Created = table.Column<DateTime>(nullable: true),
                    LastMod = table.Column<DateTime>(nullable: true),
                    CreatedByUserId = table.Column<int>(nullable: true),
                    LastModByUserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MSTRCountryCustom", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MSTRCountryRegion_CtryAlpha3Code",
                table: "MSTRCountryRegion",
                column: "CtryAlpha3Code");

            migrationBuilder.CreateIndex(
                name: "IX_MSTRContactInfo_CtryAlpha3Code",
                table: "MSTRContactInfo",
                column: "CtryAlpha3Code");

            migrationBuilder.CreateIndex(
                name: "IX_MSTRChurchBody_CtryAlpha3Code",
                table: "MSTRChurchBody",
                column: "CtryAlpha3Code");

            migrationBuilder.CreateIndex(
                name: "IX_MSTRAppGlobalOwner_CtryAlpha3Code",
                table: "MSTRAppGlobalOwner",
                column: "CtryAlpha3Code");

            migrationBuilder.AddForeignKey(
                name: "FK_MSTRAppGlobalOwner_MSTRCountry_CtryAlpha3Code",
                table: "MSTRAppGlobalOwner",
                column: "CtryAlpha3Code",
                principalTable: "MSTRCountry",
                principalColumn: "CtryAlpha3Code",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MSTRChurchBody_MSTRCountry_CtryAlpha3Code",
                table: "MSTRChurchBody",
                column: "CtryAlpha3Code",
                principalTable: "MSTRCountry",
                principalColumn: "CtryAlpha3Code",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MSTRContactInfo_MSTRCountry_CtryAlpha3Code",
                table: "MSTRContactInfo",
                column: "CtryAlpha3Code",
                principalTable: "MSTRCountry",
                principalColumn: "CtryAlpha3Code",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MSTRCountryRegion_MSTRCountry_CtryAlpha3Code",
                table: "MSTRCountryRegion",
                column: "CtryAlpha3Code",
                principalTable: "MSTRCountry",
                principalColumn: "CtryAlpha3Code",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MSTRAppGlobalOwner_MSTRCountry_CtryAlpha3Code",
                table: "MSTRAppGlobalOwner");

            migrationBuilder.DropForeignKey(
                name: "FK_MSTRChurchBody_MSTRCountry_CtryAlpha3Code",
                table: "MSTRChurchBody");

            migrationBuilder.DropForeignKey(
                name: "FK_MSTRContactInfo_MSTRCountry_CtryAlpha3Code",
                table: "MSTRContactInfo");

            migrationBuilder.DropForeignKey(
                name: "FK_MSTRCountryRegion_MSTRCountry_CtryAlpha3Code",
                table: "MSTRCountryRegion");

            migrationBuilder.DropTable(
                name: "MSTRCountryCustom");

            migrationBuilder.DropIndex(
                name: "IX_MSTRCountryRegion_CtryAlpha3Code",
                table: "MSTRCountryRegion");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MSTRCountry",
                table: "MSTRCountry");

            migrationBuilder.DropIndex(
                name: "IX_MSTRContactInfo_CtryAlpha3Code",
                table: "MSTRContactInfo");

            migrationBuilder.DropIndex(
                name: "IX_MSTRChurchBody_CtryAlpha3Code",
                table: "MSTRChurchBody");

            migrationBuilder.DropIndex(
                name: "IX_MSTRAppGlobalOwner_CtryAlpha3Code",
                table: "MSTRAppGlobalOwner");

            migrationBuilder.DropColumn(
                name: "CtryAlpha3Code",
                table: "MSTRCountryRegion");

            migrationBuilder.DropColumn(
                name: "CtryAlpha3Code",
                table: "MSTRCountry");

            migrationBuilder.DropColumn(
                name: "CtryAlpha2Code",
                table: "MSTRCountry");

            migrationBuilder.DropColumn(
                name: "Curr3LISOSymbol",
                table: "MSTRCountry");

            migrationBuilder.DropColumn(
                name: "CurrEngName",
                table: "MSTRCountry");

            migrationBuilder.DropColumn(
                name: "CurrLocName",
                table: "MSTRCountry");

            migrationBuilder.DropColumn(
                name: "CurrSymbol",
                table: "MSTRCountry");

            migrationBuilder.DropColumn(
                name: "CtryAlpha3Code",
                table: "MSTRContactInfo");

            migrationBuilder.DropColumn(
                name: "CtryAlpha3Code",
                table: "MSTRChurchBody");

            migrationBuilder.DropColumn(
                name: "CtryAlpha3Code",
                table: "MSTRAppGlobalOwner");

            migrationBuilder.AddColumn<int>(
                name: "CountryId",
                table: "MSTRCountryRegion",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "MSTRCountry",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<string>(
                name: "Acronym",
                table: "MSTRCountry",
                type: "nvarchar(3)",
                maxLength: 3,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Currency",
                table: "MSTRCountry",
                type: "nvarchar(3)",
                maxLength: 3,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SharingStatus",
                table: "MSTRCountry",
                type: "nvarchar(1)",
                maxLength: 1,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CountryId",
                table: "MSTRContactInfo",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CountryId",
                table: "MSTRChurchBody",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CountryId",
                table: "MSTRAppGlobalOwner",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_MSTRCountry",
                table: "MSTRCountry",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_MSTRCountryRegion_CountryId",
                table: "MSTRCountryRegion",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_MSTRContactInfo_CountryId",
                table: "MSTRContactInfo",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_MSTRChurchBody_CountryId",
                table: "MSTRChurchBody",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_MSTRAppGlobalOwner_CountryId",
                table: "MSTRAppGlobalOwner",
                column: "CountryId");

            migrationBuilder.AddForeignKey(
                name: "FK_MSTRAppGlobalOwner_MSTRCountry_CountryId",
                table: "MSTRAppGlobalOwner",
                column: "CountryId",
                principalTable: "MSTRCountry",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MSTRChurchBody_MSTRCountry_CountryId",
                table: "MSTRChurchBody",
                column: "CountryId",
                principalTable: "MSTRCountry",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MSTRContactInfo_MSTRCountry_CountryId",
                table: "MSTRContactInfo",
                column: "CountryId",
                principalTable: "MSTRCountry",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MSTRCountryRegion_MSTRCountry_CountryId",
                table: "MSTRCountryRegion",
                column: "CountryId",
                principalTable: "MSTRCountry",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
