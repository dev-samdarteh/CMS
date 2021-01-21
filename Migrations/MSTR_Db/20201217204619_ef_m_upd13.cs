using Microsoft.EntityFrameworkCore.Migrations;

namespace RhemaCMS.Migrations
{
    public partial class ef_m_upd13 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MSTRChurchBody_MSTRChurchBody_ParentChurchBodyId",
                table: "MSTRChurchBody");

            migrationBuilder.DropIndex(
                name: "IX_MSTRChurchBody_ParentChurchBodyId",
                table: "MSTRChurchBody");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_MSTRChurchBody_ParentChurchBodyId",
                table: "MSTRChurchBody",
                column: "ParentChurchBodyId");

            migrationBuilder.AddForeignKey(
                name: "FK_MSTRChurchBody_MSTRChurchBody_ParentChurchBodyId",
                table: "MSTRChurchBody",
                column: "ParentChurchBodyId",
                principalTable: "MSTRChurchBody",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
