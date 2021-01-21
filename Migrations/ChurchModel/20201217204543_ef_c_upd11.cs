using Microsoft.EntityFrameworkCore.Migrations;

namespace RhemaCMS.Migrations.ChurchModel
{
    public partial class ef_c_upd11 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChurchBody_ChurchBody_ParentChurchBodyId",
                table: "ChurchBody");

            migrationBuilder.DropIndex(
                name: "IX_ChurchBody_OwnedByChurchBodyId",
                table: "ChurchBody");

            migrationBuilder.DropIndex(
                name: "IX_ChurchBody_ParentChurchBodyId",
                table: "ChurchBody");

            migrationBuilder.CreateIndex(
                name: "IX_ChurchBody_OwnedByChurchBodyId",
                table: "ChurchBody",
                column: "OwnedByChurchBodyId",
                unique: true,
                filter: "[OwnedByChurchBodyId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ChurchBody_OwnedByChurchBodyId",
                table: "ChurchBody");

            migrationBuilder.CreateIndex(
                name: "IX_ChurchBody_OwnedByChurchBodyId",
                table: "ChurchBody",
                column: "OwnedByChurchBodyId");

            migrationBuilder.CreateIndex(
                name: "IX_ChurchBody_ParentChurchBodyId",
                table: "ChurchBody",
                column: "ParentChurchBodyId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChurchBody_ChurchBody_ParentChurchBodyId",
                table: "ChurchBody",
                column: "ParentChurchBodyId",
                principalTable: "ChurchBody",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
