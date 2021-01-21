using Microsoft.EntityFrameworkCore.Migrations;

namespace RhemaCMS.Migrations.ChurchModel
{
    public partial class ef_c_upd19 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NVPTag",
                table: "AppUtilityNVP");

            migrationBuilder.AddColumn<string>(
                name: "NVPCode",
                table: "AppUtilityNVP",
                maxLength: 30,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NVPCode",
                table: "AppUtilityNVP");

            migrationBuilder.AddColumn<string>(
                name: "NVPTag",
                table: "AppUtilityNVP",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);
        }
    }
}
