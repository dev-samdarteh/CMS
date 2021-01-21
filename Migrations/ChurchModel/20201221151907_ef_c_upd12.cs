using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RhemaCMS.Migrations.ChurchModel
{
    public partial class ef_c_upd12 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LanguageSpoken_ChurchBody_ChurchBodyId",
                table: "LanguageSpoken");

            migrationBuilder.DropIndex(
                name: "IX_LanguageSpoken_ChurchBodyId",
                table: "LanguageSpoken");

            migrationBuilder.DropColumn(
                name: "ChurchBodyId",
                table: "LanguageSpoken");

            migrationBuilder.DropColumn(
                name: "OwnedByChurchBodyId",
                table: "CountryCustom");

            migrationBuilder.DropColumn(
                name: "SharingStatus",
                table: "CountryCustom");

            migrationBuilder.AlterColumn<string>(
                name: "RegCode",
                table: "CountryRegion",
                maxLength: 5,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(3)",
                oldMaxLength: 3,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SharingStatus",
                table: "AppUtilityNVP",
                maxLength: 1,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CountryRegionCustom",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppGlobalOwnerId = table.Column<int>(nullable: true),
                    ChurchBodyId = table.Column<int>(nullable: true),
                    CountryRegionId = table.Column<int>(nullable: false),
                    IsDisplay = table.Column<bool>(nullable: false),
                    IsDefaultRegion = table.Column<bool>(nullable: false),
                    IsChurchRegion = table.Column<bool>(nullable: false),
                    Created = table.Column<DateTime>(nullable: true),
                    LastMod = table.Column<DateTime>(nullable: true),
                    CreatedByUserId = table.Column<int>(nullable: true),
                    LastModByUserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CountryRegionCustom", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CountryRegionCustom_AppGlobalOwner_AppGlobalOwnerId",
                        column: x => x.AppGlobalOwnerId,
                        principalTable: "AppGlobalOwner",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CountryRegionCustom_ChurchBody_ChurchBodyId",
                        column: x => x.ChurchBodyId,
                        principalTable: "ChurchBody",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CountryRegionCustom_CountryRegion_CountryRegionId",
                        column: x => x.CountryRegionId,
                        principalTable: "CountryRegion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LanguageSpokenCustom",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppGlobalOwnerId = table.Column<int>(nullable: true),
                    ChurchBodyId = table.Column<int>(nullable: true),
                    LanguageSpokenId = table.Column<int>(nullable: false),
                    IsDisplay = table.Column<bool>(nullable: false),
                    IsDefaultLanguage = table.Column<bool>(nullable: false),
                    IsChurchLanguage = table.Column<bool>(nullable: false),
                    Created = table.Column<DateTime>(nullable: true),
                    LastMod = table.Column<DateTime>(nullable: true),
                    CreatedByUserId = table.Column<int>(nullable: true),
                    LastModByUserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LanguageSpokenCustom", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LanguageSpokenCustom_AppGlobalOwner_AppGlobalOwnerId",
                        column: x => x.AppGlobalOwnerId,
                        principalTable: "AppGlobalOwner",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LanguageSpokenCustom_ChurchBody_ChurchBodyId",
                        column: x => x.ChurchBodyId,
                        principalTable: "ChurchBody",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LanguageSpokenCustom_LanguageSpoken_LanguageSpokenId",
                        column: x => x.LanguageSpokenId,
                        principalTable: "LanguageSpoken",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CountryCustom_CtryAlpha3Code",
                table: "CountryCustom",
                column: "CtryAlpha3Code");

            migrationBuilder.CreateIndex(
                name: "IX_CountryRegionCustom_AppGlobalOwnerId",
                table: "CountryRegionCustom",
                column: "AppGlobalOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_CountryRegionCustom_ChurchBodyId",
                table: "CountryRegionCustom",
                column: "ChurchBodyId");

            migrationBuilder.CreateIndex(
                name: "IX_CountryRegionCustom_CountryRegionId",
                table: "CountryRegionCustom",
                column: "CountryRegionId");

            migrationBuilder.CreateIndex(
                name: "IX_LanguageSpokenCustom_AppGlobalOwnerId",
                table: "LanguageSpokenCustom",
                column: "AppGlobalOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_LanguageSpokenCustom_ChurchBodyId",
                table: "LanguageSpokenCustom",
                column: "ChurchBodyId");

            migrationBuilder.CreateIndex(
                name: "IX_LanguageSpokenCustom_LanguageSpokenId",
                table: "LanguageSpokenCustom",
                column: "LanguageSpokenId");

            migrationBuilder.AddForeignKey(
                name: "FK_CountryCustom_Country_CtryAlpha3Code",
                table: "CountryCustom",
                column: "CtryAlpha3Code",
                principalTable: "Country",
                principalColumn: "CtryAlpha3Code",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CountryCustom_Country_CtryAlpha3Code",
                table: "CountryCustom");

            migrationBuilder.DropTable(
                name: "CountryRegionCustom");

            migrationBuilder.DropTable(
                name: "LanguageSpokenCustom");

            migrationBuilder.DropIndex(
                name: "IX_CountryCustom_CtryAlpha3Code",
                table: "CountryCustom");

            migrationBuilder.DropColumn(
                name: "SharingStatus",
                table: "AppUtilityNVP");

            migrationBuilder.AddColumn<int>(
                name: "ChurchBodyId",
                table: "LanguageSpoken",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RegCode",
                table: "CountryRegion",
                type: "nvarchar(3)",
                maxLength: 3,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 5,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OwnedByChurchBodyId",
                table: "CountryCustom",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SharingStatus",
                table: "CountryCustom",
                type: "nvarchar(1)",
                maxLength: 1,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LanguageSpoken_ChurchBodyId",
                table: "LanguageSpoken",
                column: "ChurchBodyId");

            migrationBuilder.AddForeignKey(
                name: "FK_LanguageSpoken_ChurchBody_ChurchBodyId",
                table: "LanguageSpoken",
                column: "ChurchBodyId",
                principalTable: "ChurchBody",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
