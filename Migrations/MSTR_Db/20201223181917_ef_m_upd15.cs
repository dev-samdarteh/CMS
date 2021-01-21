using Microsoft.EntityFrameworkCore.Migrations;

namespace RhemaCMS.Migrations
{
    public partial class ef_m_upd15 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "MSTRCountry");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "MSTRCountry",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }
    }
}
