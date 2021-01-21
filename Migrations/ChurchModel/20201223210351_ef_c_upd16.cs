using Microsoft.EntityFrameworkCore.Migrations;

namespace RhemaCMS.Migrations.ChurchModel
{
    public partial class ef_c_upd16 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "strFaithCategory",
                table: "AppGlobalOwner",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "strFaithStream",
                table: "AppGlobalOwner",
                maxLength: 50,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "strFaithCategory",
                table: "AppGlobalOwner");

            migrationBuilder.DropColumn(
                name: "strFaithStream",
                table: "AppGlobalOwner");
        }
    }
}
