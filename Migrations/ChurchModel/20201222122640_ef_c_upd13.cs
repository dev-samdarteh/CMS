using Microsoft.EntityFrameworkCore.Migrations;

namespace RhemaCMS.Migrations.ChurchModel
{
    public partial class ef_c_upd13 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OwnedByChurchBodyId",
                table: "AppUtilityNVP");

            migrationBuilder.DropColumn(
                name: "SharingStatus",
                table: "AppUtilityNVP");

            migrationBuilder.CreateIndex(
                name: "IX_ChurchPeriod_OwnedByChurchBodyId",
                table: "ChurchPeriod",
                column: "OwnedByChurchBodyId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChurchPeriod_ChurchBody_OwnedByChurchBodyId",
                table: "ChurchPeriod",
                column: "OwnedByChurchBodyId",
                principalTable: "ChurchBody",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChurchPeriod_ChurchBody_OwnedByChurchBodyId",
                table: "ChurchPeriod");

            migrationBuilder.DropIndex(
                name: "IX_ChurchPeriod_OwnedByChurchBodyId",
                table: "ChurchPeriod");

            migrationBuilder.AddColumn<int>(
                name: "OwnedByChurchBodyId",
                table: "AppUtilityNVP",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SharingStatus",
                table: "AppUtilityNVP",
                type: "nvarchar(1)",
                maxLength: 1,
                nullable: true);
        }
    }
}
