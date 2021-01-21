using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RhemaCMS.Migrations
{
    public partial class ef_m_init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppSubscriptionPackage",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Description = table.Column<string>(maxLength: 100, nullable: true),
                    UnitCost = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    Created = table.Column<DateTime>(nullable: true),
                    LastMod = table.Column<DateTime>(nullable: true),
                    CreatedByUserId = table.Column<int>(nullable: true),
                    LastModByUserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppSubscriptionPackage", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ChurchFaithType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FaithDescription = table.Column<string>(maxLength: 50, nullable: true),
                    Level = table.Column<int>(nullable: false),
                    FaithTypeClassId = table.Column<int>(nullable: true),
                    Category = table.Column<string>(maxLength: 2, nullable: true),
                    Created = table.Column<DateTime>(nullable: true),
                    LastMod = table.Column<DateTime>(nullable: true),
                    CreatedByUserId = table.Column<int>(nullable: true),
                    LastModByUserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChurchFaithType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChurchFaithType_ChurchFaithType_FaithTypeClassId",
                        column: x => x.FaithTypeClassId,
                        principalTable: "ChurchFaithType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MSTRChurchLevel",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
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
                    table.PrimaryKey("PK_MSTRChurchLevel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MSTRCountry",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    Acronym = table.Column<string>(maxLength: 3, nullable: true),
                    Currency = table.Column<string>(maxLength: 3, nullable: true),
                    SharingStatus = table.Column<string>(maxLength: 1, nullable: true),
                    Created = table.Column<DateTime>(nullable: true),
                    LastMod = table.Column<DateTime>(nullable: true),
                    CreatedByUserId = table.Column<int>(nullable: true),
                    LastModByUserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MSTRCountry", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MSTRCountryRegion",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    CountryId = table.Column<int>(nullable: true),
                    RegCode = table.Column<string>(maxLength: 3, nullable: true),
                    SharingStatus = table.Column<string>(maxLength: 1, nullable: true),
                    Created = table.Column<DateTime>(nullable: true),
                    LastMod = table.Column<DateTime>(nullable: true),
                    CreatedByUserId = table.Column<int>(nullable: true),
                    LastModByUserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MSTRCountryRegion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MSTRCountryRegion_MSTRCountry_CountryId",
                        column: x => x.CountryId,
                        principalTable: "MSTRCountry",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SubscriptionChurchBody",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GlobalSubscriptionId = table.Column<int>(nullable: false),
                    ChurchBodyId = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(nullable: true),
                    LastMod = table.Column<DateTime>(nullable: true),
                    CreatedByUserId = table.Column<int>(nullable: true),
                    LastModByUserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscriptionChurchBody", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppSubscription",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppGlobalOwnerId = table.Column<int>(nullable: true),
                    ChurchBodyId = table.Column<int>(nullable: true),
                    UserDesc = table.Column<string>(maxLength: 100, nullable: false),
                    Username = table.Column<string>(maxLength: 30, nullable: true),
                    Pwd = table.Column<string>(maxLength: 30, nullable: true),
                    SubscriberPhoneNum = table.Column<string>(nullable: true),
                    SLADesc = table.Column<string>(maxLength: 200, nullable: true),
                    AppSubscriptionPackageId = table.Column<int>(nullable: true),
                    SubscriptionPeriod = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    SubscriptionKey = table.Column<string>(maxLength: 50, nullable: true),
                    TotalSubscribed = table.Column<int>(nullable: true),
                    SubscriptionDate = table.Column<DateTime>(nullable: true),
                    STRT = table.Column<DateTime>(nullable: true),
                    EXPR = table.Column<DateTime>(nullable: true),
                    SLAStatus = table.Column<string>(maxLength: 1, nullable: true),
                    StatusReason = table.Column<string>(maxLength: 100, nullable: true),
                    Created = table.Column<DateTime>(nullable: true),
                    LastMod = table.Column<DateTime>(nullable: true),
                    CreatedByUserId = table.Column<int>(nullable: true),
                    LastModByUserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppSubscription", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppSubscription_AppSubscriptionPackage_AppSubscriptionPackageId",
                        column: x => x.AppSubscriptionPackageId,
                        principalTable: "AppSubscriptionPackage",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MSTRAppGlobalOwner",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
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
                    table.PrimaryKey("PK_MSTRAppGlobalOwner", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MSTRAppGlobalOwner_MSTRCountry_CountryId",
                        column: x => x.CountryId,
                        principalTable: "MSTRCountry",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MSTRAppGlobalOwner_ChurchFaithType_FaithTypeCategoryId",
                        column: x => x.FaithTypeCategoryId,
                        principalTable: "ChurchFaithType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ClientAppServerConfig",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppGlobalOwnerId = table.Column<int>(nullable: true),
                    ServerName = table.Column<string>(maxLength: 100, nullable: true),
                    DbaseName = table.Column<string>(maxLength: 100, nullable: true),
                    SvrUserId = table.Column<string>(maxLength: 100, nullable: true),
                    SvrPassword = table.Column<string>(maxLength: 100, nullable: true),
                    ConfigDate = table.Column<DateTime>(nullable: true),
                    Status = table.Column<string>(maxLength: 1, nullable: true),
                    Created = table.Column<DateTime>(nullable: true),
                    LastMod = table.Column<DateTime>(nullable: true),
                    CreatedByUserId = table.Column<int>(nullable: true),
                    LastModByUserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientAppServerConfig", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientAppServerConfig_MSTRAppGlobalOwner_AppGlobalOwnerId",
                        column: x => x.AppGlobalOwnerId,
                        principalTable: "MSTRAppGlobalOwner",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MSTRChurchBody",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
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
                    Created = table.Column<DateTime>(nullable: true),
                    LastMod = table.Column<DateTime>(nullable: true),
                    CreatedByUserId = table.Column<int>(nullable: true),
                    LastModByUserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MSTRChurchBody", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MSTRChurchBody_MSTRAppGlobalOwner_AppGlobalOwnerId",
                        column: x => x.AppGlobalOwnerId,
                        principalTable: "MSTRAppGlobalOwner",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MSTRChurchBody_MSTRChurchLevel_ChurchLevelId",
                        column: x => x.ChurchLevelId,
                        principalTable: "MSTRChurchLevel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MSTRChurchBody_MSTRCountry_CountryId",
                        column: x => x.CountryId,
                        principalTable: "MSTRCountry",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MSTRChurchBody_MSTRCountryRegion_CountryRegionId",
                        column: x => x.CountryRegionId,
                        principalTable: "MSTRCountryRegion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MSTRChurchBody_MSTRChurchBody_ParentChurchBodyId",
                        column: x => x.ParentChurchBodyId,
                        principalTable: "MSTRChurchBody",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MSTRAppUtilityNVP",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChurchBodyId = table.Column<int>(nullable: true),
                    NVPTag = table.Column<string>(maxLength: 30, nullable: true),
                    NVPCode = table.Column<string>(maxLength: 15, nullable: true),
                    Acronym = table.Column<string>(maxLength: 10, nullable: true),
                    NVPStatus = table.Column<string>(maxLength: 1, nullable: true),
                    NVPValue = table.Column<string>(maxLength: 100, nullable: true),
                    NVP_CategoryId = table.Column<int>(nullable: true),
                    OrderIndex = table.Column<int>(nullable: true),
                    RequireUserCustom = table.Column<bool>(nullable: true),
                    Created = table.Column<DateTime>(nullable: true),
                    LastMod = table.Column<DateTime>(nullable: true),
                    CreatedByUserId = table.Column<int>(nullable: true),
                    LastModByUserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MSTRAppUtilityNVP", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MSTRAppUtilityNVP_MSTRChurchBody_ChurchBodyId",
                        column: x => x.ChurchBodyId,
                        principalTable: "MSTRChurchBody",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MSTRAppUtilityNVP_MSTRAppUtilityNVP_NVP_CategoryId",
                        column: x => x.NVP_CategoryId,
                        principalTable: "MSTRAppUtilityNVP",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MSTRContactInfo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppGlobalOwnerId = table.Column<int>(nullable: true),
                    ChurchBodyId = table.Column<int>(nullable: true),
                    RefUserId = table.Column<int>(nullable: true),
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
                    table.PrimaryKey("PK_MSTRContactInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MSTRContactInfo_MSTRChurchBody_ChurchBodyId",
                        column: x => x.ChurchBodyId,
                        principalTable: "MSTRChurchBody",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MSTRContactInfo_MSTRCountry_CountryId",
                        column: x => x.CountryId,
                        principalTable: "MSTRCountry",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MSTRContactInfo_MSTRCountryRegion_RegionId",
                        column: x => x.RegionId,
                        principalTable: "MSTRCountryRegion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserGroup",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppGlobalOwnerId = table.Column<int>(nullable: true),
                    ChurchBodyId = table.Column<int>(nullable: true),
                    GroupName = table.Column<string>(maxLength: 50, nullable: true),
                    GroupDesc = table.Column<string>(maxLength: 200, nullable: true),
                    UserGroupCategoryId = table.Column<int>(nullable: true),
                    Status = table.Column<string>(maxLength: 1, nullable: true),
                    Created = table.Column<DateTime>(nullable: true),
                    LastMod = table.Column<DateTime>(nullable: true),
                    CreatedByUserId = table.Column<int>(nullable: true),
                    LastModByUserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserGroup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserGroup_MSTRAppGlobalOwner_AppGlobalOwnerId",
                        column: x => x.AppGlobalOwnerId,
                        principalTable: "MSTRAppGlobalOwner",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserGroup_MSTRChurchBody_ChurchBodyId",
                        column: x => x.ChurchBodyId,
                        principalTable: "MSTRChurchBody",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserGroup_UserGroup_UserGroupCategoryId",
                        column: x => x.UserGroupCategoryId,
                        principalTable: "UserGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserRole",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppGlobalOwnerId = table.Column<int>(nullable: true),
                    ChurchBodyId = table.Column<int>(nullable: true),
                    RoleType = table.Column<string>(maxLength: 8, nullable: true),
                    RoleDesc = table.Column<string>(maxLength: 50, nullable: true),
                    RoleStatus = table.Column<string>(maxLength: 1, nullable: true),
                    RoleLevel = table.Column<int>(nullable: false),
                    RoleName = table.Column<string>(maxLength: 30, nullable: true),
                    Created = table.Column<DateTime>(nullable: true),
                    LastMod = table.Column<DateTime>(nullable: true),
                    CreatedByUserId = table.Column<int>(nullable: true),
                    LastModByUserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRole", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRole_MSTRAppGlobalOwner_AppGlobalOwnerId",
                        column: x => x.AppGlobalOwnerId,
                        principalTable: "MSTRAppGlobalOwner",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserRole_MSTRChurchBody_ChurchBodyId",
                        column: x => x.ChurchBodyId,
                        principalTable: "MSTRChurchBody",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserProfile",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppGlobalOwnerId = table.Column<int>(nullable: true),
                    ChurchBodyId = table.Column<int>(nullable: true),
                    OwnerId = table.Column<int>(nullable: true),
                    UserDesc = table.Column<string>(maxLength: 50, nullable: true),
                    Username = table.Column<string>(maxLength: 50, nullable: false),
                    Email = table.Column<string>(maxLength: 50, nullable: true),
                    UserKey = table.Column<string>(maxLength: 256, nullable: true),
                    Pwd = table.Column<string>(maxLength: 256, nullable: true),
                    PwdSecurityQue = table.Column<string>(maxLength: 30, nullable: true),
                    PwdSecurityAns = table.Column<string>(maxLength: 256, nullable: true),
                    PwdExpr = table.Column<DateTime>(nullable: true),
                    ResetPwdOnNextLogOn = table.Column<bool>(nullable: false),
                    STRT = table.Column<DateTime>(nullable: true),
                    EXPR = table.Column<DateTime>(nullable: true),
                    UserStatus = table.Column<string>(maxLength: 1, nullable: true),
                    UserId = table.Column<string>(nullable: true),
                    ChurchMemberId = table.Column<int>(nullable: true),
                    UserPhoto = table.Column<string>(nullable: true),
                    UserScope = table.Column<string>(maxLength: 1, nullable: true),
                    ContactInfoId = table.Column<int>(nullable: true),
                    ProfileScope = table.Column<string>(maxLength: 1, nullable: true),
                    Created = table.Column<DateTime>(nullable: true),
                    LastMod = table.Column<DateTime>(nullable: true),
                    CreatedByUserId = table.Column<int>(nullable: true),
                    LastModByUserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserProfile_MSTRAppGlobalOwner_AppGlobalOwnerId",
                        column: x => x.AppGlobalOwnerId,
                        principalTable: "MSTRAppGlobalOwner",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserProfile_MSTRChurchBody_ChurchBodyId",
                        column: x => x.ChurchBodyId,
                        principalTable: "MSTRChurchBody",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserProfile_MSTRContactInfo_ContactInfoId",
                        column: x => x.ContactInfoId,
                        principalTable: "MSTRContactInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserAuditTrail",
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
                    table.PrimaryKey("PK_UserAuditTrail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserAuditTrail_MSTRAppGlobalOwner_AppGlobalOwnerId",
                        column: x => x.AppGlobalOwnerId,
                        principalTable: "MSTRAppGlobalOwner",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserAuditTrail_MSTRChurchBody_ChurchBodyId",
                        column: x => x.ChurchBodyId,
                        principalTable: "MSTRChurchBody",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserAuditTrail_UserProfile_UserProfileId",
                        column: x => x.UserProfileId,
                        principalTable: "UserProfile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserProfileGroup",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppGlobalOwnerId = table.Column<int>(nullable: true),
                    ChurchBodyId = table.Column<int>(nullable: true),
                    UserGroupId = table.Column<int>(nullable: false),
                    UserProfileId = table.Column<int>(nullable: false),
                    Status = table.Column<string>(maxLength: 1, nullable: true),
                    STRT = table.Column<DateTime>(nullable: true),
                    EXPR = table.Column<DateTime>(nullable: true),
                    Created = table.Column<DateTime>(nullable: true),
                    LastMod = table.Column<DateTime>(nullable: true),
                    CreatedByUserId = table.Column<int>(nullable: true),
                    LastModByUserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfileGroup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserProfileGroup_MSTRAppGlobalOwner_AppGlobalOwnerId",
                        column: x => x.AppGlobalOwnerId,
                        principalTable: "MSTRAppGlobalOwner",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserProfileGroup_MSTRChurchBody_ChurchBodyId",
                        column: x => x.ChurchBodyId,
                        principalTable: "MSTRChurchBody",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserProfileGroup_UserGroup_UserGroupId",
                        column: x => x.UserGroupId,
                        principalTable: "UserGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserProfileGroup_UserProfile_UserProfileId",
                        column: x => x.UserProfileId,
                        principalTable: "UserProfile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserProfileRole",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppGlobalOwnerId = table.Column<int>(nullable: true),
                    UserProfileId = table.Column<int>(nullable: false),
                    UserRoleId = table.Column<int>(nullable: false),
                    ProfileRoleStatus = table.Column<string>(maxLength: 1, nullable: true),
                    STRT = table.Column<DateTime>(nullable: true),
                    EXPR = table.Column<DateTime>(nullable: true),
                    ChurchBodyId = table.Column<int>(nullable: true),
                    Created = table.Column<DateTime>(nullable: true),
                    LastMod = table.Column<DateTime>(nullable: true),
                    CreatedByUserId = table.Column<int>(nullable: true),
                    LastModByUserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfileRole", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserProfileRole_MSTRAppGlobalOwner_AppGlobalOwnerId",
                        column: x => x.AppGlobalOwnerId,
                        principalTable: "MSTRAppGlobalOwner",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserProfileRole_MSTRChurchBody_ChurchBodyId",
                        column: x => x.ChurchBodyId,
                        principalTable: "MSTRChurchBody",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserProfileRole_UserProfile_UserProfileId",
                        column: x => x.UserProfileId,
                        principalTable: "UserProfile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserProfileRole_UserRole_UserRoleId",
                        column: x => x.UserRoleId,
                        principalTable: "UserRole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserGroupPermission",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppGlobalOwnerId = table.Column<int>(nullable: true),
                    ChurchBodyId = table.Column<int>(nullable: true),
                    UserGroupId = table.Column<int>(nullable: false),
                    UserPermissionId = table.Column<int>(nullable: false),
                    Status = table.Column<string>(maxLength: 1, nullable: true),
                    STRT = table.Column<DateTime>(nullable: true),
                    EXPR = table.Column<DateTime>(nullable: true),
                    ViewPerm = table.Column<bool>(nullable: false),
                    CreatePerm = table.Column<bool>(nullable: false),
                    EditPerm = table.Column<bool>(nullable: false),
                    DeletePerm = table.Column<bool>(nullable: false),
                    ManagePerm = table.Column<bool>(nullable: false),
                    Created = table.Column<DateTime>(nullable: true),
                    LastMod = table.Column<DateTime>(nullable: true),
                    CreatedByUserId = table.Column<int>(nullable: true),
                    LastModByUserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserGroupPermission", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserGroupPermission_MSTRAppGlobalOwner_AppGlobalOwnerId",
                        column: x => x.AppGlobalOwnerId,
                        principalTable: "MSTRAppGlobalOwner",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserGroupPermission_MSTRChurchBody_ChurchBodyId",
                        column: x => x.ChurchBodyId,
                        principalTable: "MSTRChurchBody",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserGroupPermission_UserGroup_UserGroupId",
                        column: x => x.UserGroupId,
                        principalTable: "UserGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRolePermission",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppGlobalOwnerId = table.Column<int>(nullable: true),
                    ChurchBodyId = table.Column<int>(nullable: true),
                    UserRoleId = table.Column<int>(nullable: false),
                    UserPermissionId = table.Column<int>(nullable: false),
                    Status = table.Column<string>(maxLength: 1, nullable: true),
                    ViewPerm = table.Column<bool>(nullable: false),
                    CreatePerm = table.Column<bool>(nullable: false),
                    EditPerm = table.Column<bool>(nullable: false),
                    DeletePerm = table.Column<bool>(nullable: false),
                    ManagePerm = table.Column<bool>(nullable: false),
                    Created = table.Column<DateTime>(nullable: true),
                    LastMod = table.Column<DateTime>(nullable: true),
                    CreatedByUserId = table.Column<int>(nullable: true),
                    LastModByUserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRolePermission", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRolePermission_MSTRAppGlobalOwner_AppGlobalOwnerId",
                        column: x => x.AppGlobalOwnerId,
                        principalTable: "MSTRAppGlobalOwner",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserRolePermission_MSTRChurchBody_ChurchBodyId",
                        column: x => x.ChurchBodyId,
                        principalTable: "MSTRChurchBody",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserRolePermission_UserRole_UserRoleId",
                        column: x => x.UserRoleId,
                        principalTable: "UserRole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserPermission",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserPermCategoryId = table.Column<int>(nullable: true),
                    PermissionCode = table.Column<string>(maxLength: 10, nullable: true),
                    PermissionName = table.Column<string>(maxLength: 100, nullable: true),
                    PermStatus = table.Column<string>(maxLength: 1, nullable: true),
                    Created = table.Column<DateTime>(nullable: true),
                    LastMod = table.Column<DateTime>(nullable: true),
                    CreatedByUserId = table.Column<int>(nullable: true),
                    LastModByUserId = table.Column<int>(nullable: true),
                    UserRolePermissionId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPermission", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserPermission_UserPermission_UserPermCategoryId",
                        column: x => x.UserPermCategoryId,
                        principalTable: "UserPermission",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserPermission_UserRolePermission_UserRolePermissionId",
                        column: x => x.UserRolePermissionId,
                        principalTable: "UserRolePermission",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppSubscription_AppGlobalOwnerId",
                table: "AppSubscription",
                column: "AppGlobalOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_AppSubscription_AppSubscriptionPackageId",
                table: "AppSubscription",
                column: "AppSubscriptionPackageId");

            migrationBuilder.CreateIndex(
                name: "IX_AppSubscription_ChurchBodyId",
                table: "AppSubscription",
                column: "ChurchBodyId");

            migrationBuilder.CreateIndex(
                name: "IX_ChurchFaithType_FaithTypeClassId",
                table: "ChurchFaithType",
                column: "FaithTypeClassId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientAppServerConfig_AppGlobalOwnerId",
                table: "ClientAppServerConfig",
                column: "AppGlobalOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_MSTRAppGlobalOwner_ContactInfoId",
                table: "MSTRAppGlobalOwner",
                column: "ContactInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_MSTRAppGlobalOwner_CountryId",
                table: "MSTRAppGlobalOwner",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_MSTRAppGlobalOwner_FaithTypeCategoryId",
                table: "MSTRAppGlobalOwner",
                column: "FaithTypeCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_MSTRAppUtilityNVP_ChurchBodyId",
                table: "MSTRAppUtilityNVP",
                column: "ChurchBodyId");

            migrationBuilder.CreateIndex(
                name: "IX_MSTRAppUtilityNVP_NVP_CategoryId",
                table: "MSTRAppUtilityNVP",
                column: "NVP_CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_MSTRChurchBody_AppGlobalOwnerId",
                table: "MSTRChurchBody",
                column: "AppGlobalOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_MSTRChurchBody_ChurchLevelId",
                table: "MSTRChurchBody",
                column: "ChurchLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_MSTRChurchBody_ContactInfoId",
                table: "MSTRChurchBody",
                column: "ContactInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_MSTRChurchBody_CountryId",
                table: "MSTRChurchBody",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_MSTRChurchBody_CountryRegionId",
                table: "MSTRChurchBody",
                column: "CountryRegionId");

            migrationBuilder.CreateIndex(
                name: "IX_MSTRChurchBody_ParentChurchBodyId",
                table: "MSTRChurchBody",
                column: "ParentChurchBodyId");

            migrationBuilder.CreateIndex(
                name: "IX_MSTRContactInfo_ChurchBodyId",
                table: "MSTRContactInfo",
                column: "ChurchBodyId");

            migrationBuilder.CreateIndex(
                name: "IX_MSTRContactInfo_CountryId",
                table: "MSTRContactInfo",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_MSTRContactInfo_RegionId",
                table: "MSTRContactInfo",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_MSTRCountryRegion_CountryId",
                table: "MSTRCountryRegion",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_SubscriptionChurchBody_ChurchBodyId",
                table: "SubscriptionChurchBody",
                column: "ChurchBodyId");

            migrationBuilder.CreateIndex(
                name: "IX_SubscriptionChurchBody_GlobalSubscriptionId",
                table: "SubscriptionChurchBody",
                column: "GlobalSubscriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAuditTrail_AppGlobalOwnerId",
                table: "UserAuditTrail",
                column: "AppGlobalOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAuditTrail_ChurchBodyId",
                table: "UserAuditTrail",
                column: "ChurchBodyId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAuditTrail_UserProfileId",
                table: "UserAuditTrail",
                column: "UserProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_UserGroup_AppGlobalOwnerId",
                table: "UserGroup",
                column: "AppGlobalOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_UserGroup_ChurchBodyId",
                table: "UserGroup",
                column: "ChurchBodyId");

            migrationBuilder.CreateIndex(
                name: "IX_UserGroup_UserGroupCategoryId",
                table: "UserGroup",
                column: "UserGroupCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_UserGroupPermission_AppGlobalOwnerId",
                table: "UserGroupPermission",
                column: "AppGlobalOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_UserGroupPermission_ChurchBodyId",
                table: "UserGroupPermission",
                column: "ChurchBodyId");

            migrationBuilder.CreateIndex(
                name: "IX_UserGroupPermission_UserGroupId",
                table: "UserGroupPermission",
                column: "UserGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_UserGroupPermission_UserPermissionId",
                table: "UserGroupPermission",
                column: "UserPermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPermission_UserPermCategoryId",
                table: "UserPermission",
                column: "UserPermCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPermission_UserRolePermissionId",
                table: "UserPermission",
                column: "UserRolePermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfile_AppGlobalOwnerId",
                table: "UserProfile",
                column: "AppGlobalOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfile_ChurchBodyId",
                table: "UserProfile",
                column: "ChurchBodyId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfile_ContactInfoId",
                table: "UserProfile",
                column: "ContactInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfileGroup_AppGlobalOwnerId",
                table: "UserProfileGroup",
                column: "AppGlobalOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfileGroup_ChurchBodyId",
                table: "UserProfileGroup",
                column: "ChurchBodyId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfileGroup_UserGroupId",
                table: "UserProfileGroup",
                column: "UserGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfileGroup_UserProfileId",
                table: "UserProfileGroup",
                column: "UserProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfileRole_AppGlobalOwnerId",
                table: "UserProfileRole",
                column: "AppGlobalOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfileRole_ChurchBodyId",
                table: "UserProfileRole",
                column: "ChurchBodyId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfileRole_UserProfileId",
                table: "UserProfileRole",
                column: "UserProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfileRole_UserRoleId",
                table: "UserProfileRole",
                column: "UserRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_AppGlobalOwnerId",
                table: "UserRole",
                column: "AppGlobalOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_ChurchBodyId",
                table: "UserRole",
                column: "ChurchBodyId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRolePermission_AppGlobalOwnerId",
                table: "UserRolePermission",
                column: "AppGlobalOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRolePermission_ChurchBodyId",
                table: "UserRolePermission",
                column: "ChurchBodyId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRolePermission_UserPermissionId",
                table: "UserRolePermission",
                column: "UserPermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRolePermission_UserRoleId",
                table: "UserRolePermission",
                column: "UserRoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_SubscriptionChurchBody_MSTRChurchBody_ChurchBodyId",
                table: "SubscriptionChurchBody",
                column: "ChurchBodyId",
                principalTable: "MSTRChurchBody",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SubscriptionChurchBody_AppSubscription_GlobalSubscriptionId",
                table: "SubscriptionChurchBody",
                column: "GlobalSubscriptionId",
                principalTable: "AppSubscription",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppSubscription_MSTRAppGlobalOwner_AppGlobalOwnerId",
                table: "AppSubscription",
                column: "AppGlobalOwnerId",
                principalTable: "MSTRAppGlobalOwner",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AppSubscription_MSTRChurchBody_ChurchBodyId",
                table: "AppSubscription",
                column: "ChurchBodyId",
                principalTable: "MSTRChurchBody",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MSTRAppGlobalOwner_MSTRContactInfo_ContactInfoId",
                table: "MSTRAppGlobalOwner",
                column: "ContactInfoId",
                principalTable: "MSTRContactInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MSTRChurchBody_MSTRContactInfo_ContactInfoId",
                table: "MSTRChurchBody",
                column: "ContactInfoId",
                principalTable: "MSTRContactInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserGroupPermission_UserPermission_UserPermissionId",
                table: "UserGroupPermission",
                column: "UserPermissionId",
                principalTable: "UserPermission",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRolePermission_UserPermission_UserPermissionId",
                table: "UserRolePermission",
                column: "UserPermissionId",
                principalTable: "UserPermission",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MSTRChurchBody_MSTRAppGlobalOwner_AppGlobalOwnerId",
                table: "MSTRChurchBody");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRole_MSTRAppGlobalOwner_AppGlobalOwnerId",
                table: "UserRole");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRolePermission_MSTRAppGlobalOwner_AppGlobalOwnerId",
                table: "UserRolePermission");

            migrationBuilder.DropForeignKey(
                name: "FK_MSTRContactInfo_MSTRChurchBody_ChurchBodyId",
                table: "MSTRContactInfo");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRole_MSTRChurchBody_ChurchBodyId",
                table: "UserRole");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRolePermission_MSTRChurchBody_ChurchBodyId",
                table: "UserRolePermission");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRolePermission_UserPermission_UserPermissionId",
                table: "UserRolePermission");

            migrationBuilder.DropTable(
                name: "ClientAppServerConfig");

            migrationBuilder.DropTable(
                name: "MSTRAppUtilityNVP");

            migrationBuilder.DropTable(
                name: "SubscriptionChurchBody");

            migrationBuilder.DropTable(
                name: "UserAuditTrail");

            migrationBuilder.DropTable(
                name: "UserGroupPermission");

            migrationBuilder.DropTable(
                name: "UserProfileGroup");

            migrationBuilder.DropTable(
                name: "UserProfileRole");

            migrationBuilder.DropTable(
                name: "AppSubscription");

            migrationBuilder.DropTable(
                name: "UserGroup");

            migrationBuilder.DropTable(
                name: "UserProfile");

            migrationBuilder.DropTable(
                name: "AppSubscriptionPackage");

            migrationBuilder.DropTable(
                name: "MSTRAppGlobalOwner");

            migrationBuilder.DropTable(
                name: "ChurchFaithType");

            migrationBuilder.DropTable(
                name: "MSTRChurchBody");

            migrationBuilder.DropTable(
                name: "MSTRChurchLevel");

            migrationBuilder.DropTable(
                name: "MSTRContactInfo");

            migrationBuilder.DropTable(
                name: "MSTRCountryRegion");

            migrationBuilder.DropTable(
                name: "MSTRCountry");

            migrationBuilder.DropTable(
                name: "UserPermission");

            migrationBuilder.DropTable(
                name: "UserRolePermission");

            migrationBuilder.DropTable(
                name: "UserRole");
        }
    }
}
