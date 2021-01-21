using Microsoft.EntityFrameworkCore.Migrations;

namespace RhemaCMS.Migrations.ChurchModel
{
    public partial class ef_c_upd17 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FaithTypeCategoryId",
                table: "AppGlobalOwner");

            migrationBuilder.DropColumn(
                name: "strFaithCategory",
                table: "AppGlobalOwner");

            migrationBuilder.AddColumn<string>(
                name: "strFaithTypeCategory",
                table: "AppGlobalOwner",
                maxLength: 50,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "strFaithTypeCategory",
                table: "AppGlobalOwner");

            migrationBuilder.AddColumn<int>(
                name: "FaithTypeCategoryId",
                table: "AppGlobalOwner",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "strFaithCategory",
                table: "AppGlobalOwner",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }
    }
}
