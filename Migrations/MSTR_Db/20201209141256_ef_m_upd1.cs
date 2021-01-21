using Microsoft.EntityFrameworkCore.Migrations;

namespace RhemaCMS.Migrations
{
    public partial class ef_m_upd1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "UserProfile");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "UserProfile");

            migrationBuilder.RenameColumn(
                name: "STRT",
                table: "UserGroupPermission",
                newName: "Strt");

            migrationBuilder.RenameColumn(
                name: "EXPR",
                table: "UserGroupPermission",
                newName: "Expr");

            migrationBuilder.AddColumn<bool>(
                name: "IsChurchMember",
                table: "UserProfile",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "OwnerUserId",
                table: "UserProfile",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsChurchMember",
                table: "UserProfile");

            migrationBuilder.DropColumn(
                name: "OwnerUserId",
                table: "UserProfile");

            migrationBuilder.RenameColumn(
                name: "Strt",
                table: "UserGroupPermission",
                newName: "STRT");

            migrationBuilder.RenameColumn(
                name: "Expr",
                table: "UserGroupPermission",
                newName: "EXPR");

            migrationBuilder.AddColumn<int>(
                name: "OwnerId",
                table: "UserProfile",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "UserProfile",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
