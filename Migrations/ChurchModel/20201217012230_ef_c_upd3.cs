using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RhemaCMS.Migrations.ChurchModel
{
    public partial class ef_c_upd3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppGlobalOwner_Country_CountryId",
                table: "AppGlobalOwner");

            migrationBuilder.DropForeignKey(
                name: "FK_ChurchBody_Country_CountryId",
                table: "ChurchBody");

            migrationBuilder.DropForeignKey(
                name: "FK_ChurchMember_Country_NationalityId",
                table: "ChurchMember");

            migrationBuilder.DropForeignKey(
                name: "FK_ChurchVisitor_Country_NationalityId",
                table: "ChurchVisitor");

            migrationBuilder.DropForeignKey(
                name: "FK_ContactInfo_Country_CountryId",
                table: "ContactInfo");

            migrationBuilder.DropForeignKey(
                name: "FK_Country_ChurchBody_ChurchBodyId",
                table: "Country");

            migrationBuilder.DropForeignKey(
                name: "FK_CountryRegion_Country_CountryId",
                table: "CountryRegion");

            migrationBuilder.DropForeignKey(
                name: "FK_Currency_Country_CountryId",
                table: "Currency");

            migrationBuilder.DropForeignKey(
                name: "FK_LanguageSpoken_Country_CountryId",
                table: "LanguageSpoken");

            migrationBuilder.DropForeignKey(
                name: "FK_MemberContact_Country_ExtConCountryId",
                table: "MemberContact");

            migrationBuilder.DropForeignKey(
                name: "FK_MemberEducHistory_Country_CountryId",
                table: "MemberEducHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_MemberRelation_Country_CountryIdExtCon",
                table: "MemberRelation");

            migrationBuilder.DropForeignKey(
                name: "FK_National_IdType_Country_CountryId",
                table: "National_IdType");

            migrationBuilder.DropIndex(
                name: "IX_National_IdType_CountryId",
                table: "National_IdType");

            migrationBuilder.DropIndex(
                name: "IX_MemberRelation_CountryIdExtCon",
                table: "MemberRelation");

            migrationBuilder.DropIndex(
                name: "IX_MemberEducHistory_CountryId",
                table: "MemberEducHistory");

            migrationBuilder.DropIndex(
                name: "IX_MemberContact_ExtConCountryId",
                table: "MemberContact");

            migrationBuilder.DropIndex(
                name: "IX_LanguageSpoken_CountryId",
                table: "LanguageSpoken");

            migrationBuilder.DropIndex(
                name: "IX_Currency_CountryId",
                table: "Currency");

            migrationBuilder.DropIndex(
                name: "IX_CountryRegion_CountryId",
                table: "CountryRegion");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Country",
                table: "Country");

            migrationBuilder.DropIndex(
                name: "IX_Country_ChurchBodyId",
                table: "Country");

            migrationBuilder.DropIndex(
                name: "IX_ContactInfo_CountryId",
                table: "ContactInfo");

            migrationBuilder.DropIndex(
                name: "IX_ChurchBody_CountryId",
                table: "ChurchBody");

            migrationBuilder.DropIndex(
                name: "IX_AppGlobalOwner_CountryId",
                table: "AppGlobalOwner");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "National_IdType");

            migrationBuilder.DropColumn(
                name: "CountryIdExtCon",
                table: "MemberRelation");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "MemberEducHistory");

            migrationBuilder.DropColumn(
                name: "ExtConCountryId",
                table: "MemberContact");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "LanguageSpoken");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "Currency");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "CountryRegion");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Country");

            migrationBuilder.DropColumn(
                name: "Acronym",
                table: "Country");

            migrationBuilder.DropColumn(
                name: "ChurchBodyId",
                table: "Country");

            migrationBuilder.DropColumn(
                name: "Currency",
                table: "Country");

            migrationBuilder.DropColumn(
                name: "IsChurchCountry",
                table: "Country");

            migrationBuilder.DropColumn(
                name: "IsDefaultCountry",
                table: "Country");

            migrationBuilder.DropColumn(
                name: "IsDisplay",
                table: "Country");

            migrationBuilder.DropColumn(
                name: "OwnedByChurchBodyId",
                table: "Country");

            migrationBuilder.DropColumn(
                name: "SharingStatus",
                table: "Country");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "ContactInfo");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "ChurchBody");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "AppGlobalOwner");

            migrationBuilder.AddColumn<string>(
                name: "CtryAlpha3Code",
                table: "National_IdType",
                maxLength: 3,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CtryAlpha3CodeExtCon",
                table: "MemberRelation",
                maxLength: 3,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CtryAlpha3Code",
                table: "MemberEducHistory",
                maxLength: 3,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CtryAlpha3Code",
                table: "MemberContact",
                maxLength: 3,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CtryAlpha3Code",
                table: "LanguageSpoken",
                maxLength: 3,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CtryAlpha3Code",
                table: "Currency",
                maxLength: 3,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CtryAlpha3Code",
                table: "CountryRegion",
                maxLength: 3,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Country",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(60)",
                oldMaxLength: 60);

            migrationBuilder.AddColumn<string>(
                name: "CtryAlpha3Code",
                table: "Country",
                maxLength: 3,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CtryAlpha2Code",
                table: "Country",
                maxLength: 2,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Curr3LISOSymbol",
                table: "Country",
                maxLength: 3,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CurrEngName",
                table: "Country",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CurrLocName",
                table: "Country",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CurrSymbol",
                table: "Country",
                maxLength: 1,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CtryAlpha3Code",
                table: "ContactInfo",
                maxLength: 3,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NationalityId",
                table: "ChurchVisitor",
                maxLength: 3,
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "NationalityId",
                table: "ChurchMember",
                maxLength: 3,
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CtryAlpha3Code",
                table: "ChurchBody",
                maxLength: 3,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CtryAlpha3Code",
                table: "AppGlobalOwner",
                maxLength: 3,
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Country",
                table: "Country",
                column: "CtryAlpha3Code");

            migrationBuilder.CreateTable(
                name: "CountryCustom",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppGlobalOwnerId = table.Column<int>(nullable: true),
                    ChurchBodyId = table.Column<int>(nullable: true),
                    CtryAlpha3Code = table.Column<string>(maxLength: 3, nullable: true),
                    IsDisplay = table.Column<bool>(nullable: false),
                    IsDefaultCountry = table.Column<bool>(nullable: false),
                    IsChurchCountry = table.Column<bool>(nullable: false),
                    OwnedByChurchBodyId = table.Column<int>(nullable: true),
                    SharingStatus = table.Column<string>(maxLength: 1, nullable: true),
                    Created = table.Column<DateTime>(nullable: true),
                    LastMod = table.Column<DateTime>(nullable: true),
                    CreatedByUserId = table.Column<int>(nullable: true),
                    LastModByUserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CountryCustom", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CountryCustom_AppGlobalOwner_AppGlobalOwnerId",
                        column: x => x.AppGlobalOwnerId,
                        principalTable: "AppGlobalOwner",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CountryCustom_ChurchBody_ChurchBodyId",
                        column: x => x.ChurchBodyId,
                        principalTable: "ChurchBody",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserAuditTrail_CL",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppGlobalOwnerId = table.Column<int>(nullable: true),
                    ChurchBodyId = table.Column<int>(nullable: true),
                    AuditType = table.Column<string>(maxLength: 1, nullable: true),
                    UI_Desc = table.Column<string>(maxLength: 50, nullable: true),
                    Url = table.Column<string>(nullable: true),
                    EventDate = table.Column<DateTime>(nullable: false),
                    EventDetail = table.Column<string>(nullable: true),
                    UserProfileId = table.Column<int>(nullable: true),
                    Created = table.Column<DateTime>(nullable: true),
                    LastMod = table.Column<DateTime>(nullable: true),
                    CreatedByUserId = table.Column<int>(nullable: true),
                    LastModByUserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAuditTrail_CL", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_National_IdType_CtryAlpha3Code",
                table: "National_IdType",
                column: "CtryAlpha3Code");

            migrationBuilder.CreateIndex(
                name: "IX_MemberRelation_CtryAlpha3CodeExtCon",
                table: "MemberRelation",
                column: "CtryAlpha3CodeExtCon");

            migrationBuilder.CreateIndex(
                name: "IX_MemberEducHistory_CtryAlpha3Code",
                table: "MemberEducHistory",
                column: "CtryAlpha3Code");

            migrationBuilder.CreateIndex(
                name: "IX_MemberContact_CtryAlpha3Code",
                table: "MemberContact",
                column: "CtryAlpha3Code");

            migrationBuilder.CreateIndex(
                name: "IX_LanguageSpoken_CtryAlpha3Code",
                table: "LanguageSpoken",
                column: "CtryAlpha3Code");

            migrationBuilder.CreateIndex(
                name: "IX_Currency_CtryAlpha3Code",
                table: "Currency",
                column: "CtryAlpha3Code");

            migrationBuilder.CreateIndex(
                name: "IX_CountryRegion_CtryAlpha3Code",
                table: "CountryRegion",
                column: "CtryAlpha3Code");

            migrationBuilder.CreateIndex(
                name: "IX_ContactInfo_CtryAlpha3Code",
                table: "ContactInfo",
                column: "CtryAlpha3Code");

            migrationBuilder.CreateIndex(
                name: "IX_ChurchBody_CtryAlpha3Code",
                table: "ChurchBody",
                column: "CtryAlpha3Code");

            migrationBuilder.CreateIndex(
                name: "IX_AppGlobalOwner_CtryAlpha3Code",
                table: "AppGlobalOwner",
                column: "CtryAlpha3Code",
                unique: true,
                filter: "[CtryAlpha3Code] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CountryCustom_AppGlobalOwnerId",
                table: "CountryCustom",
                column: "AppGlobalOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_CountryCustom_ChurchBodyId",
                table: "CountryCustom",
                column: "ChurchBodyId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppGlobalOwner_Country_CtryAlpha3Code",
                table: "AppGlobalOwner",
                column: "CtryAlpha3Code",
                principalTable: "Country",
                principalColumn: "CtryAlpha3Code",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ChurchBody_Country_CtryAlpha3Code",
                table: "ChurchBody",
                column: "CtryAlpha3Code",
                principalTable: "Country",
                principalColumn: "CtryAlpha3Code",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ChurchMember_Country_NationalityId",
                table: "ChurchMember",
                column: "NationalityId",
                principalTable: "Country",
                principalColumn: "CtryAlpha3Code",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ChurchVisitor_Country_NationalityId",
                table: "ChurchVisitor",
                column: "NationalityId",
                principalTable: "Country",
                principalColumn: "CtryAlpha3Code",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ContactInfo_Country_CtryAlpha3Code",
                table: "ContactInfo",
                column: "CtryAlpha3Code",
                principalTable: "Country",
                principalColumn: "CtryAlpha3Code",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CountryRegion_Country_CtryAlpha3Code",
                table: "CountryRegion",
                column: "CtryAlpha3Code",
                principalTable: "Country",
                principalColumn: "CtryAlpha3Code",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Currency_Country_CtryAlpha3Code",
                table: "Currency",
                column: "CtryAlpha3Code",
                principalTable: "Country",
                principalColumn: "CtryAlpha3Code",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LanguageSpoken_Country_CtryAlpha3Code",
                table: "LanguageSpoken",
                column: "CtryAlpha3Code",
                principalTable: "Country",
                principalColumn: "CtryAlpha3Code",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MemberContact_Country_CtryAlpha3Code",
                table: "MemberContact",
                column: "CtryAlpha3Code",
                principalTable: "Country",
                principalColumn: "CtryAlpha3Code",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MemberEducHistory_Country_CtryAlpha3Code",
                table: "MemberEducHistory",
                column: "CtryAlpha3Code",
                principalTable: "Country",
                principalColumn: "CtryAlpha3Code",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MemberRelation_Country_CtryAlpha3CodeExtCon",
                table: "MemberRelation",
                column: "CtryAlpha3CodeExtCon",
                principalTable: "Country",
                principalColumn: "CtryAlpha3Code",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_National_IdType_Country_CtryAlpha3Code",
                table: "National_IdType",
                column: "CtryAlpha3Code",
                principalTable: "Country",
                principalColumn: "CtryAlpha3Code",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppGlobalOwner_Country_CtryAlpha3Code",
                table: "AppGlobalOwner");

            migrationBuilder.DropForeignKey(
                name: "FK_ChurchBody_Country_CtryAlpha3Code",
                table: "ChurchBody");

            migrationBuilder.DropForeignKey(
                name: "FK_ChurchMember_Country_NationalityId",
                table: "ChurchMember");

            migrationBuilder.DropForeignKey(
                name: "FK_ChurchVisitor_Country_NationalityId",
                table: "ChurchVisitor");

            migrationBuilder.DropForeignKey(
                name: "FK_ContactInfo_Country_CtryAlpha3Code",
                table: "ContactInfo");

            migrationBuilder.DropForeignKey(
                name: "FK_CountryRegion_Country_CtryAlpha3Code",
                table: "CountryRegion");

            migrationBuilder.DropForeignKey(
                name: "FK_Currency_Country_CtryAlpha3Code",
                table: "Currency");

            migrationBuilder.DropForeignKey(
                name: "FK_LanguageSpoken_Country_CtryAlpha3Code",
                table: "LanguageSpoken");

            migrationBuilder.DropForeignKey(
                name: "FK_MemberContact_Country_CtryAlpha3Code",
                table: "MemberContact");

            migrationBuilder.DropForeignKey(
                name: "FK_MemberEducHistory_Country_CtryAlpha3Code",
                table: "MemberEducHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_MemberRelation_Country_CtryAlpha3CodeExtCon",
                table: "MemberRelation");

            migrationBuilder.DropForeignKey(
                name: "FK_National_IdType_Country_CtryAlpha3Code",
                table: "National_IdType");

            migrationBuilder.DropTable(
                name: "CountryCustom");

            migrationBuilder.DropTable(
                name: "UserAuditTrail_CL");

            migrationBuilder.DropIndex(
                name: "IX_National_IdType_CtryAlpha3Code",
                table: "National_IdType");

            migrationBuilder.DropIndex(
                name: "IX_MemberRelation_CtryAlpha3CodeExtCon",
                table: "MemberRelation");

            migrationBuilder.DropIndex(
                name: "IX_MemberEducHistory_CtryAlpha3Code",
                table: "MemberEducHistory");

            migrationBuilder.DropIndex(
                name: "IX_MemberContact_CtryAlpha3Code",
                table: "MemberContact");

            migrationBuilder.DropIndex(
                name: "IX_LanguageSpoken_CtryAlpha3Code",
                table: "LanguageSpoken");

            migrationBuilder.DropIndex(
                name: "IX_Currency_CtryAlpha3Code",
                table: "Currency");

            migrationBuilder.DropIndex(
                name: "IX_CountryRegion_CtryAlpha3Code",
                table: "CountryRegion");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Country",
                table: "Country");

            migrationBuilder.DropIndex(
                name: "IX_ContactInfo_CtryAlpha3Code",
                table: "ContactInfo");

            migrationBuilder.DropIndex(
                name: "IX_ChurchBody_CtryAlpha3Code",
                table: "ChurchBody");

            migrationBuilder.DropIndex(
                name: "IX_AppGlobalOwner_CtryAlpha3Code",
                table: "AppGlobalOwner");

            migrationBuilder.DropColumn(
                name: "CtryAlpha3Code",
                table: "National_IdType");

            migrationBuilder.DropColumn(
                name: "CtryAlpha3CodeExtCon",
                table: "MemberRelation");

            migrationBuilder.DropColumn(
                name: "CtryAlpha3Code",
                table: "MemberEducHistory");

            migrationBuilder.DropColumn(
                name: "CtryAlpha3Code",
                table: "MemberContact");

            migrationBuilder.DropColumn(
                name: "CtryAlpha3Code",
                table: "LanguageSpoken");

            migrationBuilder.DropColumn(
                name: "CtryAlpha3Code",
                table: "Currency");

            migrationBuilder.DropColumn(
                name: "CtryAlpha3Code",
                table: "CountryRegion");

            migrationBuilder.DropColumn(
                name: "CtryAlpha3Code",
                table: "Country");

            migrationBuilder.DropColumn(
                name: "CtryAlpha2Code",
                table: "Country");

            migrationBuilder.DropColumn(
                name: "Curr3LISOSymbol",
                table: "Country");

            migrationBuilder.DropColumn(
                name: "CurrEngName",
                table: "Country");

            migrationBuilder.DropColumn(
                name: "CurrLocName",
                table: "Country");

            migrationBuilder.DropColumn(
                name: "CurrSymbol",
                table: "Country");

            migrationBuilder.DropColumn(
                name: "CtryAlpha3Code",
                table: "ContactInfo");

            migrationBuilder.DropColumn(
                name: "CtryAlpha3Code",
                table: "ChurchBody");

            migrationBuilder.DropColumn(
                name: "CtryAlpha3Code",
                table: "AppGlobalOwner");

            migrationBuilder.AddColumn<int>(
                name: "CountryId",
                table: "National_IdType",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CountryIdExtCon",
                table: "MemberRelation",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CountryId",
                table: "MemberEducHistory",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ExtConCountryId",
                table: "MemberContact",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CountryId",
                table: "LanguageSpoken",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CountryId",
                table: "Currency",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CountryId",
                table: "CountryRegion",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Country",
                type: "nvarchar(60)",
                maxLength: 60,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 100);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Country",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<string>(
                name: "Acronym",
                table: "Country",
                type: "nvarchar(3)",
                maxLength: 3,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ChurchBodyId",
                table: "Country",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Currency",
                table: "Country",
                type: "nvarchar(3)",
                maxLength: 3,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsChurchCountry",
                table: "Country",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDefaultCountry",
                table: "Country",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDisplay",
                table: "Country",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "OwnedByChurchBodyId",
                table: "Country",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SharingStatus",
                table: "Country",
                type: "nvarchar(1)",
                maxLength: 1,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CountryId",
                table: "ContactInfo",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "NationalityId",
                table: "ChurchVisitor",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 3,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "NationalityId",
                table: "ChurchMember",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 3,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CountryId",
                table: "ChurchBody",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CountryId",
                table: "AppGlobalOwner",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Country",
                table: "Country",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_National_IdType_CountryId",
                table: "National_IdType",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberRelation_CountryIdExtCon",
                table: "MemberRelation",
                column: "CountryIdExtCon");

            migrationBuilder.CreateIndex(
                name: "IX_MemberEducHistory_CountryId",
                table: "MemberEducHistory",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberContact_ExtConCountryId",
                table: "MemberContact",
                column: "ExtConCountryId");

            migrationBuilder.CreateIndex(
                name: "IX_LanguageSpoken_CountryId",
                table: "LanguageSpoken",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Currency_CountryId",
                table: "Currency",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_CountryRegion_CountryId",
                table: "CountryRegion",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Country_ChurchBodyId",
                table: "Country",
                column: "ChurchBodyId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactInfo_CountryId",
                table: "ContactInfo",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_ChurchBody_CountryId",
                table: "ChurchBody",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_AppGlobalOwner_CountryId",
                table: "AppGlobalOwner",
                column: "CountryId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppGlobalOwner_Country_CountryId",
                table: "AppGlobalOwner",
                column: "CountryId",
                principalTable: "Country",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ChurchBody_Country_CountryId",
                table: "ChurchBody",
                column: "CountryId",
                principalTable: "Country",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ChurchMember_Country_NationalityId",
                table: "ChurchMember",
                column: "NationalityId",
                principalTable: "Country",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ChurchVisitor_Country_NationalityId",
                table: "ChurchVisitor",
                column: "NationalityId",
                principalTable: "Country",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ContactInfo_Country_CountryId",
                table: "ContactInfo",
                column: "CountryId",
                principalTable: "Country",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Country_ChurchBody_ChurchBodyId",
                table: "Country",
                column: "ChurchBodyId",
                principalTable: "ChurchBody",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CountryRegion_Country_CountryId",
                table: "CountryRegion",
                column: "CountryId",
                principalTable: "Country",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Currency_Country_CountryId",
                table: "Currency",
                column: "CountryId",
                principalTable: "Country",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LanguageSpoken_Country_CountryId",
                table: "LanguageSpoken",
                column: "CountryId",
                principalTable: "Country",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MemberContact_Country_ExtConCountryId",
                table: "MemberContact",
                column: "ExtConCountryId",
                principalTable: "Country",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MemberEducHistory_Country_CountryId",
                table: "MemberEducHistory",
                column: "CountryId",
                principalTable: "Country",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MemberRelation_Country_CountryIdExtCon",
                table: "MemberRelation",
                column: "CountryIdExtCon",
                principalTable: "Country",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_National_IdType_Country_CountryId",
                table: "National_IdType",
                column: "CountryId",
                principalTable: "Country",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
