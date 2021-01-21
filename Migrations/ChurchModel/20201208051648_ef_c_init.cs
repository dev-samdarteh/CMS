using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RhemaCMS.Migrations.ChurchModel
{
    public partial class ef_c_init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccountPeriod",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppGlobalOwnerId = table.Column<int>(nullable: true),
                    ChurchBodyId = table.Column<int>(nullable: true),
                    OwnedByChurchBodyId = table.Column<int>(nullable: true),
                    PeriodIndex = table.Column<int>(nullable: false),
                    PeriodCode = table.Column<string>(maxLength: 2, nullable: true),
                    PeriodDesc = table.Column<string>(maxLength: 15, nullable: true),
                    ChurchPeriodId = table.Column<int>(nullable: true),
                    StartDate = table.Column<DateTime>(nullable: true),
                    EndDate = table.Column<DateTime>(nullable: true),
                    LengthInDays = table.Column<int>(nullable: false),
                    PeriodType = table.Column<string>(maxLength: 2, nullable: true),
                    PeriodStatus = table.Column<string>(maxLength: 1, nullable: true),
                    LongevityStatus = table.Column<string>(maxLength: 1, nullable: true),
                    SharingStatus = table.Column<string>(maxLength: 1, nullable: true),
                    Created = table.Column<DateTime>(nullable: true),
                    LastMod = table.Column<DateTime>(nullable: true),
                    CreatedByUserId = table.Column<int>(nullable: true),
                    LastModByUserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountPeriod", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppUtilityNVP",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppGlobalOwnerId = table.Column<int>(nullable: true),
                    ChurchBodyId = table.Column<int>(nullable: true),
                    NVPTag = table.Column<string>(maxLength: 30, nullable: true),
                    NVPCode = table.Column<string>(maxLength: 15, nullable: true),
                    Acronym = table.Column<string>(maxLength: 10, nullable: true),
                    NVPStatus = table.Column<string>(maxLength: 1, nullable: true),
                    NVPValue = table.Column<string>(maxLength: 100, nullable: true),
                    NVPCategoryId = table.Column<int>(nullable: true),
                    OrderIndex = table.Column<int>(nullable: true),
                    RequireUserCustom = table.Column<bool>(nullable: true),
                    OwnedByChurchBodyId = table.Column<int>(nullable: true),
                    Created = table.Column<DateTime>(nullable: true),
                    LastMod = table.Column<DateTime>(nullable: true),
                    CreatedByUserId = table.Column<int>(nullable: true),
                    LastModByUserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUtilityNVP", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppUtilityNVP_AppUtilityNVP_NVPCategoryId",
                        column: x => x.NVPCategoryId,
                        principalTable: "AppUtilityNVP",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ChurchBody",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MSTR_AppGlobalOwnerId = table.Column<int>(nullable: true),
                    MSTR_ChurchBodyId = table.Column<int>(nullable: true),
                    MSTR_ParentChurchBodyId = table.Column<int>(nullable: true),
                    MSTR_ChurchLevelId = table.Column<int>(nullable: true),
                    AppGlobalOwnerId = table.Column<int>(nullable: true),
                    ChurchLevelId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    GlobalChurchCode = table.Column<string>(maxLength: 20, nullable: true),
                    RootChurchCode = table.Column<string>(maxLength: 200, nullable: true),
                    OrganisationType = table.Column<string>(maxLength: 2, nullable: true),
                    SubscriptionKey = table.Column<string>(maxLength: 50, nullable: true),
                    ParentChurchBodyId = table.Column<int>(nullable: true),
                    ContactInfoId = table.Column<int>(nullable: true),
                    CountryId = table.Column<int>(nullable: true),
                    CountryRegionId = table.Column<int>(nullable: true),
                    Comments = table.Column<string>(maxLength: 200, nullable: true),
                    Status = table.Column<string>(maxLength: 1, nullable: true),
                    ChurchCodeCustom = table.Column<string>(maxLength: 20, nullable: true),
                    Formed = table.Column<DateTime>(nullable: true),
                    Innaug = table.Column<DateTime>(nullable: true),
                    ChurchType = table.Column<string>(maxLength: 2, nullable: true),
                    BriefHistory = table.Column<string>(maxLength: 500, nullable: true),
                    ChurchUnitIndex = table.Column<double>(nullable: true),
                    Description = table.Column<string>(maxLength: 200, nullable: true),
                    IsActivated = table.Column<bool>(nullable: true),
                    OwnedByChurchBodyId = table.Column<int>(nullable: true),
                    SharingStatus = table.Column<string>(maxLength: 1, nullable: true),
                    ChurchWorkStatus = table.Column<string>(maxLength: 1, nullable: true),
                    GenderStatus = table.Column<string>(maxLength: 1, nullable: true),
                    ChurchUnitLogo = table.Column<string>(nullable: true),
                    IsUnitGenerational = table.Column<bool>(nullable: true),
                    ChurchUnitMaxAge = table.Column<int>(nullable: true),
                    ChurchUnitMinAge = table.Column<int>(nullable: true),
                    IsFullAutonomy = table.Column<bool>(nullable: false),
                    GradeLevel = table.Column<int>(nullable: false),
                    ApplyToClergyOnly = table.Column<bool>(nullable: false),
                    SupervisedByChurchBodyId = table.Column<int>(nullable: true),
                    MaxNumAllowed = table.Column<int>(nullable: true),
                    MinNumAllowed = table.Column<int>(nullable: true),
                    TagName = table.Column<string>(maxLength: 50, nullable: true),
                    PrimaryFunction = table.Column<string>(maxLength: 200, nullable: true),
                    OwnershipStatus = table.Column<string>(maxLength: 1, nullable: true),
                    Created = table.Column<DateTime>(nullable: true),
                    LastMod = table.Column<DateTime>(nullable: true),
                    CreatedByUserId = table.Column<int>(nullable: true),
                    LastModByUserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChurchBody", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChurchBody_ChurchBody_OwnedByChurchBodyId",
                        column: x => x.OwnedByChurchBodyId,
                        principalTable: "ChurchBody",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChurchBody_ChurchBody_ParentChurchBodyId",
                        column: x => x.ParentChurchBodyId,
                        principalTable: "ChurchBody",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CertificateType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChurchBodyId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    CertLevel = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    OwnedByChurchBodyId = table.Column<int>(nullable: true),
                    SharingStatus = table.Column<string>(maxLength: 1, nullable: true),
                    Created = table.Column<DateTime>(nullable: true),
                    LastMod = table.Column<DateTime>(nullable: true),
                    CreatedByUserId = table.Column<int>(nullable: true),
                    LastModByUserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CertificateType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CertificateType_ChurchBody_ChurchBodyId",
                        column: x => x.ChurchBodyId,
                        principalTable: "ChurchBody",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CertificateType_ChurchBody_OwnedByChurchBodyId",
                        column: x => x.OwnedByChurchBodyId,
                        principalTable: "ChurchBody",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ChurchLevel",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MSTR_AppGlobalOwnerId = table.Column<int>(nullable: true),
                    MSTR_ChurchLevelId = table.Column<int>(nullable: true),
                    AppGlobalOwnerId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(maxLength: 8, nullable: true),
                    CustomName = table.Column<string>(maxLength: 50, nullable: true),
                    LevelIndex = table.Column<int>(nullable: false),
                    SharingStatus = table.Column<string>(maxLength: 1, nullable: true),
                    Acronym = table.Column<string>(maxLength: 10, nullable: true),
                    Created = table.Column<DateTime>(nullable: true),
                    LastMod = table.Column<DateTime>(nullable: true),
                    CreatedByUserId = table.Column<int>(nullable: true),
                    LastModByUserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChurchLevel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ChurchMember",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppGlobalOwnerId = table.Column<int>(nullable: true),
                    OwnedByChurchBodyId = table.Column<int>(nullable: true),
                    ChurchBodyId = table.Column<int>(nullable: true),
                    MemberCode = table.Column<string>(maxLength: 50, nullable: true),
                    RootChurchCode = table.Column<string>(maxLength: 200, nullable: true),
                    Title = table.Column<string>(maxLength: 10, nullable: true),
                    FirstName = table.Column<string>(maxLength: 30, nullable: true),
                    MiddleName = table.Column<string>(maxLength: 30, nullable: true),
                    LastName = table.Column<string>(maxLength: 30, nullable: true),
                    MaidenName = table.Column<string>(maxLength: 100, nullable: true),
                    Gender = table.Column<string>(maxLength: 1, nullable: true),
                    DateOfBirth = table.Column<DateTime>(nullable: true),
                    MaritalStatus = table.Column<string>(maxLength: 1, nullable: true),
                    MarriageType = table.Column<string>(maxLength: 1, nullable: true),
                    NationalityId = table.Column<int>(nullable: true),
                    ContactInfoId = table.Column<int>(nullable: true),
                    Hobbies = table.Column<string>(maxLength: 50, nullable: true),
                    Hometown = table.Column<string>(maxLength: 30, nullable: true),
                    HometownRegionId = table.Column<int>(nullable: true),
                    IdTypeId = table.Column<int>(nullable: true),
                    PhotoUrl = table.Column<string>(nullable: true),
                    OtherInfo = table.Column<string>(maxLength: 300, nullable: true),
                    MotherTongueId = table.Column<int>(nullable: true),
                    National_IdNum = table.Column<string>(maxLength: 30, nullable: true),
                    IsActivated = table.Column<bool>(nullable: false),
                    MarriageRegNo = table.Column<string>(maxLength: 50, nullable: true),
                    MemberGlobalId = table.Column<string>(maxLength: 50, nullable: true),
                    MemberTypeId = table.Column<int>(nullable: true),
                    MemberCustomCode = table.Column<string>(maxLength: 50, nullable: true),
                    MemberClass = table.Column<string>(maxLength: 1, nullable: true),
                    Status = table.Column<string>(maxLength: 1, nullable: true),
                    Created = table.Column<DateTime>(nullable: true),
                    LastMod = table.Column<DateTime>(nullable: true),
                    CreatedByUserId = table.Column<int>(nullable: true),
                    LastModByUserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChurchMember", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChurchMember_ChurchBody_ChurchBodyId",
                        column: x => x.ChurchBodyId,
                        principalTable: "ChurchBody",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ChurchPeriod",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppGlobalOwnerId = table.Column<int>(nullable: true),
                    ChurchBodyId = table.Column<int>(nullable: true),
                    From = table.Column<DateTime>(nullable: true),
                    To = table.Column<DateTime>(nullable: true),
                    Status = table.Column<string>(maxLength: 1, nullable: true),
                    PeriodDesc = table.Column<string>(maxLength: 50, nullable: true),
                    LengthInDays = table.Column<int>(nullable: false),
                    PeriodType = table.Column<string>(maxLength: 2, nullable: true),
                    SharingStatus = table.Column<string>(maxLength: 1, nullable: true),
                    OwnedByChurchBodyId = table.Column<int>(nullable: true),
                    Created = table.Column<DateTime>(nullable: true),
                    LastMod = table.Column<DateTime>(nullable: true),
                    CreatedByUserId = table.Column<int>(nullable: true),
                    LastModByUserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChurchPeriod", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChurchPeriod_ChurchBody_ChurchBodyId",
                        column: x => x.ChurchBodyId,
                        principalTable: "ChurchBody",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Country",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppGlobalOwnerId = table.Column<int>(nullable: true),
                    ChurchBodyId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(maxLength: 60, nullable: false),
                    Acronym = table.Column<string>(maxLength: 3, nullable: true),
                    Currency = table.Column<string>(maxLength: 3, nullable: true),
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
                    table.PrimaryKey("PK_Country", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Country_ChurchBody_ChurchBodyId",
                        column: x => x.ChurchBodyId,
                        principalTable: "ChurchBody",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CountryRegion",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppGlobalOwnerId = table.Column<int>(nullable: true),
                    ChurchBodyId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    CountryId = table.Column<int>(nullable: true),
                    RegCode = table.Column<string>(maxLength: 3, nullable: true),
                    SharingStatus = table.Column<string>(maxLength: 1, nullable: true),
                    OwnedByChurchBodyId = table.Column<int>(nullable: true),
                    Created = table.Column<DateTime>(nullable: true),
                    LastMod = table.Column<DateTime>(nullable: true),
                    CreatedByUserId = table.Column<int>(nullable: true),
                    LastModByUserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CountryRegion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CountryRegion_ChurchBody_ChurchBodyId",
                        column: x => x.ChurchBodyId,
                        principalTable: "ChurchBody",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CountryRegion_Country_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Country",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ContactInfo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppGlobalOwnerId = table.Column<int>(nullable: true),
                    ChurchBodyId = table.Column<int>(nullable: true),
                    ContactRef = table.Column<string>(maxLength: 50, nullable: true),
                    ChurchFellow = table.Column<bool>(nullable: false),
                    ChurchMemberId = table.Column<int>(nullable: true),
                    ContactName = table.Column<string>(maxLength: 100, nullable: true),
                    ResidenceAddress = table.Column<string>(maxLength: 100, nullable: true),
                    Location = table.Column<string>(maxLength: 30, nullable: true),
                    City = table.Column<string>(maxLength: 30, nullable: true),
                    RegionId = table.Column<int>(nullable: true),
                    CountryId = table.Column<int>(nullable: true),
                    ResAddrSameAsPostAddr = table.Column<bool>(nullable: false),
                    PostalAddress = table.Column<string>(maxLength: 30, nullable: true),
                    DigitalAddress = table.Column<string>(maxLength: 30, nullable: true),
                    Telephone = table.Column<string>(maxLength: 15, nullable: true),
                    MobilePhone1 = table.Column<string>(maxLength: 15, nullable: true),
                    MobilePhone2 = table.Column<string>(maxLength: 15, nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Website = table.Column<string>(nullable: true),
                    Created = table.Column<DateTime>(nullable: true),
                    LastMod = table.Column<DateTime>(nullable: true),
                    CreatedByUserId = table.Column<int>(nullable: true),
                    LastModByUserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContactInfo_ChurchBody_ChurchBodyId",
                        column: x => x.ChurchBodyId,
                        principalTable: "ChurchBody",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ContactInfo_Country_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Country",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ContactInfo_CountryRegion_RegionId",
                        column: x => x.RegionId,
                        principalTable: "CountryRegion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AppGlobalOwner",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MSTR_AppGlobalOwnerId = table.Column<int>(nullable: true),
                    OwnerName = table.Column<string>(maxLength: 100, nullable: false),
                    Allias = table.Column<string>(maxLength: 50, nullable: true),
                    Acronym = table.Column<string>(maxLength: 10, nullable: true),
                    PrefixKey = table.Column<string>(maxLength: 13, nullable: true),
                    GlobalChurchCode = table.Column<string>(maxLength: 20, nullable: true),
                    RootChurchCode = table.Column<string>(maxLength: 20, nullable: true),
                    TotalLevels = table.Column<int>(nullable: true),
                    Motto = table.Column<string>(maxLength: 100, nullable: true),
                    Slogan = table.Column<string>(maxLength: 100, nullable: true),
                    ContactInfoId = table.Column<int>(nullable: true),
                    CountryId = table.Column<int>(nullable: true),
                    FaithTypeCategoryId = table.Column<int>(nullable: true),
                    Status = table.Column<string>(maxLength: 1, nullable: true),
                    ChurchLogo = table.Column<string>(nullable: true),
                    Comments = table.Column<string>(maxLength: 200, nullable: true),
                    Created = table.Column<DateTime>(nullable: true),
                    LastMod = table.Column<DateTime>(nullable: true),
                    CreatedByUserId = table.Column<int>(nullable: true),
                    LastModByUserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppGlobalOwner", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppGlobalOwner_ContactInfo_ContactInfoId",
                        column: x => x.ContactInfoId,
                        principalTable: "ContactInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AppGlobalOwner_Country_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Country",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Currency",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppGlobalOwnerId = table.Column<int>(nullable: true),
                    ChurchBodyId = table.Column<int>(nullable: true),
                    CurrencyName = table.Column<string>(maxLength: 50, nullable: true),
                    Acronym = table.Column<string>(maxLength: 3, nullable: true),
                    SubAcronym = table.Column<string>(maxLength: 3, nullable: true),
                    Symbol = table.Column<string>(maxLength: 3, nullable: true),
                    SubSymbol = table.Column<string>(maxLength: 3, nullable: true),
                    CountryId = table.Column<int>(nullable: true),
                    IsBaseCurrency = table.Column<bool>(nullable: false),
                    BaseRate = table.Column<decimal>(type: "decimal(18, 4)", nullable: false),
                    Status = table.Column<string>(maxLength: 1, nullable: true),
                    OwnedByChurchBodyId = table.Column<int>(nullable: true),
                    Created = table.Column<DateTime>(nullable: true),
                    LastMod = table.Column<DateTime>(nullable: true),
                    CreatedByUserId = table.Column<int>(nullable: true),
                    LastModByUserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currency", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Currency_AppGlobalOwner_AppGlobalOwnerId",
                        column: x => x.AppGlobalOwnerId,
                        principalTable: "AppGlobalOwner",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Currency_ChurchBody_ChurchBodyId",
                        column: x => x.ChurchBodyId,
                        principalTable: "ChurchBody",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Currency_Country_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Country",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InstitutionType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppGlobalOwnerId = table.Column<int>(nullable: true),
                    ChurchBodyId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    EduLevelIndex = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    SharingStatus = table.Column<string>(maxLength: 1, nullable: true),
                    OwnedByChurchBodyId = table.Column<int>(nullable: true),
                    Created = table.Column<DateTime>(nullable: true),
                    LastMod = table.Column<DateTime>(nullable: true),
                    CreatedByUserId = table.Column<int>(nullable: true),
                    LastModByUserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstitutionType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InstitutionType_AppGlobalOwner_AppGlobalOwnerId",
                        column: x => x.AppGlobalOwnerId,
                        principalTable: "AppGlobalOwner",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InstitutionType_ChurchBody_ChurchBodyId",
                        column: x => x.ChurchBodyId,
                        principalTable: "ChurchBody",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LanguageSpoken",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppGlobalOwnerId = table.Column<int>(nullable: true),
                    ChurchBodyId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    CountryId = table.Column<int>(nullable: true),
                    SharingStatus = table.Column<string>(maxLength: 1, nullable: true),
                    OwnedByChurchBodyId = table.Column<int>(nullable: true),
                    Created = table.Column<DateTime>(nullable: true),
                    LastMod = table.Column<DateTime>(nullable: true),
                    CreatedByUserId = table.Column<int>(nullable: true),
                    LastModByUserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LanguageSpoken", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LanguageSpoken_AppGlobalOwner_AppGlobalOwnerId",
                        column: x => x.AppGlobalOwnerId,
                        principalTable: "AppGlobalOwner",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LanguageSpoken_ChurchBody_ChurchBodyId",
                        column: x => x.ChurchBodyId,
                        principalTable: "ChurchBody",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LanguageSpoken_Country_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Country",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "National_IdType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppGlobalOwnerId = table.Column<int>(nullable: true),
                    ChurchBodyId = table.Column<int>(nullable: true),
                    OwnedByChurchBodyId = table.Column<int>(nullable: true),
                    IdTypeDesc = table.Column<string>(maxLength: 50, nullable: false),
                    CountryId = table.Column<int>(nullable: true),
                    SharingStatus = table.Column<string>(maxLength: 1, nullable: true),
                    Created = table.Column<DateTime>(nullable: true),
                    LastMod = table.Column<DateTime>(nullable: true),
                    CreatedByUserId = table.Column<int>(nullable: true),
                    LastModByUserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_National_IdType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_National_IdType_AppGlobalOwner_AppGlobalOwnerId",
                        column: x => x.AppGlobalOwnerId,
                        principalTable: "AppGlobalOwner",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_National_IdType_ChurchBody_ChurchBodyId",
                        column: x => x.ChurchBodyId,
                        principalTable: "ChurchBody",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_National_IdType_Country_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Country",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RelationshipType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppGlobalOwnerId = table.Column<int>(nullable: true),
                    ChurchBodyId = table.Column<int>(nullable: true),
                    OwnedByChurchBodyId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(maxLength: 30, nullable: false),
                    RelationIndex = table.Column<int>(nullable: true),
                    RelationshipTypeFemalePairId = table.Column<int>(nullable: true),
                    RelationshipTypeGenericPairId = table.Column<int>(nullable: true),
                    RelationshipTypeMalePairId = table.Column<int>(nullable: true),
                    IsChild = table.Column<bool>(nullable: false),
                    IsSpouse = table.Column<bool>(nullable: false),
                    Created = table.Column<DateTime>(nullable: true),
                    LastMod = table.Column<DateTime>(nullable: true),
                    CreatedByUserId = table.Column<int>(nullable: true),
                    LastModByUserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RelationshipType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RelationshipType_AppGlobalOwner_AppGlobalOwnerId",
                        column: x => x.AppGlobalOwnerId,
                        principalTable: "AppGlobalOwner",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RelationshipType_ChurchBody_ChurchBodyId",
                        column: x => x.ChurchBodyId,
                        principalTable: "ChurchBody",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccountPeriod_AppGlobalOwnerId",
                table: "AccountPeriod",
                column: "AppGlobalOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountPeriod_ChurchBodyId",
                table: "AccountPeriod",
                column: "ChurchBodyId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountPeriod_ChurchPeriodId",
                table: "AccountPeriod",
                column: "ChurchPeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_AppGlobalOwner_ContactInfoId",
                table: "AppGlobalOwner",
                column: "ContactInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_AppGlobalOwner_CountryId",
                table: "AppGlobalOwner",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_AppUtilityNVP_AppGlobalOwnerId",
                table: "AppUtilityNVP",
                column: "AppGlobalOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_AppUtilityNVP_ChurchBodyId",
                table: "AppUtilityNVP",
                column: "ChurchBodyId");

            migrationBuilder.CreateIndex(
                name: "IX_AppUtilityNVP_NVPCategoryId",
                table: "AppUtilityNVP",
                column: "NVPCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CertificateType_ChurchBodyId",
                table: "CertificateType",
                column: "ChurchBodyId");

            migrationBuilder.CreateIndex(
                name: "IX_CertificateType_OwnedByChurchBodyId",
                table: "CertificateType",
                column: "OwnedByChurchBodyId");

            migrationBuilder.CreateIndex(
                name: "IX_ChurchBody_AppGlobalOwnerId",
                table: "ChurchBody",
                column: "AppGlobalOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_ChurchBody_ChurchLevelId",
                table: "ChurchBody",
                column: "ChurchLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_ChurchBody_ContactInfoId",
                table: "ChurchBody",
                column: "ContactInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_ChurchBody_CountryId",
                table: "ChurchBody",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_ChurchBody_CountryRegionId",
                table: "ChurchBody",
                column: "CountryRegionId");

            migrationBuilder.CreateIndex(
                name: "IX_ChurchBody_OwnedByChurchBodyId",
                table: "ChurchBody",
                column: "OwnedByChurchBodyId");

            migrationBuilder.CreateIndex(
                name: "IX_ChurchBody_ParentChurchBodyId",
                table: "ChurchBody",
                column: "ParentChurchBodyId");

            migrationBuilder.CreateIndex(
                name: "IX_ChurchLevel_AppGlobalOwnerId",
                table: "ChurchLevel",
                column: "AppGlobalOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_ChurchMember_AppGlobalOwnerId",
                table: "ChurchMember",
                column: "AppGlobalOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_ChurchMember_ChurchBodyId",
                table: "ChurchMember",
                column: "ChurchBodyId");

            migrationBuilder.CreateIndex(
                name: "IX_ChurchMember_HometownRegionId",
                table: "ChurchMember",
                column: "HometownRegionId");

            migrationBuilder.CreateIndex(
                name: "IX_ChurchMember_IdTypeId",
                table: "ChurchMember",
                column: "IdTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ChurchMember_MotherTongueId",
                table: "ChurchMember",
                column: "MotherTongueId");

            migrationBuilder.CreateIndex(
                name: "IX_ChurchMember_NationalityId",
                table: "ChurchMember",
                column: "NationalityId");

            migrationBuilder.CreateIndex(
                name: "IX_ChurchPeriod_AppGlobalOwnerId",
                table: "ChurchPeriod",
                column: "AppGlobalOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_ChurchPeriod_ChurchBodyId",
                table: "ChurchPeriod",
                column: "ChurchBodyId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactInfo_ChurchBodyId",
                table: "ContactInfo",
                column: "ChurchBodyId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactInfo_CountryId",
                table: "ContactInfo",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactInfo_RegionId",
                table: "ContactInfo",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_Country_AppGlobalOwnerId",
                table: "Country",
                column: "AppGlobalOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Country_ChurchBodyId",
                table: "Country",
                column: "ChurchBodyId");

            migrationBuilder.CreateIndex(
                name: "IX_CountryRegion_AppGlobalOwnerId",
                table: "CountryRegion",
                column: "AppGlobalOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_CountryRegion_ChurchBodyId",
                table: "CountryRegion",
                column: "ChurchBodyId");

            migrationBuilder.CreateIndex(
                name: "IX_CountryRegion_CountryId",
                table: "CountryRegion",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Currency_AppGlobalOwnerId",
                table: "Currency",
                column: "AppGlobalOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Currency_ChurchBodyId",
                table: "Currency",
                column: "ChurchBodyId");

            migrationBuilder.CreateIndex(
                name: "IX_Currency_CountryId",
                table: "Currency",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_InstitutionType_AppGlobalOwnerId",
                table: "InstitutionType",
                column: "AppGlobalOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_InstitutionType_ChurchBodyId",
                table: "InstitutionType",
                column: "ChurchBodyId");

            migrationBuilder.CreateIndex(
                name: "IX_LanguageSpoken_AppGlobalOwnerId",
                table: "LanguageSpoken",
                column: "AppGlobalOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_LanguageSpoken_ChurchBodyId",
                table: "LanguageSpoken",
                column: "ChurchBodyId");

            migrationBuilder.CreateIndex(
                name: "IX_LanguageSpoken_CountryId",
                table: "LanguageSpoken",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_National_IdType_AppGlobalOwnerId",
                table: "National_IdType",
                column: "AppGlobalOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_National_IdType_ChurchBodyId",
                table: "National_IdType",
                column: "ChurchBodyId");

            migrationBuilder.CreateIndex(
                name: "IX_National_IdType_CountryId",
                table: "National_IdType",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_RelationshipType_AppGlobalOwnerId",
                table: "RelationshipType",
                column: "AppGlobalOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_RelationshipType_ChurchBodyId",
                table: "RelationshipType",
                column: "ChurchBodyId");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountPeriod_AppGlobalOwner_AppGlobalOwnerId",
                table: "AccountPeriod",
                column: "AppGlobalOwnerId",
                principalTable: "AppGlobalOwner",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AccountPeriod_ChurchBody_ChurchBodyId",
                table: "AccountPeriod",
                column: "ChurchBodyId",
                principalTable: "ChurchBody",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AccountPeriod_ChurchPeriod_ChurchPeriodId",
                table: "AccountPeriod",
                column: "ChurchPeriodId",
                principalTable: "ChurchPeriod",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AppUtilityNVP_AppGlobalOwner_AppGlobalOwnerId",
                table: "AppUtilityNVP",
                column: "AppGlobalOwnerId",
                principalTable: "AppGlobalOwner",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AppUtilityNVP_ChurchBody_ChurchBodyId",
                table: "AppUtilityNVP",
                column: "ChurchBodyId",
                principalTable: "ChurchBody",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ChurchBody_AppGlobalOwner_AppGlobalOwnerId",
                table: "ChurchBody",
                column: "AppGlobalOwnerId",
                principalTable: "AppGlobalOwner",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ChurchBody_ContactInfo_ContactInfoId",
                table: "ChurchBody",
                column: "ContactInfoId",
                principalTable: "ContactInfo",
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
                name: "FK_ChurchBody_ChurchLevel_ChurchLevelId",
                table: "ChurchBody",
                column: "ChurchLevelId",
                principalTable: "ChurchLevel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ChurchBody_CountryRegion_CountryRegionId",
                table: "ChurchBody",
                column: "CountryRegionId",
                principalTable: "CountryRegion",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ChurchLevel_AppGlobalOwner_AppGlobalOwnerId",
                table: "ChurchLevel",
                column: "AppGlobalOwnerId",
                principalTable: "AppGlobalOwner",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ChurchMember_AppGlobalOwner_AppGlobalOwnerId",
                table: "ChurchMember",
                column: "AppGlobalOwnerId",
                principalTable: "AppGlobalOwner",
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
                name: "FK_ChurchMember_CountryRegion_HometownRegionId",
                table: "ChurchMember",
                column: "HometownRegionId",
                principalTable: "CountryRegion",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ChurchMember_National_IdType_IdTypeId",
                table: "ChurchMember",
                column: "IdTypeId",
                principalTable: "National_IdType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ChurchMember_LanguageSpoken_MotherTongueId",
                table: "ChurchMember",
                column: "MotherTongueId",
                principalTable: "LanguageSpoken",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ChurchPeriod_AppGlobalOwner_AppGlobalOwnerId",
                table: "ChurchPeriod",
                column: "AppGlobalOwnerId",
                principalTable: "AppGlobalOwner",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Country_AppGlobalOwner_AppGlobalOwnerId",
                table: "Country",
                column: "AppGlobalOwnerId",
                principalTable: "AppGlobalOwner",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CountryRegion_AppGlobalOwner_AppGlobalOwnerId",
                table: "CountryRegion",
                column: "AppGlobalOwnerId",
                principalTable: "AppGlobalOwner",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChurchBody_AppGlobalOwner_AppGlobalOwnerId",
                table: "ChurchBody");

            migrationBuilder.DropForeignKey(
                name: "FK_ChurchLevel_AppGlobalOwner_AppGlobalOwnerId",
                table: "ChurchLevel");

            migrationBuilder.DropForeignKey(
                name: "FK_Country_AppGlobalOwner_AppGlobalOwnerId",
                table: "Country");

            migrationBuilder.DropForeignKey(
                name: "FK_CountryRegion_AppGlobalOwner_AppGlobalOwnerId",
                table: "CountryRegion");

            migrationBuilder.DropForeignKey(
                name: "FK_ContactInfo_ChurchBody_ChurchBodyId",
                table: "ContactInfo");

            migrationBuilder.DropForeignKey(
                name: "FK_Country_ChurchBody_ChurchBodyId",
                table: "Country");

            migrationBuilder.DropForeignKey(
                name: "FK_CountryRegion_ChurchBody_ChurchBodyId",
                table: "CountryRegion");

            migrationBuilder.DropTable(
                name: "AccountPeriod");

            migrationBuilder.DropTable(
                name: "AppUtilityNVP");

            migrationBuilder.DropTable(
                name: "CertificateType");

            migrationBuilder.DropTable(
                name: "ChurchMember");

            migrationBuilder.DropTable(
                name: "Currency");

            migrationBuilder.DropTable(
                name: "InstitutionType");

            migrationBuilder.DropTable(
                name: "RelationshipType");

            migrationBuilder.DropTable(
                name: "ChurchPeriod");

            migrationBuilder.DropTable(
                name: "National_IdType");

            migrationBuilder.DropTable(
                name: "LanguageSpoken");

            migrationBuilder.DropTable(
                name: "AppGlobalOwner");

            migrationBuilder.DropTable(
                name: "ChurchBody");

            migrationBuilder.DropTable(
                name: "ChurchLevel");

            migrationBuilder.DropTable(
                name: "ContactInfo");

            migrationBuilder.DropTable(
                name: "CountryRegion");

            migrationBuilder.DropTable(
                name: "Country");
        }
    }
}
