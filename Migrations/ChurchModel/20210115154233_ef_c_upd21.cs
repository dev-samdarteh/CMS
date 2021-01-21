using Microsoft.EntityFrameworkCore.Migrations;

namespace RhemaCMS.Migrations.ChurchModel
{
    public partial class ef_c_upd21 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ChurchBodyId",
                table: "CountryRegion",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CountryRegion_ChurchBodyId",
                table: "CountryRegion",
                column: "ChurchBodyId");

            migrationBuilder.AddForeignKey(
                name: "FK_CountryRegion_ChurchBody_ChurchBodyId",
                table: "CountryRegion",
                column: "ChurchBodyId",
                principalTable: "ChurchBody",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CountryRegion_ChurchBody_ChurchBodyId",
                table: "CountryRegion");

            migrationBuilder.DropIndex(
                name: "IX_CountryRegion_ChurchBodyId",
                table: "CountryRegion");

            migrationBuilder.DropColumn(
                name: "ChurchBodyId",
                table: "CountryRegion");
        }
    }
}
