using Microsoft.EntityFrameworkCore.Migrations;

namespace RhemaCMS.Migrations.ChurchModel
{
    public partial class ef_c_upd18 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NVPCode",
                table: "AppUtilityNVP");

            migrationBuilder.AddColumn<string>(
                name: "NVPSubCode",
                table: "AppUtilityNVP",
                maxLength: 15,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NVPSubCode",
                table: "AppUtilityNVP");

            migrationBuilder.AddColumn<string>(
                name: "NVPCode",
                table: "AppUtilityNVP",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: true);
        }
    }
}
