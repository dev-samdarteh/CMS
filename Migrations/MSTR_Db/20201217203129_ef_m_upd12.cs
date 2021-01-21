using Microsoft.EntityFrameworkCore.Migrations;

namespace RhemaCMS.Migrations
{
    public partial class ef_m_upd12 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MSTRContactInfo_MSTRChurchBody_ChurchBodyId",
                table: "MSTRContactInfo");

            migrationBuilder.DropIndex(
                name: "IX_MSTRContactInfo_ChurchBodyId",
                table: "MSTRContactInfo");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_MSTRContactInfo_ChurchBodyId",
                table: "MSTRContactInfo",
                column: "ChurchBodyId");

            migrationBuilder.AddForeignKey(
                name: "FK_MSTRContactInfo_MSTRChurchBody_ChurchBodyId",
                table: "MSTRContactInfo",
                column: "ChurchBodyId",
                principalTable: "MSTRChurchBody",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
