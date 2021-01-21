using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RhemaCMS.Migrations.ChurchModel
{
    public partial class ef_c_upd1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ActivityPeriod",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChurchBodyId = table.Column<int>(nullable: false),
                    From = table.Column<DateTime>(nullable: true),
                    To = table.Column<DateTime>(nullable: true),
                    Status = table.Column<string>(maxLength: 1, nullable: true),
                    PeriodDesc = table.Column<string>(maxLength: 50, nullable: true),
                    OwnedByChurchBodyId = table.Column<int>(nullable: false),
                    LengthInDays = table.Column<int>(nullable: false),
                    PeriodType = table.Column<string>(maxLength: 2, nullable: true),
                    SharingStatus = table.Column<string>(maxLength: 1, nullable: true),
                    Created = table.Column<DateTime>(nullable: true),
                    LastMod = table.Column<DateTime>(nullable: true),
                    CreatedByUserId = table.Column<int>(nullable: true),
                    LastModByUserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityPeriod", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ChurchBodyService",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppGlobalOwnerId = table.Column<int>(nullable: true),
                    ChurchBodyId = table.Column<int>(nullable: true),
                    OwnedByChurchBodyId = table.Column<int>(nullable: true),
                    ServiceName = table.Column<string>(maxLength: 50, nullable: false),
                    IsTimed = table.Column<bool>(nullable: false),
                    ServiceStart = table.Column<DateTime>(nullable: true),
                    ServiceEnd = table.Column<DateTime>(nullable: true),
                    MeetingDay = table.Column<string>(maxLength: 2, nullable: true),
                    Frequency = table.Column<string>(maxLength: 2, nullable: true),
                    ServiceType = table.Column<string>(maxLength: 1, nullable: true),
                    ServiceCategoryId = table.Column<int>(nullable: true),
                    OrderIndex = table.Column<int>(nullable: false),
                    MaxPersCapacity = table.Column<long>(nullable: false),
                    SharingStatus = table.Column<string>(maxLength: 1, nullable: true),
                    Created = table.Column<DateTime>(nullable: true),
                    LastMod = table.Column<DateTime>(nullable: true),
                    CreatedByUserId = table.Column<int>(nullable: true),
                    LastModByUserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChurchBodyService", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChurchBodyService_AppGlobalOwner_AppGlobalOwnerId",
                        column: x => x.AppGlobalOwnerId,
                        principalTable: "AppGlobalOwner",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChurchBodyService_ChurchBody_ChurchBodyId",
                        column: x => x.ChurchBodyId,
                        principalTable: "ChurchBody",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChurchBodyService_ChurchBodyService_ServiceCategoryId",
                        column: x => x.ServiceCategoryId,
                        principalTable: "ChurchBodyService",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ChurchEventCategory",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppGlobalOwnerId = table.Column<int>(nullable: true),
                    ChurchBodyId = table.Column<int>(nullable: true),
                    OwnedByChurchBodyId = table.Column<int>(nullable: true),
                    EventName = table.Column<string>(maxLength: 50, nullable: false),
                    Description = table.Column<string>(maxLength: 200, nullable: true),
                    SharingStatus = table.Column<string>(maxLength: 1, nullable: true),
                    Created = table.Column<DateTime>(nullable: true),
                    LastMod = table.Column<DateTime>(nullable: true),
                    CreatedByUserId = table.Column<int>(nullable: true),
                    LastModByUserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChurchEventCategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChurchEventCategory_AppGlobalOwner_AppGlobalOwnerId",
                        column: x => x.AppGlobalOwnerId,
                        principalTable: "AppGlobalOwner",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChurchEventCategory_ChurchBody_ChurchBodyId",
                        column: x => x.ChurchBodyId,
                        principalTable: "ChurchBody",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ChurchLifeActivity",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppGlobalOwnerId = table.Column<int>(nullable: true),
                    ChurchBodyId = table.Column<int>(nullable: true),
                    OwnedByChurchBodyId = table.Column<int>(nullable: true),
                    Description = table.Column<string>(maxLength: 50, nullable: false),
                    ActivityType = table.Column<string>(maxLength: 2, nullable: true),
                    Tag = table.Column<string>(maxLength: 50, nullable: true),
                    ShortCode = table.Column<string>(maxLength: 10, nullable: false),
                    IsMainline = table.Column<bool>(nullable: false),
                    IsService = table.Column<bool>(nullable: false),
                    SharingStatus = table.Column<string>(maxLength: 1, nullable: true),
                    Created = table.Column<DateTime>(nullable: true),
                    LastMod = table.Column<DateTime>(nullable: true),
                    CreatedByUserId = table.Column<int>(nullable: true),
                    LastModByUserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChurchLifeActivity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChurchLifeActivity_AppGlobalOwner_AppGlobalOwnerId",
                        column: x => x.AppGlobalOwnerId,
                        principalTable: "AppGlobalOwner",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChurchLifeActivity_ChurchBody_ChurchBodyId",
                        column: x => x.ChurchBodyId,
                        principalTable: "ChurchBody",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ChurchMemStatus",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppGlobalOwnerId = table.Column<int>(nullable: true),
                    ChurchBodyId = table.Column<int>(nullable: true),
                    OwnedByChurchBodyId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(maxLength: 15, nullable: true),
                    Description = table.Column<string>(maxLength: 50, nullable: true),
                    Abbrev = table.Column<string>(maxLength: 4, nullable: true),
                    Deceased = table.Column<bool>(nullable: false),
                    Available = table.Column<bool>(nullable: false),
                    OrderIndex = table.Column<int>(nullable: true),
                    SharingStatus = table.Column<string>(maxLength: 1, nullable: true),
                    Created = table.Column<DateTime>(nullable: true),
                    LastMod = table.Column<DateTime>(nullable: true),
                    CreatedByUserId = table.Column<int>(nullable: true),
                    LastModByUserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChurchMemStatus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChurchMemStatus_AppGlobalOwner_AppGlobalOwnerId",
                        column: x => x.AppGlobalOwnerId,
                        principalTable: "AppGlobalOwner",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChurchMemStatus_ChurchBody_ChurchBodyId",
                        column: x => x.ChurchBodyId,
                        principalTable: "ChurchBody",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ChurchMemType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppGlobalOwnerId = table.Column<int>(nullable: false),
                    ChurchBodyId = table.Column<int>(nullable: false),
                    OwnedByChurchBodyId = table.Column<int>(nullable: false),
                    Description = table.Column<string>(maxLength: 50, nullable: false),
                    ApplyToClergyOnly = table.Column<bool>(nullable: false),
                    SharingStatus = table.Column<string>(maxLength: 1, nullable: true),
                    Created = table.Column<DateTime>(nullable: true),
                    LastMod = table.Column<DateTime>(nullable: true),
                    CreatedByUserId = table.Column<int>(nullable: true),
                    LastModByUserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChurchMemType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChurchMemType_AppGlobalOwner_AppGlobalOwnerId",
                        column: x => x.AppGlobalOwnerId,
                        principalTable: "AppGlobalOwner",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChurchMemType_ChurchBody_ChurchBodyId",
                        column: x => x.ChurchBodyId,
                        principalTable: "ChurchBody",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChurchRank",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppGlobalOwnerId = table.Column<int>(nullable: true),
                    ChurchBodyId = table.Column<int>(nullable: true),
                    OwnedByChurchBodyId = table.Column<int>(nullable: true),
                    RankName = table.Column<string>(maxLength: 50, nullable: false),
                    Description = table.Column<string>(maxLength: 100, nullable: true),
                    ApplyToClergyOnly = table.Column<bool>(nullable: false),
                    RankIndex = table.Column<int>(nullable: false),
                    SharingStatus = table.Column<string>(maxLength: 1, nullable: true),
                    Created = table.Column<DateTime>(nullable: true),
                    LastMod = table.Column<DateTime>(nullable: true),
                    CreatedByUserId = table.Column<int>(nullable: true),
                    LastModByUserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChurchRank", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChurchRank_AppGlobalOwner_AppGlobalOwnerId",
                        column: x => x.AppGlobalOwnerId,
                        principalTable: "AppGlobalOwner",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChurchRank_ChurchBody_ChurchBodyId",
                        column: x => x.ChurchBodyId,
                        principalTable: "ChurchBody",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ChurchRole",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppGlobalOwnerId = table.Column<int>(nullable: true),
                    OwnedByChurchBodyId = table.Column<int>(nullable: true),
                    ParentChurchBodyId = table.Column<int>(nullable: true),
                    TargetChurchLevelId = table.Column<int>(nullable: true),
                    OrganisationType = table.Column<string>(maxLength: 2, nullable: true),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    Description = table.Column<string>(maxLength: 200, nullable: true),
                    GlobalChurchCode = table.Column<string>(maxLength: 20, nullable: true),
                    RootChurchCode = table.Column<string>(maxLength: 200, nullable: true),
                    RankIndex = table.Column<int>(nullable: true),
                    ApplyToClergyOnly = table.Column<bool>(nullable: false),
                    MaxNumAllowed = table.Column<int>(nullable: true),
                    MinNumAllowed = table.Column<int>(nullable: true),
                    PrimaryFunction = table.Column<string>(maxLength: 200, nullable: true),
                    Comments = table.Column<string>(maxLength: 100, nullable: true),
                    IsActivated = table.Column<bool>(nullable: true),
                    SharingStatus = table.Column<string>(maxLength: 1, nullable: true),
                    ChurchWorkStatus = table.Column<string>(maxLength: 1, nullable: true),
                    OwnershipStatus = table.Column<string>(maxLength: 1, nullable: true),
                    Created = table.Column<DateTime>(nullable: true),
                    LastMod = table.Column<DateTime>(nullable: true),
                    CreatedByUserId = table.Column<int>(nullable: true),
                    LastModByUserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChurchRole", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChurchRole_AppGlobalOwner_AppGlobalOwnerId",
                        column: x => x.AppGlobalOwnerId,
                        principalTable: "AppGlobalOwner",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChurchRole_ChurchBody_OwnedByChurchBodyId",
                        column: x => x.OwnedByChurchBodyId,
                        principalTable: "ChurchBody",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChurchRole_ChurchBody_ParentChurchBodyId",
                        column: x => x.ParentChurchBodyId,
                        principalTable: "ChurchBody",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChurchRole_ChurchLevel_TargetChurchLevelId",
                        column: x => x.TargetChurchLevelId,
                        principalTable: "ChurchLevel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ChurchSectorUnit",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppGlobalOwnerId = table.Column<int>(nullable: true),
                    OwnedByChurchBodyId = table.Column<int>(nullable: true),
                    ParentChurchBodyId = table.Column<int>(nullable: true),
                    TargetChurchLevelId = table.Column<int>(nullable: true),
                    OrganisationType = table.Column<string>(maxLength: 2, nullable: true),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    Description = table.Column<string>(maxLength: 200, nullable: true),
                    GlobalChurchCode = table.Column<string>(maxLength: 20, nullable: true),
                    RootChurchCode = table.Column<string>(maxLength: 200, nullable: true),
                    ContactInfoId = table.Column<int>(nullable: true),
                    Formed = table.Column<DateTime>(nullable: true),
                    Innaug = table.Column<DateTime>(nullable: true),
                    BriefHistory = table.Column<string>(maxLength: 500, nullable: true),
                    OrderIndex = table.Column<double>(nullable: true),
                    GenderStatus = table.Column<string>(maxLength: 1, nullable: true),
                    UnitLogo = table.Column<string>(nullable: true),
                    IsUnitGenerational = table.Column<bool>(nullable: true),
                    UnitMaxAge = table.Column<int>(nullable: true),
                    UnitMinAge = table.Column<int>(nullable: true),
                    PrimaryFunction = table.Column<string>(maxLength: 200, nullable: true),
                    Comments = table.Column<string>(maxLength: 100, nullable: true),
                    IsActivated = table.Column<bool>(nullable: true),
                    SharingStatus = table.Column<string>(maxLength: 1, nullable: true),
                    ChurchWorkStatus = table.Column<string>(maxLength: 1, nullable: true),
                    OwnershipStatus = table.Column<string>(maxLength: 1, nullable: true),
                    Created = table.Column<DateTime>(nullable: true),
                    LastMod = table.Column<DateTime>(nullable: true),
                    CreatedByUserId = table.Column<int>(nullable: true),
                    LastModByUserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChurchSectorUnit", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChurchSectorUnit_AppGlobalOwner_AppGlobalOwnerId",
                        column: x => x.AppGlobalOwnerId,
                        principalTable: "AppGlobalOwner",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChurchSectorUnit_ContactInfo_ContactInfoId",
                        column: x => x.ContactInfoId,
                        principalTable: "ContactInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChurchSectorUnit_ChurchBody_OwnedByChurchBodyId",
                        column: x => x.OwnedByChurchBodyId,
                        principalTable: "ChurchBody",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChurchSectorUnit_ChurchBody_ParentChurchBodyId",
                        column: x => x.ParentChurchBodyId,
                        principalTable: "ChurchBody",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChurchSectorUnit_ChurchLevel_TargetChurchLevelId",
                        column: x => x.TargetChurchLevelId,
                        principalTable: "ChurchLevel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ChurchVisitorType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppGlobalOwnerId = table.Column<int>(nullable: true),
                    ChurchBodyId = table.Column<int>(nullable: true),
                    OwnedByChurchBodyId = table.Column<int>(nullable: true),
                    Description = table.Column<string>(maxLength: 50, nullable: false),
                    ApplyToClergyOnly = table.Column<bool>(nullable: false),
                    SharingStatus = table.Column<string>(maxLength: 1, nullable: true),
                    OrderIndex = table.Column<int>(nullable: true),
                    Created = table.Column<DateTime>(nullable: true),
                    LastMod = table.Column<DateTime>(nullable: true),
                    CreatedByUserId = table.Column<int>(nullable: true),
                    LastModByUserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChurchVisitorType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChurchVisitorType_AppGlobalOwner_AppGlobalOwnerId",
                        column: x => x.AppGlobalOwnerId,
                        principalTable: "AppGlobalOwner",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChurchVisitorType_ChurchBody_ChurchBodyId",
                        column: x => x.ChurchBodyId,
                        principalTable: "ChurchBody",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LeaderRoleCategory",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChurchBodyId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 30, nullable: false),
                    Description = table.Column<string>(maxLength: 100, nullable: true),
                    OwnedByChurchBodyId = table.Column<int>(nullable: true),
                    IsCoreRole = table.Column<bool>(nullable: false),
                    EventRole = table.Column<bool>(nullable: false),
                    IsLayRole = table.Column<bool>(nullable: false),
                    IsOrdainedRole = table.Column<bool>(nullable: false),
                    Status = table.Column<string>(maxLength: 1, nullable: true),
                    SharingStatus = table.Column<string>(maxLength: 1, nullable: true),
                    IsChurchLeaderRole = table.Column<bool>(nullable: false),
                    IsChurchWorkerRole = table.Column<bool>(nullable: false),
                    IsClergyRole = table.Column<bool>(nullable: false),
                    Created = table.Column<DateTime>(nullable: true),
                    LastMod = table.Column<DateTime>(nullable: true),
                    CreatedByUserId = table.Column<int>(nullable: true),
                    LastModByUserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaderRoleCategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LeaderRoleCategory_ChurchBody_ChurchBodyId",
                        column: x => x.ChurchBodyId,
                        principalTable: "ChurchBody",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LeaderRoleCategory_ChurchBody_OwnedByChurchBodyId",
                        column: x => x.OwnedByChurchBodyId,
                        principalTable: "ChurchBody",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MemberContact",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppGlobalOwnerId = table.Column<int>(nullable: true),
                    ChurchBodyId = table.Column<int>(nullable: true),
                    OwnedByChurchBodyId = table.Column<int>(nullable: true),
                    ChurchMemberId = table.Column<int>(nullable: true),
                    IsCurrentContact = table.Column<bool>(nullable: false),
                    IsChurchFellow = table.Column<bool>(nullable: false),
                    InternalContactId = table.Column<int>(nullable: true),
                    RelationshipId = table.Column<int>(nullable: true),
                    ExtConContactName = table.Column<string>(maxLength: 100, nullable: true),
                    CityExtCon = table.Column<string>(maxLength: 50, nullable: true),
                    ExtConCountryId = table.Column<int>(nullable: true),
                    ExtConRegionId = table.Column<int>(nullable: true),
                    ExtConDenomination = table.Column<string>(maxLength: 100, nullable: true),
                    ExtConFaithCategory = table.Column<string>(maxLength: 50, nullable: true),
                    ExtConDigitalAddress = table.Column<string>(maxLength: 50, nullable: true),
                    ExtConLocation = table.Column<string>(maxLength: 50, nullable: true),
                    ExtConMobilePhone = table.Column<string>(maxLength: 15, nullable: true),
                    ExtConEmail = table.Column<string>(maxLength: 50, nullable: true),
                    ExtConResidenceAddress = table.Column<string>(maxLength: 100, nullable: true),
                    ExtConResAddrSameAsPostAddr = table.Column<bool>(nullable: false),
                    ExtConPostalAddress = table.Column<string>(maxLength: 100, nullable: true),
                    Created = table.Column<DateTime>(nullable: true),
                    LastMod = table.Column<DateTime>(nullable: true),
                    CreatedByUserId = table.Column<int>(nullable: true),
                    LastModByUserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberContact", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MemberContact_AppGlobalOwner_AppGlobalOwnerId",
                        column: x => x.AppGlobalOwnerId,
                        principalTable: "AppGlobalOwner",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MemberContact_ChurchBody_ChurchBodyId",
                        column: x => x.ChurchBodyId,
                        principalTable: "ChurchBody",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MemberContact_ChurchMember_ChurchMemberId",
                        column: x => x.ChurchMemberId,
                        principalTable: "ChurchMember",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MemberContact_Country_ExtConCountryId",
                        column: x => x.ExtConCountryId,
                        principalTable: "Country",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MemberContact_CountryRegion_ExtConRegionId",
                        column: x => x.ExtConRegionId,
                        principalTable: "CountryRegion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MemberContact_ChurchMember_InternalContactId",
                        column: x => x.InternalContactId,
                        principalTable: "ChurchMember",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MemberContact_RelationshipType_RelationshipId",
                        column: x => x.RelationshipId,
                        principalTable: "RelationshipType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MemberEducHistory",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppGlobalOwnerId = table.Column<int>(nullable: true),
                    ChurchBodyId = table.Column<int>(nullable: true),
                    ChurchMemberId = table.Column<int>(nullable: false),
                    InstitutionName = table.Column<string>(maxLength: 50, nullable: true),
                    InstitutionTypeId = table.Column<int>(maxLength: 50, nullable: false),
                    CertificateId = table.Column<int>(nullable: true),
                    Location = table.Column<string>(nullable: true),
                    CountryId = table.Column<int>(nullable: true),
                    IsCompleted = table.Column<bool>(nullable: true),
                    Since = table.Column<DateTime>(nullable: true),
                    Until = table.Column<DateTime>(nullable: true),
                    Discipline = table.Column<string>(maxLength: 100, nullable: true),
                    Created = table.Column<DateTime>(nullable: true),
                    LastMod = table.Column<DateTime>(nullable: true),
                    CreatedByUserId = table.Column<int>(nullable: true),
                    LastModByUserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberEducHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MemberEducHistory_AppGlobalOwner_AppGlobalOwnerId",
                        column: x => x.AppGlobalOwnerId,
                        principalTable: "AppGlobalOwner",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MemberEducHistory_CertificateType_CertificateId",
                        column: x => x.CertificateId,
                        principalTable: "CertificateType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MemberEducHistory_ChurchBody_ChurchBodyId",
                        column: x => x.ChurchBodyId,
                        principalTable: "ChurchBody",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MemberEducHistory_ChurchMember_ChurchMemberId",
                        column: x => x.ChurchMemberId,
                        principalTable: "ChurchMember",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MemberEducHistory_Country_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Country",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MemberEducHistory_InstitutionType_InstitutionTypeId",
                        column: x => x.InstitutionTypeId,
                        principalTable: "InstitutionType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MemberLanguageSpoken",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppGlobalOwnerId = table.Column<int>(nullable: true),
                    ChurchBodyId = table.Column<int>(nullable: true),
                    ChurchMemberId = table.Column<int>(nullable: false),
                    LanguageSpokenId = table.Column<int>(nullable: true),
                    IsPrimaryLanguage = table.Column<bool>(nullable: false),
                    ProficiencyLevel = table.Column<int>(nullable: true),
                    Created = table.Column<DateTime>(nullable: true),
                    LastMod = table.Column<DateTime>(nullable: true),
                    CreatedByUserId = table.Column<int>(nullable: true),
                    LastModByUserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberLanguageSpoken", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MemberLanguageSpoken_AppGlobalOwner_AppGlobalOwnerId",
                        column: x => x.AppGlobalOwnerId,
                        principalTable: "AppGlobalOwner",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MemberLanguageSpoken_ChurchBody_ChurchBodyId",
                        column: x => x.ChurchBodyId,
                        principalTable: "ChurchBody",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MemberLanguageSpoken_ChurchMember_ChurchMemberId",
                        column: x => x.ChurchMemberId,
                        principalTable: "ChurchMember",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MemberLanguageSpoken_LanguageSpoken_LanguageSpokenId",
                        column: x => x.LanguageSpokenId,
                        principalTable: "LanguageSpoken",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MemberProfessionBrand",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppGlobalOwnerId = table.Column<int>(nullable: true),
                    ChurchBodyId = table.Column<int>(nullable: true),
                    OwnedByChurchBodyId = table.Column<int>(nullable: true),
                    ChurchMemberId = table.Column<int>(nullable: false),
                    Profession = table.Column<string>(maxLength: 50, nullable: true),
                    IsActivePractice = table.Column<bool>(nullable: false),
                    Since = table.Column<DateTime>(nullable: true),
                    Until = table.Column<DateTime>(nullable: true),
                    BrandProfile = table.Column<string>(maxLength: 200, nullable: true),
                    Created = table.Column<DateTime>(nullable: true),
                    LastMod = table.Column<DateTime>(nullable: true),
                    CreatedByUserId = table.Column<int>(nullable: true),
                    LastModByUserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberProfessionBrand", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MemberProfessionBrand_AppGlobalOwner_AppGlobalOwnerId",
                        column: x => x.AppGlobalOwnerId,
                        principalTable: "AppGlobalOwner",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MemberProfessionBrand_ChurchBody_ChurchBodyId",
                        column: x => x.ChurchBodyId,
                        principalTable: "ChurchBody",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MemberProfessionBrand_ChurchMember_ChurchMemberId",
                        column: x => x.ChurchMemberId,
                        principalTable: "ChurchMember",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MemberRelation",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppGlobalOwnerId = table.Column<int>(nullable: true),
                    ChurchBodyId = table.Column<int>(nullable: true),
                    OwnedByChurchBodyId = table.Column<int>(nullable: true),
                    ChurchMemberId = table.Column<int>(nullable: false),
                    RelationType = table.Column<string>(maxLength: 1, nullable: false),
                    ChurchFellow = table.Column<string>(maxLength: 1, nullable: true),
                    RelationChurchMemberId = table.Column<int>(nullable: true),
                    ExternalNonMemAssociateId = table.Column<int>(nullable: true),
                    RelationshipId = table.Column<int>(nullable: true),
                    Status = table.Column<string>(maxLength: 1, nullable: true),
                    RegionIdExtCon = table.Column<int>(nullable: true),
                    IsNextOfKin = table.Column<bool>(nullable: false),
                    ResAddrSameAsPostAddrExtCon = table.Column<bool>(nullable: false),
                    CityExtCon = table.Column<string>(maxLength: 30, nullable: true),
                    ContactNameExtCon = table.Column<string>(maxLength: 100, nullable: true),
                    CountryIdExtCon = table.Column<int>(nullable: true),
                    DenominationExtCon = table.Column<string>(maxLength: 100, nullable: true),
                    DigitalAddressExtCon = table.Column<string>(maxLength: 30, nullable: true),
                    EmailExtCon = table.Column<string>(nullable: true),
                    FaithTypeCategoryIdExtCon = table.Column<int>(nullable: true),
                    LocationExtCon = table.Column<string>(maxLength: 30, nullable: true),
                    MobilePhoneExtCon = table.Column<string>(maxLength: 15, nullable: true),
                    PostalAddressExtCon = table.Column<string>(maxLength: 30, nullable: true),
                    ResidenceAddressExtCon = table.Column<string>(maxLength: 100, nullable: true),
                    Created = table.Column<DateTime>(nullable: true),
                    LastMod = table.Column<DateTime>(nullable: true),
                    CreatedByUserId = table.Column<int>(nullable: true),
                    LastModByUserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberRelation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MemberRelation_AppGlobalOwner_AppGlobalOwnerId",
                        column: x => x.AppGlobalOwnerId,
                        principalTable: "AppGlobalOwner",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MemberRelation_ChurchBody_ChurchBodyId",
                        column: x => x.ChurchBodyId,
                        principalTable: "ChurchBody",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MemberRelation_ChurchMember_ChurchMemberId",
                        column: x => x.ChurchMemberId,
                        principalTable: "ChurchMember",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MemberRelation_Country_CountryIdExtCon",
                        column: x => x.CountryIdExtCon,
                        principalTable: "Country",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MemberRelation_CountryRegion_RegionIdExtCon",
                        column: x => x.RegionIdExtCon,
                        principalTable: "CountryRegion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MemberRelation_ChurchMember_RelationChurchMemberId",
                        column: x => x.RelationChurchMemberId,
                        principalTable: "ChurchMember",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MemberRelation_RelationshipType_RelationshipId",
                        column: x => x.RelationshipId,
                        principalTable: "RelationshipType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MemberWorkExperience",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppGlobalOwnerId = table.Column<int>(nullable: true),
                    ChurchBodyId = table.Column<int>(nullable: true),
                    OwnedByChurchBodyId = table.Column<int>(nullable: true),
                    ChurchMemberId = table.Column<int>(nullable: false),
                    WorkPlace = table.Column<string>(maxLength: 50, nullable: false),
                    WorkRole = table.Column<string>(maxLength: 50, nullable: false),
                    Started = table.Column<DateTime>(nullable: true),
                    Ended = table.Column<DateTime>(nullable: true),
                    Reason = table.Column<string>(maxLength: 100, nullable: true),
                    IsCurrentWork = table.Column<bool>(nullable: false),
                    Created = table.Column<DateTime>(nullable: true),
                    LastMod = table.Column<DateTime>(nullable: true),
                    CreatedByUserId = table.Column<int>(nullable: true),
                    LastModByUserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberWorkExperience", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MemberWorkExperience_AppGlobalOwner_AppGlobalOwnerId",
                        column: x => x.AppGlobalOwnerId,
                        principalTable: "AppGlobalOwner",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MemberWorkExperience_ChurchBody_ChurchBodyId",
                        column: x => x.ChurchBodyId,
                        principalTable: "ChurchBody",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MemberWorkExperience_ChurchMember_ChurchMemberId",
                        column: x => x.ChurchMemberId,
                        principalTable: "ChurchMember",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MemberRegistration",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppGlobalOwnerId = table.Column<int>(nullable: true),
                    ChurchBodyId = table.Column<int>(nullable: true),
                    OwnedByChurchBodyId = table.Column<int>(nullable: true),
                    ChurchMemberId = table.Column<int>(nullable: false),
                    ChurchYear = table.Column<string>(maxLength: 9, nullable: true),
                    ChurchPeriodId = table.Column<int>(nullable: true),
                    RegistrationDate = table.Column<DateTime>(nullable: true),
                    RegCode = table.Column<string>(maxLength: 10, nullable: true),
                    Created = table.Column<DateTime>(nullable: true),
                    LastMod = table.Column<DateTime>(nullable: true),
                    CreatedByUserId = table.Column<int>(nullable: true),
                    LastModByUserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberRegistration", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MemberRegistration_AppGlobalOwner_AppGlobalOwnerId",
                        column: x => x.AppGlobalOwnerId,
                        principalTable: "AppGlobalOwner",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MemberRegistration_ChurchBody_ChurchBodyId",
                        column: x => x.ChurchBodyId,
                        principalTable: "ChurchBody",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MemberRegistration_ChurchMember_ChurchMemberId",
                        column: x => x.ChurchMemberId,
                        principalTable: "ChurchMember",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MemberRegistration_ActivityPeriod_ChurchPeriodId",
                        column: x => x.ChurchPeriodId,
                        principalTable: "ActivityPeriod",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MemberChurchLife",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppGlobalOwnerId = table.Column<int>(nullable: true),
                    ChurchBodyId = table.Column<int>(nullable: true),
                    OwnedByChurchBodyId = table.Column<int>(nullable: true),
                    ChurchMemberId = table.Column<int>(nullable: false),
                    Joined = table.Column<DateTime>(nullable: true),
                    Departed = table.Column<DateTime>(nullable: true),
                    IsPioneer = table.Column<bool>(nullable: false),
                    IsCurrentMember = table.Column<bool>(nullable: false),
                    IsMemBaptized = table.Column<bool>(nullable: false),
                    IsMemConfirmed = table.Column<bool>(nullable: false),
                    ChurchBodyServiceId = table.Column<int>(nullable: true),
                    IsMemCommunicant = table.Column<bool>(nullable: false),
                    NonCommReason = table.Column<string>(maxLength: 100, nullable: true),
                    EnrollReason = table.Column<string>(maxLength: 100, nullable: true),
                    DepartReason = table.Column<string>(maxLength: 100, nullable: true),
                    IsDeceased = table.Column<bool>(nullable: false),
                    MemberLifeSummary = table.Column<string>(maxLength: 200, nullable: true),
                    Created = table.Column<DateTime>(nullable: true),
                    LastMod = table.Column<DateTime>(nullable: true),
                    CreatedByUserId = table.Column<int>(nullable: true),
                    LastModByUserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberChurchLife", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MemberChurchLife_AppGlobalOwner_AppGlobalOwnerId",
                        column: x => x.AppGlobalOwnerId,
                        principalTable: "AppGlobalOwner",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MemberChurchLife_ChurchBody_ChurchBodyId",
                        column: x => x.ChurchBodyId,
                        principalTable: "ChurchBody",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MemberChurchLife_ChurchBodyService_ChurchBodyServiceId",
                        column: x => x.ChurchBodyServiceId,
                        principalTable: "ChurchBodyService",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MemberChurchLife_ChurchMember_ChurchMemberId",
                        column: x => x.ChurchMemberId,
                        principalTable: "ChurchMember",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChurchCalendarEvent",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppGlobalOwnerId = table.Column<int>(nullable: true),
                    ChurchBodyId = table.Column<int>(nullable: true),
                    OwnedByChurchBodyId = table.Column<int>(nullable: true),
                    Subject = table.Column<string>(maxLength: 100, nullable: true),
                    Description = table.Column<string>(maxLength: 200, nullable: true),
                    ChurchLifeActivityId = table.Column<int>(nullable: true),
                    ChurchEventCategoryId = table.Column<int>(nullable: true),
                    IsChurchServiceEvent = table.Column<bool>(nullable: false),
                    ChurchBodyServiceId = table.Column<int>(nullable: true),
                    Venue = table.Column<string>(maxLength: 100, nullable: true),
                    IsFullDay = table.Column<bool>(nullable: false),
                    EventTo = table.Column<DateTime>(nullable: true),
                    EventFrom = table.Column<DateTime>(nullable: true),
                    ThemeColor = table.Column<string>(maxLength: 30, nullable: true),
                    IsEventActive = table.Column<bool>(nullable: false),
                    SharingStatus = table.Column<string>(maxLength: 1, nullable: true),
                    PhotoUrl = table.Column<string>(nullable: true),
                    Created = table.Column<DateTime>(nullable: true),
                    LastMod = table.Column<DateTime>(nullable: true),
                    CreatedByUserId = table.Column<int>(nullable: true),
                    LastModByUserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChurchCalendarEvent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChurchCalendarEvent_AppGlobalOwner_AppGlobalOwnerId",
                        column: x => x.AppGlobalOwnerId,
                        principalTable: "AppGlobalOwner",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChurchCalendarEvent_ChurchBody_ChurchBodyId",
                        column: x => x.ChurchBodyId,
                        principalTable: "ChurchBody",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChurchCalendarEvent_ChurchBodyService_ChurchBodyServiceId",
                        column: x => x.ChurchBodyServiceId,
                        principalTable: "ChurchBodyService",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChurchCalendarEvent_ChurchEventCategory_ChurchEventCategoryId",
                        column: x => x.ChurchEventCategoryId,
                        principalTable: "ChurchEventCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChurchCalendarEvent_ChurchLifeActivity_ChurchLifeActivityId",
                        column: x => x.ChurchLifeActivityId,
                        principalTable: "ChurchLifeActivity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ChurchLifeActivityReqDef",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppGlobalOwnerId = table.Column<int>(nullable: true),
                    ChurchBodyId = table.Column<int>(nullable: true),
                    OwnedByChurchBodyId = table.Column<int>(nullable: true),
                    ChurchLifeActivityId = table.Column<int>(nullable: true),
                    RequirementDesc = table.Column<string>(maxLength: 50, nullable: true),
                    OrderIndex = table.Column<int>(nullable: true),
                    ExpectedOccurences = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    Frequency = table.Column<string>(maxLength: 2, nullable: true),
                    IsRequired = table.Column<bool>(nullable: false),
                    IsSequenced = table.Column<bool>(nullable: false),
                    SharingStatus = table.Column<string>(maxLength: 1, nullable: true),
                    Created = table.Column<DateTime>(nullable: true),
                    LastMod = table.Column<DateTime>(nullable: true),
                    CreatedByUserId = table.Column<int>(nullable: true),
                    LastModByUserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChurchLifeActivityReqDef", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChurchLifeActivityReqDef_AppGlobalOwner_AppGlobalOwnerId",
                        column: x => x.AppGlobalOwnerId,
                        principalTable: "AppGlobalOwner",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChurchLifeActivityReqDef_ChurchBody_ChurchBodyId",
                        column: x => x.ChurchBodyId,
                        principalTable: "ChurchBody",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChurchLifeActivityReqDef_ChurchLifeActivity_ChurchLifeActivityId",
                        column: x => x.ChurchLifeActivityId,
                        principalTable: "ChurchLifeActivity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MemberStatus",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppGlobalOwnerId = table.Column<int>(nullable: true),
                    ChurchBodyId = table.Column<int>(nullable: true),
                    OwnedByChurchBodyId = table.Column<int>(nullable: true),
                    ChurchMemberId = table.Column<int>(nullable: true),
                    ChurchMemStatusId = table.Column<int>(nullable: true),
                    IsCurrent = table.Column<bool>(nullable: false),
                    Since = table.Column<DateTime>(nullable: true),
                    Until = table.Column<DateTime>(nullable: true),
                    Comments = table.Column<string>(maxLength: 200, nullable: true),
                    Created = table.Column<DateTime>(nullable: true),
                    LastMod = table.Column<DateTime>(nullable: true),
                    CreatedByUserId = table.Column<int>(nullable: true),
                    LastModByUserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberStatus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MemberStatus_AppGlobalOwner_AppGlobalOwnerId",
                        column: x => x.AppGlobalOwnerId,
                        principalTable: "AppGlobalOwner",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MemberStatus_ChurchBody_ChurchBodyId",
                        column: x => x.ChurchBodyId,
                        principalTable: "ChurchBody",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MemberStatus_ChurchMemStatus_ChurchMemStatusId",
                        column: x => x.ChurchMemStatusId,
                        principalTable: "ChurchMemStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MemberStatus_ChurchMember_ChurchMemberId",
                        column: x => x.ChurchMemberId,
                        principalTable: "ChurchMember",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MemberType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppGlobalOwnerId = table.Column<int>(nullable: true),
                    ChurchBodyId = table.Column<int>(nullable: true),
                    OwnedByChurchBodyId = table.Column<int>(nullable: true),
                    ChurchMemberId = table.Column<int>(nullable: false),
                    ChurchMemTypeId = table.Column<int>(nullable: true),
                    Assigned = table.Column<DateTime>(nullable: true),
                    IsCurrent = table.Column<bool>(nullable: false),
                    Until = table.Column<DateTime>(nullable: true),
                    Comments = table.Column<string>(maxLength: 300, nullable: true),
                    Created = table.Column<DateTime>(nullable: true),
                    LastMod = table.Column<DateTime>(nullable: true),
                    CreatedByUserId = table.Column<int>(nullable: true),
                    LastModByUserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MemberType_AppGlobalOwner_AppGlobalOwnerId",
                        column: x => x.AppGlobalOwnerId,
                        principalTable: "AppGlobalOwner",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MemberType_ChurchBody_ChurchBodyId",
                        column: x => x.ChurchBodyId,
                        principalTable: "ChurchBody",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MemberType_ChurchMemType_ChurchMemTypeId",
                        column: x => x.ChurchMemTypeId,
                        principalTable: "ChurchMemType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MemberType_ChurchMember_ChurchMemberId",
                        column: x => x.ChurchMemberId,
                        principalTable: "ChurchMember",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MemberRank",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppGlobalOwnerId = table.Column<int>(nullable: true),
                    ChurchBodyId = table.Column<int>(nullable: true),
                    OwnedByChurchBodyId = table.Column<int>(nullable: true),
                    ChurchMemberId = table.Column<int>(nullable: false),
                    ChurchRankId = table.Column<int>(nullable: true),
                    Assigned = table.Column<DateTime>(nullable: true),
                    IsCurrentRank = table.Column<bool>(nullable: false),
                    Until = table.Column<DateTime>(nullable: true),
                    Comments = table.Column<string>(maxLength: 200, nullable: true),
                    Created = table.Column<DateTime>(nullable: true),
                    LastMod = table.Column<DateTime>(nullable: true),
                    CreatedByUserId = table.Column<int>(nullable: true),
                    LastModByUserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberRank", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MemberRank_AppGlobalOwner_AppGlobalOwnerId",
                        column: x => x.AppGlobalOwnerId,
                        principalTable: "AppGlobalOwner",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MemberRank_ChurchBody_ChurchBodyId",
                        column: x => x.ChurchBodyId,
                        principalTable: "ChurchBody",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MemberRank_ChurchMember_ChurchMemberId",
                        column: x => x.ChurchMemberId,
                        principalTable: "ChurchMember",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MemberRank_ChurchRank_ChurchRankId",
                        column: x => x.ChurchRankId,
                        principalTable: "ChurchRank",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MemberChurchSector",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppGlobalOwnerId = table.Column<int>(nullable: true),
                    ChurchBodyId = table.Column<int>(nullable: true),
                    OwnedByChurchBodyId = table.Column<int>(nullable: true),
                    ChurchMemberId = table.Column<int>(nullable: true),
                    ChurchSectorId = table.Column<int>(nullable: true),
                    Joined = table.Column<DateTime>(nullable: true),
                    Departed = table.Column<DateTime>(nullable: true),
                    IsCoreArea = table.Column<bool>(nullable: false),
                    IsPioneer = table.Column<bool>(nullable: false),
                    IsCurrSector = table.Column<bool>(nullable: false),
                    IsCurrentMember = table.Column<bool>(nullable: false),
                    Created = table.Column<DateTime>(nullable: true),
                    LastMod = table.Column<DateTime>(nullable: true),
                    CreatedByUserId = table.Column<int>(nullable: true),
                    LastModByUserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberChurchSector", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MemberChurchSector_AppGlobalOwner_AppGlobalOwnerId",
                        column: x => x.AppGlobalOwnerId,
                        principalTable: "AppGlobalOwner",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MemberChurchSector_ChurchBody_ChurchBodyId",
                        column: x => x.ChurchBodyId,
                        principalTable: "ChurchBody",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MemberChurchSector_ChurchMember_ChurchMemberId",
                        column: x => x.ChurchMemberId,
                        principalTable: "ChurchMember",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MemberChurchSector_ChurchSectorUnit_ChurchSectorId",
                        column: x => x.ChurchSectorId,
                        principalTable: "ChurchSectorUnit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ChurchVisitor",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppGlobalOwnerId = table.Column<int>(nullable: false),
                    ChurchBodyId = table.Column<int>(nullable: false),
                    OwnedByChurchBodyId = table.Column<int>(nullable: false),
                    ChurchVisitorTypeId = table.Column<int>(nullable: true),
                    ChurchActivityId = table.Column<int>(nullable: true),
                    AgeBracketId = table.Column<int>(nullable: true),
                    VisitReason = table.Column<string>(maxLength: 100, nullable: true),
                    Vis_Status = table.Column<string>(maxLength: 1, nullable: true),
                    Comments = table.Column<string>(maxLength: 200, nullable: true),
                    Title = table.Column<string>(maxLength: 10, nullable: true),
                    FirstName = table.Column<string>(maxLength: 30, nullable: true),
                    MiddleName = table.Column<string>(maxLength: 30, nullable: true),
                    LastName = table.Column<string>(maxLength: 30, nullable: true),
                    Gender = table.Column<string>(maxLength: 1, nullable: true),
                    MaritalStatus = table.Column<string>(maxLength: 1, nullable: false),
                    FirstVisitDate = table.Column<DateTime>(nullable: true),
                    NationalityId = table.Column<int>(nullable: false),
                    PrimaryLanguageId = table.Column<int>(nullable: true),
                    ResidenceAddress = table.Column<string>(maxLength: 100, nullable: true),
                    Location = table.Column<string>(maxLength: 30, nullable: true),
                    DigitalAddress = table.Column<string>(maxLength: 50, nullable: true),
                    MobilePhone = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Denomination = table.Column<string>(maxLength: 100, nullable: true),
                    MSTR_FaithTypeCategoryId = table.Column<int>(nullable: true),
                    IsContactPersChurchFellow = table.Column<bool>(nullable: true),
                    ChurchContactId = table.Column<int>(nullable: true),
                    ExtConChurchContactName = table.Column<string>(maxLength: 100, nullable: true),
                    ExtConChurchContactLocation = table.Column<string>(maxLength: 50, nullable: true),
                    ExtConChurchContactMobilePhone = table.Column<string>(nullable: true),
                    ExtConChurchContactEmail = table.Column<string>(nullable: true),
                    Created = table.Column<DateTime>(nullable: true),
                    LastMod = table.Column<DateTime>(nullable: true),
                    CreatedByUserId = table.Column<int>(nullable: true),
                    LastModByUserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChurchVisitor", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChurchVisitor_AppUtilityNVP_AgeBracketId",
                        column: x => x.AgeBracketId,
                        principalTable: "AppUtilityNVP",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChurchVisitor_AppGlobalOwner_AppGlobalOwnerId",
                        column: x => x.AppGlobalOwnerId,
                        principalTable: "AppGlobalOwner",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChurchVisitor_ChurchLifeActivity_ChurchActivityId",
                        column: x => x.ChurchActivityId,
                        principalTable: "ChurchLifeActivity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChurchVisitor_ChurchBody_ChurchBodyId",
                        column: x => x.ChurchBodyId,
                        principalTable: "ChurchBody",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChurchVisitor_ChurchMember_ChurchContactId",
                        column: x => x.ChurchContactId,
                        principalTable: "ChurchMember",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChurchVisitor_ChurchVisitorType_ChurchVisitorTypeId",
                        column: x => x.ChurchVisitorTypeId,
                        principalTable: "ChurchVisitorType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChurchVisitor_Country_NationalityId",
                        column: x => x.NationalityId,
                        principalTable: "Country",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChurchVisitor_LanguageSpoken_PrimaryLanguageId",
                        column: x => x.PrimaryLanguageId,
                        principalTable: "LanguageSpoken",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LeaderRole",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChurchBodyId = table.Column<int>(nullable: false),
                    RoleName = table.Column<string>(maxLength: 30, nullable: false),
                    LeaderRoleCategoryId = table.Column<int>(nullable: true),
                    AuthorityIndex = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    OwnedByChurchBodyId = table.Column<int>(nullable: true),
                    Status = table.Column<string>(maxLength: 1, nullable: true),
                    ScopeRestricted = table.Column<bool>(nullable: false),
                    SharingStatus = table.Column<string>(maxLength: 1, nullable: true),
                    Created = table.Column<DateTime>(nullable: true),
                    LastMod = table.Column<DateTime>(nullable: true),
                    CreatedByUserId = table.Column<int>(nullable: true),
                    LastModByUserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaderRole", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LeaderRole_ChurchBody_ChurchBodyId",
                        column: x => x.ChurchBodyId,
                        principalTable: "ChurchBody",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LeaderRole_LeaderRoleCategory_LeaderRoleCategoryId",
                        column: x => x.LeaderRoleCategoryId,
                        principalTable: "LeaderRoleCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LeaderRole_ChurchBody_OwnedByChurchBodyId",
                        column: x => x.OwnedByChurchBodyId,
                        principalTable: "ChurchBody",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MemberChurchRole",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppGlobalOwnerId = table.Column<int>(nullable: true),
                    ChurchBodyId = table.Column<int>(nullable: true),
                    OwnedByChurchBodyId = table.Column<int>(nullable: true),
                    ChurchMemberId = table.Column<int>(nullable: true),
                    OrganisationType = table.Column<string>(maxLength: 2, nullable: true),
                    IsRoleUnitChurchSector = table.Column<bool>(nullable: false),
                    ChurchSectorUnitId = table.Column<int>(nullable: true),
                    ChurchBodyUnitId = table.Column<int>(nullable: true),
                    LeaderRoleId = table.Column<int>(nullable: true),
                    IsCoreRole = table.Column<bool>(nullable: false),
                    IsCurrentRole = table.Column<bool>(nullable: false),
                    BatchCode = table.Column<string>(maxLength: 10, nullable: true),
                    Commenced = table.Column<DateTime>(nullable: true),
                    Completed = table.Column<DateTime>(nullable: true),
                    CompletionReason = table.Column<string>(maxLength: 50, nullable: true),
                    RoleProfile = table.Column<string>(maxLength: 200, nullable: true),
                    Created = table.Column<DateTime>(nullable: true),
                    LastMod = table.Column<DateTime>(nullable: true),
                    CreatedByUserId = table.Column<int>(nullable: true),
                    LastModByUserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberChurchRole", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MemberChurchRole_AppGlobalOwner_AppGlobalOwnerId",
                        column: x => x.AppGlobalOwnerId,
                        principalTable: "AppGlobalOwner",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MemberChurchRole_ChurchBody_ChurchBodyId",
                        column: x => x.ChurchBodyId,
                        principalTable: "ChurchBody",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MemberChurchRole_ChurchBody_ChurchBodyUnitId",
                        column: x => x.ChurchBodyUnitId,
                        principalTable: "ChurchBody",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MemberChurchRole_ChurchMember_ChurchMemberId",
                        column: x => x.ChurchMemberId,
                        principalTable: "ChurchMember",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MemberChurchRole_ChurchBody_ChurchSectorUnitId",
                        column: x => x.ChurchSectorUnitId,
                        principalTable: "ChurchBody",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MemberChurchRole_LeaderRole_LeaderRoleId",
                        column: x => x.LeaderRoleId,
                        principalTable: "LeaderRole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EventActivityReqLog",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppGlobalOwnerId = table.Column<int>(nullable: true),
                    ChurchBodyId = table.Column<int>(nullable: true),
                    OwnedByChurchBodyId = table.Column<int>(nullable: true),
                    ChurchMemberId = table.Column<int>(nullable: true),
                    ChurchCalendarId = table.Column<int>(nullable: false),
                    ChurchEventId = table.Column<int>(nullable: true),
                    RequirementDefId = table.Column<int>(nullable: true),
                    MemberChurchRoleId = table.Column<int>(nullable: true),
                    Status = table.Column<string>(maxLength: 1, nullable: true),
                    Completed = table.Column<DateTime>(nullable: true),
                    OrderIndex = table.Column<int>(nullable: true),
                    Details = table.Column<string>(maxLength: 100, nullable: true),
                    PhotoUrl = table.Column<string>(nullable: true),
                    Notes = table.Column<string>(maxLength: 200, nullable: true),
                    Created = table.Column<DateTime>(nullable: true),
                    LastMod = table.Column<DateTime>(nullable: true),
                    CreatedByUserId = table.Column<int>(nullable: true),
                    LastModByUserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventActivityReqLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventActivityReqLog_AppGlobalOwner_AppGlobalOwnerId",
                        column: x => x.AppGlobalOwnerId,
                        principalTable: "AppGlobalOwner",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EventActivityReqLog_ChurchBody_ChurchBodyId",
                        column: x => x.ChurchBodyId,
                        principalTable: "ChurchBody",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EventActivityReqLog_ChurchCalendarEvent_ChurchEventId",
                        column: x => x.ChurchEventId,
                        principalTable: "ChurchCalendarEvent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EventActivityReqLog_ChurchMember_ChurchMemberId",
                        column: x => x.ChurchMemberId,
                        principalTable: "ChurchMember",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EventActivityReqLog_MemberChurchRole_MemberChurchRoleId",
                        column: x => x.MemberChurchRoleId,
                        principalTable: "MemberChurchRole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EventActivityReqLog_ChurchLifeActivityReqDef_RequirementDefId",
                        column: x => x.RequirementDefId,
                        principalTable: "ChurchLifeActivityReqDef",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MemberChurchLifeActivity",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppGlobalOwnerId = table.Column<int>(nullable: true),
                    ChurchBodyId = table.Column<int>(nullable: true),
                    OwnedByChurchBodyId = table.Column<int>(nullable: true),
                    ChurchMemberId = table.Column<int>(nullable: true),
                    ChurchLifeActivityId = table.Column<int>(nullable: true),
                    EventDate = table.Column<DateTime>(nullable: true),
                    Venue = table.Column<string>(maxLength: 50, nullable: true),
                    MemberChurchRoleId = table.Column<int>(nullable: true),
                    OfficiatedByName = table.Column<string>(maxLength: 100, nullable: true),
                    OfficiatedByRole = table.Column<string>(maxLength: 100, nullable: true),
                    ActivityVenue = table.Column<string>(maxLength: 100, nullable: true),
                    IsActivity = table.Column<bool>(nullable: false),
                    IsOfficiatedByChurchFellow = table.Column<bool>(nullable: false),
                    VenueChurchBodyId = table.Column<int>(nullable: true),
                    CongregationExt = table.Column<string>(maxLength: 100, nullable: true),
                    IsOfficiatedByExt = table.Column<bool>(nullable: false),
                    OfficiatedByCongExt = table.Column<string>(maxLength: 100, nullable: true),
                    OfficiatedByNameExt = table.Column<string>(maxLength: 100, nullable: true),
                    OfficiatedByRoleExt = table.Column<string>(maxLength: 100, nullable: true),
                    PhotoUrl = table.Column<string>(nullable: true),
                    Comments = table.Column<string>(maxLength: 300, nullable: true),
                    Created = table.Column<DateTime>(nullable: true),
                    LastMod = table.Column<DateTime>(nullable: true),
                    CreatedByUserId = table.Column<int>(nullable: true),
                    LastModByUserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberChurchLifeActivity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MemberChurchLifeActivity_AppGlobalOwner_AppGlobalOwnerId",
                        column: x => x.AppGlobalOwnerId,
                        principalTable: "AppGlobalOwner",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MemberChurchLifeActivity_ChurchBody_ChurchBodyId",
                        column: x => x.ChurchBodyId,
                        principalTable: "ChurchBody",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MemberChurchLifeActivity_ChurchLifeActivity_ChurchLifeActivityId",
                        column: x => x.ChurchLifeActivityId,
                        principalTable: "ChurchLifeActivity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MemberChurchLifeActivity_ChurchMember_ChurchMemberId",
                        column: x => x.ChurchMemberId,
                        principalTable: "ChurchMember",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MemberChurchLifeActivity_MemberChurchRole_MemberChurchRoleId",
                        column: x => x.MemberChurchRoleId,
                        principalTable: "MemberChurchRole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MemberChurchLifeActivity_ChurchBody_VenueChurchBodyId",
                        column: x => x.VenueChurchBodyId,
                        principalTable: "ChurchBody",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChurchBodyService_AppGlobalOwnerId",
                table: "ChurchBodyService",
                column: "AppGlobalOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_ChurchBodyService_ChurchBodyId",
                table: "ChurchBodyService",
                column: "ChurchBodyId");

            migrationBuilder.CreateIndex(
                name: "IX_ChurchBodyService_ServiceCategoryId",
                table: "ChurchBodyService",
                column: "ServiceCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ChurchCalendarEvent_AppGlobalOwnerId",
                table: "ChurchCalendarEvent",
                column: "AppGlobalOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_ChurchCalendarEvent_ChurchBodyId",
                table: "ChurchCalendarEvent",
                column: "ChurchBodyId");

            migrationBuilder.CreateIndex(
                name: "IX_ChurchCalendarEvent_ChurchBodyServiceId",
                table: "ChurchCalendarEvent",
                column: "ChurchBodyServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_ChurchCalendarEvent_ChurchEventCategoryId",
                table: "ChurchCalendarEvent",
                column: "ChurchEventCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ChurchCalendarEvent_ChurchLifeActivityId",
                table: "ChurchCalendarEvent",
                column: "ChurchLifeActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_ChurchEventCategory_AppGlobalOwnerId",
                table: "ChurchEventCategory",
                column: "AppGlobalOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_ChurchEventCategory_ChurchBodyId",
                table: "ChurchEventCategory",
                column: "ChurchBodyId");

            migrationBuilder.CreateIndex(
                name: "IX_ChurchLifeActivity_AppGlobalOwnerId",
                table: "ChurchLifeActivity",
                column: "AppGlobalOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_ChurchLifeActivity_ChurchBodyId",
                table: "ChurchLifeActivity",
                column: "ChurchBodyId");

            migrationBuilder.CreateIndex(
                name: "IX_ChurchLifeActivityReqDef_AppGlobalOwnerId",
                table: "ChurchLifeActivityReqDef",
                column: "AppGlobalOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_ChurchLifeActivityReqDef_ChurchBodyId",
                table: "ChurchLifeActivityReqDef",
                column: "ChurchBodyId");

            migrationBuilder.CreateIndex(
                name: "IX_ChurchLifeActivityReqDef_ChurchLifeActivityId",
                table: "ChurchLifeActivityReqDef",
                column: "ChurchLifeActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_ChurchMemStatus_AppGlobalOwnerId",
                table: "ChurchMemStatus",
                column: "AppGlobalOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_ChurchMemStatus_ChurchBodyId",
                table: "ChurchMemStatus",
                column: "ChurchBodyId");

            migrationBuilder.CreateIndex(
                name: "IX_ChurchMemType_AppGlobalOwnerId",
                table: "ChurchMemType",
                column: "AppGlobalOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_ChurchMemType_ChurchBodyId",
                table: "ChurchMemType",
                column: "ChurchBodyId");

            migrationBuilder.CreateIndex(
                name: "IX_ChurchRank_AppGlobalOwnerId",
                table: "ChurchRank",
                column: "AppGlobalOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_ChurchRank_ChurchBodyId",
                table: "ChurchRank",
                column: "ChurchBodyId");

            migrationBuilder.CreateIndex(
                name: "IX_ChurchRole_AppGlobalOwnerId",
                table: "ChurchRole",
                column: "AppGlobalOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_ChurchRole_OwnedByChurchBodyId",
                table: "ChurchRole",
                column: "OwnedByChurchBodyId");

            migrationBuilder.CreateIndex(
                name: "IX_ChurchRole_ParentChurchBodyId",
                table: "ChurchRole",
                column: "ParentChurchBodyId");

            migrationBuilder.CreateIndex(
                name: "IX_ChurchRole_TargetChurchLevelId",
                table: "ChurchRole",
                column: "TargetChurchLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_ChurchSectorUnit_AppGlobalOwnerId",
                table: "ChurchSectorUnit",
                column: "AppGlobalOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_ChurchSectorUnit_ContactInfoId",
                table: "ChurchSectorUnit",
                column: "ContactInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_ChurchSectorUnit_OwnedByChurchBodyId",
                table: "ChurchSectorUnit",
                column: "OwnedByChurchBodyId");

            migrationBuilder.CreateIndex(
                name: "IX_ChurchSectorUnit_ParentChurchBodyId",
                table: "ChurchSectorUnit",
                column: "ParentChurchBodyId");

            migrationBuilder.CreateIndex(
                name: "IX_ChurchSectorUnit_TargetChurchLevelId",
                table: "ChurchSectorUnit",
                column: "TargetChurchLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_ChurchVisitor_AgeBracketId",
                table: "ChurchVisitor",
                column: "AgeBracketId");

            migrationBuilder.CreateIndex(
                name: "IX_ChurchVisitor_AppGlobalOwnerId",
                table: "ChurchVisitor",
                column: "AppGlobalOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_ChurchVisitor_ChurchActivityId",
                table: "ChurchVisitor",
                column: "ChurchActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_ChurchVisitor_ChurchBodyId",
                table: "ChurchVisitor",
                column: "ChurchBodyId");

            migrationBuilder.CreateIndex(
                name: "IX_ChurchVisitor_ChurchContactId",
                table: "ChurchVisitor",
                column: "ChurchContactId");

            migrationBuilder.CreateIndex(
                name: "IX_ChurchVisitor_ChurchVisitorTypeId",
                table: "ChurchVisitor",
                column: "ChurchVisitorTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ChurchVisitor_NationalityId",
                table: "ChurchVisitor",
                column: "NationalityId");

            migrationBuilder.CreateIndex(
                name: "IX_ChurchVisitor_PrimaryLanguageId",
                table: "ChurchVisitor",
                column: "PrimaryLanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_ChurchVisitorType_AppGlobalOwnerId",
                table: "ChurchVisitorType",
                column: "AppGlobalOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_ChurchVisitorType_ChurchBodyId",
                table: "ChurchVisitorType",
                column: "ChurchBodyId");

            migrationBuilder.CreateIndex(
                name: "IX_EventActivityReqLog_AppGlobalOwnerId",
                table: "EventActivityReqLog",
                column: "AppGlobalOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_EventActivityReqLog_ChurchBodyId",
                table: "EventActivityReqLog",
                column: "ChurchBodyId");

            migrationBuilder.CreateIndex(
                name: "IX_EventActivityReqLog_ChurchEventId",
                table: "EventActivityReqLog",
                column: "ChurchEventId");

            migrationBuilder.CreateIndex(
                name: "IX_EventActivityReqLog_ChurchMemberId",
                table: "EventActivityReqLog",
                column: "ChurchMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_EventActivityReqLog_MemberChurchRoleId",
                table: "EventActivityReqLog",
                column: "MemberChurchRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_EventActivityReqLog_RequirementDefId",
                table: "EventActivityReqLog",
                column: "RequirementDefId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaderRole_ChurchBodyId",
                table: "LeaderRole",
                column: "ChurchBodyId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaderRole_LeaderRoleCategoryId",
                table: "LeaderRole",
                column: "LeaderRoleCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaderRole_OwnedByChurchBodyId",
                table: "LeaderRole",
                column: "OwnedByChurchBodyId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaderRoleCategory_ChurchBodyId",
                table: "LeaderRoleCategory",
                column: "ChurchBodyId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaderRoleCategory_OwnedByChurchBodyId",
                table: "LeaderRoleCategory",
                column: "OwnedByChurchBodyId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberChurchLife_AppGlobalOwnerId",
                table: "MemberChurchLife",
                column: "AppGlobalOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberChurchLife_ChurchBodyId",
                table: "MemberChurchLife",
                column: "ChurchBodyId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberChurchLife_ChurchBodyServiceId",
                table: "MemberChurchLife",
                column: "ChurchBodyServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberChurchLife_ChurchMemberId",
                table: "MemberChurchLife",
                column: "ChurchMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberChurchLifeActivity_AppGlobalOwnerId",
                table: "MemberChurchLifeActivity",
                column: "AppGlobalOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberChurchLifeActivity_ChurchBodyId",
                table: "MemberChurchLifeActivity",
                column: "ChurchBodyId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberChurchLifeActivity_ChurchLifeActivityId",
                table: "MemberChurchLifeActivity",
                column: "ChurchLifeActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberChurchLifeActivity_ChurchMemberId",
                table: "MemberChurchLifeActivity",
                column: "ChurchMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberChurchLifeActivity_MemberChurchRoleId",
                table: "MemberChurchLifeActivity",
                column: "MemberChurchRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberChurchLifeActivity_VenueChurchBodyId",
                table: "MemberChurchLifeActivity",
                column: "VenueChurchBodyId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberChurchRole_AppGlobalOwnerId",
                table: "MemberChurchRole",
                column: "AppGlobalOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberChurchRole_ChurchBodyId",
                table: "MemberChurchRole",
                column: "ChurchBodyId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberChurchRole_ChurchBodyUnitId",
                table: "MemberChurchRole",
                column: "ChurchBodyUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberChurchRole_ChurchMemberId",
                table: "MemberChurchRole",
                column: "ChurchMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberChurchRole_ChurchSectorUnitId",
                table: "MemberChurchRole",
                column: "ChurchSectorUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberChurchRole_LeaderRoleId",
                table: "MemberChurchRole",
                column: "LeaderRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberChurchSector_AppGlobalOwnerId",
                table: "MemberChurchSector",
                column: "AppGlobalOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberChurchSector_ChurchBodyId",
                table: "MemberChurchSector",
                column: "ChurchBodyId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberChurchSector_ChurchMemberId",
                table: "MemberChurchSector",
                column: "ChurchMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberChurchSector_ChurchSectorId",
                table: "MemberChurchSector",
                column: "ChurchSectorId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberContact_AppGlobalOwnerId",
                table: "MemberContact",
                column: "AppGlobalOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberContact_ChurchBodyId",
                table: "MemberContact",
                column: "ChurchBodyId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberContact_ChurchMemberId",
                table: "MemberContact",
                column: "ChurchMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberContact_ExtConCountryId",
                table: "MemberContact",
                column: "ExtConCountryId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberContact_ExtConRegionId",
                table: "MemberContact",
                column: "ExtConRegionId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberContact_InternalContactId",
                table: "MemberContact",
                column: "InternalContactId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberContact_RelationshipId",
                table: "MemberContact",
                column: "RelationshipId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberEducHistory_AppGlobalOwnerId",
                table: "MemberEducHistory",
                column: "AppGlobalOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberEducHistory_CertificateId",
                table: "MemberEducHistory",
                column: "CertificateId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberEducHistory_ChurchBodyId",
                table: "MemberEducHistory",
                column: "ChurchBodyId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberEducHistory_ChurchMemberId",
                table: "MemberEducHistory",
                column: "ChurchMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberEducHistory_CountryId",
                table: "MemberEducHistory",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberEducHistory_InstitutionTypeId",
                table: "MemberEducHistory",
                column: "InstitutionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberLanguageSpoken_AppGlobalOwnerId",
                table: "MemberLanguageSpoken",
                column: "AppGlobalOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberLanguageSpoken_ChurchBodyId",
                table: "MemberLanguageSpoken",
                column: "ChurchBodyId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberLanguageSpoken_ChurchMemberId",
                table: "MemberLanguageSpoken",
                column: "ChurchMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberLanguageSpoken_LanguageSpokenId",
                table: "MemberLanguageSpoken",
                column: "LanguageSpokenId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberProfessionBrand_AppGlobalOwnerId",
                table: "MemberProfessionBrand",
                column: "AppGlobalOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberProfessionBrand_ChurchBodyId",
                table: "MemberProfessionBrand",
                column: "ChurchBodyId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberProfessionBrand_ChurchMemberId",
                table: "MemberProfessionBrand",
                column: "ChurchMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberRank_AppGlobalOwnerId",
                table: "MemberRank",
                column: "AppGlobalOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberRank_ChurchBodyId",
                table: "MemberRank",
                column: "ChurchBodyId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberRank_ChurchMemberId",
                table: "MemberRank",
                column: "ChurchMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberRank_ChurchRankId",
                table: "MemberRank",
                column: "ChurchRankId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberRegistration_AppGlobalOwnerId",
                table: "MemberRegistration",
                column: "AppGlobalOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberRegistration_ChurchBodyId",
                table: "MemberRegistration",
                column: "ChurchBodyId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberRegistration_ChurchMemberId",
                table: "MemberRegistration",
                column: "ChurchMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberRegistration_ChurchPeriodId",
                table: "MemberRegistration",
                column: "ChurchPeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberRelation_AppGlobalOwnerId",
                table: "MemberRelation",
                column: "AppGlobalOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberRelation_ChurchBodyId",
                table: "MemberRelation",
                column: "ChurchBodyId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberRelation_ChurchMemberId",
                table: "MemberRelation",
                column: "ChurchMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberRelation_CountryIdExtCon",
                table: "MemberRelation",
                column: "CountryIdExtCon");

            migrationBuilder.CreateIndex(
                name: "IX_MemberRelation_RegionIdExtCon",
                table: "MemberRelation",
                column: "RegionIdExtCon");

            migrationBuilder.CreateIndex(
                name: "IX_MemberRelation_RelationChurchMemberId",
                table: "MemberRelation",
                column: "RelationChurchMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberRelation_RelationshipId",
                table: "MemberRelation",
                column: "RelationshipId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberStatus_AppGlobalOwnerId",
                table: "MemberStatus",
                column: "AppGlobalOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberStatus_ChurchBodyId",
                table: "MemberStatus",
                column: "ChurchBodyId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberStatus_ChurchMemStatusId",
                table: "MemberStatus",
                column: "ChurchMemStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberStatus_ChurchMemberId",
                table: "MemberStatus",
                column: "ChurchMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberType_AppGlobalOwnerId",
                table: "MemberType",
                column: "AppGlobalOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberType_ChurchBodyId",
                table: "MemberType",
                column: "ChurchBodyId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberType_ChurchMemTypeId",
                table: "MemberType",
                column: "ChurchMemTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberType_ChurchMemberId",
                table: "MemberType",
                column: "ChurchMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberWorkExperience_AppGlobalOwnerId",
                table: "MemberWorkExperience",
                column: "AppGlobalOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberWorkExperience_ChurchBodyId",
                table: "MemberWorkExperience",
                column: "ChurchBodyId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberWorkExperience_ChurchMemberId",
                table: "MemberWorkExperience",
                column: "ChurchMemberId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChurchRole");

            migrationBuilder.DropTable(
                name: "ChurchVisitor");

            migrationBuilder.DropTable(
                name: "EventActivityReqLog");

            migrationBuilder.DropTable(
                name: "MemberChurchLife");

            migrationBuilder.DropTable(
                name: "MemberChurchLifeActivity");

            migrationBuilder.DropTable(
                name: "MemberChurchSector");

            migrationBuilder.DropTable(
                name: "MemberContact");

            migrationBuilder.DropTable(
                name: "MemberEducHistory");

            migrationBuilder.DropTable(
                name: "MemberLanguageSpoken");

            migrationBuilder.DropTable(
                name: "MemberProfessionBrand");

            migrationBuilder.DropTable(
                name: "MemberRank");

            migrationBuilder.DropTable(
                name: "MemberRegistration");

            migrationBuilder.DropTable(
                name: "MemberRelation");

            migrationBuilder.DropTable(
                name: "MemberStatus");

            migrationBuilder.DropTable(
                name: "MemberType");

            migrationBuilder.DropTable(
                name: "MemberWorkExperience");

            migrationBuilder.DropTable(
                name: "ChurchVisitorType");

            migrationBuilder.DropTable(
                name: "ChurchCalendarEvent");

            migrationBuilder.DropTable(
                name: "ChurchLifeActivityReqDef");

            migrationBuilder.DropTable(
                name: "MemberChurchRole");

            migrationBuilder.DropTable(
                name: "ChurchSectorUnit");

            migrationBuilder.DropTable(
                name: "ChurchRank");

            migrationBuilder.DropTable(
                name: "ActivityPeriod");

            migrationBuilder.DropTable(
                name: "ChurchMemStatus");

            migrationBuilder.DropTable(
                name: "ChurchMemType");

            migrationBuilder.DropTable(
                name: "ChurchBodyService");

            migrationBuilder.DropTable(
                name: "ChurchEventCategory");

            migrationBuilder.DropTable(
                name: "ChurchLifeActivity");

            migrationBuilder.DropTable(
                name: "LeaderRole");

            migrationBuilder.DropTable(
                name: "LeaderRoleCategory");
        }
    }
}
