using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RhemaCMS.Migrations.ChurchModel
{
    public partial class ef_c_upd20 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CurrencyCustom",
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
                    IsBaseCurrency = table.Column<bool>(nullable: false),
                    BaseRate = table.Column<decimal>(type: "decimal(18, 4)", nullable: false),
                    Created = table.Column<DateTime>(nullable: true),
                    LastMod = table.Column<DateTime>(nullable: true),
                    CreatedByUserId = table.Column<int>(nullable: true),
                    LastModByUserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrencyCustom", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CurrencyCustom_AppGlobalOwner_AppGlobalOwnerId",
                        column: x => x.AppGlobalOwnerId,
                        principalTable: "AppGlobalOwner",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CurrencyCustom_ChurchBody_ChurchBodyId",
                        column: x => x.ChurchBodyId,
                        principalTable: "ChurchBody",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CurrencyCustom_Country_CtryAlpha3Code",
                        column: x => x.CtryAlpha3Code,
                        principalTable: "Country",
                        principalColumn: "CtryAlpha3Code",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CurrencyCustom_AppGlobalOwnerId",
                table: "CurrencyCustom",
                column: "AppGlobalOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_CurrencyCustom_ChurchBodyId",
                table: "CurrencyCustom",
                column: "ChurchBodyId");

            migrationBuilder.CreateIndex(
                name: "IX_CurrencyCustom_CtryAlpha3Code",
                table: "CurrencyCustom",
                column: "CtryAlpha3Code");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CurrencyCustom");
        }
    }
}
