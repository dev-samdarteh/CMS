using Microsoft.EntityFrameworkCore.Migrations;

namespace RhemaCMS.Migrations.ChurchModel
{
    public partial class ef_c_upd8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Country_AppGlobalOwner_AppGlobalOwnerId",
                table: "Country");

            migrationBuilder.DropIndex(
                name: "IX_Country_AppGlobalOwnerId",
                table: "Country");

            migrationBuilder.DropIndex(
                name: "IX_AppGlobalOwner_CtryAlpha3Code",
                table: "AppGlobalOwner");

            migrationBuilder.DropColumn(
                name: "AppGlobalOwnerId",
                table: "Country");

            migrationBuilder.CreateIndex(
                name: "IX_AppGlobalOwner_CtryAlpha3Code",
                table: "AppGlobalOwner",
                column: "CtryAlpha3Code");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AppGlobalOwner_CtryAlpha3Code",
                table: "AppGlobalOwner");

            migrationBuilder.AddColumn<int>(
                name: "AppGlobalOwnerId",
                table: "Country",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Country_AppGlobalOwnerId",
                table: "Country",
                column: "AppGlobalOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_AppGlobalOwner_CtryAlpha3Code",
                table: "AppGlobalOwner",
                column: "CtryAlpha3Code",
                unique: true,
                filter: "[CtryAlpha3Code] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Country_AppGlobalOwner_AppGlobalOwnerId",
                table: "Country",
                column: "AppGlobalOwnerId",
                principalTable: "AppGlobalOwner",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
