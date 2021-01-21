using Microsoft.EntityFrameworkCore.Migrations;

namespace RhemaCMS.Migrations.ChurchModel
{
    public partial class ef_c_upd23 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsChurchCountry",
                table: "CurrencyCustom");

            migrationBuilder.DropColumn(
                name: "IsDefaultCountry",
                table: "CurrencyCustom");

            migrationBuilder.AddColumn<bool>(
                name: "IsDefaultCurrency",
                table: "CurrencyCustom",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDefaultCurrency",
                table: "CurrencyCustom");

            migrationBuilder.AddColumn<bool>(
                name: "IsChurchCountry",
                table: "CurrencyCustom",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDefaultCountry",
                table: "CurrencyCustom",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
