using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using RhemaCMS.Models.Adhoc;
using RhemaCMS.Models.CLNTModels;
//using RhemaCMS.Models.MSTRModels;

namespace RhemaCMS.Models.CLNTModels
{
    public partial class ChurchModelContext : DbContext
    {

        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public ChurchModelContext()
        { }

        public ChurchModelContext(DbContextOptions<ChurchModelContext> options)
            : base(options)
        { }

        public ChurchModelContext(DbContextOptions<ChurchModelContext> options, IConfiguration configuration) : base(options)
        {
            if (configuration != null)
                _configuration = configuration;

            //, IConfiguration configuration
            //this._configuration = configuration;
            //DbConnection = new SqlConnection(this._configuration.GetConnectionString("Connection1"));
        }
        public ChurchModelContext(string connectionString)
        {
            this._connectionString = connectionString;
        }


        /// General setup --- lookups
        public virtual DbSet<UserAuditTrail_CL> UserAuditTrail_CL { get; set; }
        public virtual DbSet<AppGlobalOwner> AppGlobalOwner { get; set; }
        public virtual DbSet<ChurchBody> ChurchBody { get; set; } 
        public virtual DbSet<Country> Country { get; set; }
        public virtual DbSet<CountryCustom> CountryCustom { get; set; }
        public virtual DbSet<CountryRegion> CountryRegion { get; set; }
        public virtual DbSet<CountryRegionCustom> CountryRegionCustom { get; set; }
        public virtual DbSet<ChurchLevel> ChurchLevel { get; set; }
        public virtual DbSet<Currency> Currency { get; set; }           
        public virtual DbSet<CurrencyCustom> CurrencyCustom { get; set; }           
        public virtual DbSet<CertificateType> CertificateType { get; set; }
        public virtual DbSet<ContactInfo> ContactInfo { get; set; }
        public virtual DbSet<AppUtilityNVP> AppUtilityNVP { get; set; }
        public virtual DbSet<InstitutionType> InstitutionType { get; set; }
        public virtual DbSet<LanguageSpoken> LanguageSpoken { get; set; }
        public virtual DbSet<LanguageSpokenCustom> LanguageSpokenCustom { get; set; }
        public virtual DbSet<ChurchPeriod> ChurchPeriod { get; set; }
        public virtual DbSet<AccountPeriod> AccountPeriod { get; set; }
        public virtual DbSet<National_IdType> National_IdType { get; set; }
        public virtual DbSet<RelationshipType> RelationshipType { get; set; }   
        public virtual DbSet<ChurchSector> ChurchSectorUnit { get; set; }
        public virtual DbSet<ChurchRole> ChurchRole { get; set; }   // pairings done. // Church Position | Team // with Church sector -- define positions within the sector.
        public virtual DbSet<ChurchMemStatus> ChurchMemStatus { get; set; } // Regular - Distant - Passed etc
        public virtual DbSet<ChurchMemType> ChurchMemType { get; set; } // Member - Minister
        public virtual DbSet<ChurchRank> ChurchRank { get; set; }   // Member - Presbyter - Min Probationer - Minister - Senior Minister
        public virtual DbSet<ChurchLifeActivity> ChurchLifeActivity { get; set; }
        public virtual DbSet<ChurchLifeActivityReqDef> ChurchLifeActivityReqDef { get; set; }
        


        /// member related ----------------
        /// 
       // public virtual DbSet<ChurchBodyAssociate> ChurchBodyAssociate { get; set; }   // external members linked to the church body
        public virtual DbSet<ChurchMember> ChurchMember { get; set; }
        public virtual DbSet<MemberChurchLife> MemberChurchLife { get; set; }
        public virtual DbSet<MemberChurchLifeActivity> MemberChurchLifeActivity { get; set; }
        public virtual DbSet<MemberChurchSector> MemberChurchSector { get; set; }            //public virtual DbSet<MemberChurchUnit> MemberChurchUnit { get; set; }
        public virtual DbSet<MemberContact> MemberContact { get; set; }
        public virtual DbSet<MemberEducHistory> MemberEducHistory { get; set; }
        public virtual DbSet<MemberLanguageSpoken> MemberLanguageSpoken { get; set; }
        public virtual DbSet<MemberChurchRole> MemberChurchRole { get; set; }         //public virtual DbSet<MemberPosition> MemberPosition { get; set; }
        public virtual DbSet<MemberProfessionBrand> MemberProfessionBrand { get; set; }
        public virtual DbSet<MemberRank> MemberRank { get; set; }
        public virtual DbSet<MemberRegistration> MemberRegistration { get; set; }
        public virtual DbSet<MemberRelation> MemberRelation { get; set; }
        public virtual DbSet<MemberStatus> MemberStatus { get; set; }
        public virtual DbSet<MemberType> MemberType { get; set; }
        public virtual DbSet<MemberWorkExperience> MemberWorkExperience { get; set; }
        public virtual DbSet<ChurchVisitor> ChurchVisitor { get; set; }
        public virtual DbSet<ChurchVisitorType> ChurchVisitorType { get; set; }


        /// Church-life module --------
        ///
        public virtual DbSet<ChurchBodyService> ChurchBodyService { get; set; }
        public virtual DbSet<ChurchCalendarEvent> ChurchCalendarEvent { get; set; }
        public virtual DbSet<EventActivityReqLog> EventActivityReqLog { get; set; }         
        public virtual DbSet<ChurchEventCategory> ChurchEventCategory { get; set; }    
        //public virtual DbSet<ChurchEventActor> ChurchEventActor { get; set; }



        //  public virtual DbSet<ApprovalAction> ApprovalAction { get; set; }
        //  public virtual DbSet<ApprovalActionStep> ApprovalActionStep { get; set; }
        //  public virtual DbSet<ApprovalProcess> ApprovalProcess { get; set; }
        //  public virtual DbSet<ApprovalProcessStep> ApprovalProcessStep { get; set; }
        //  public virtual DbSet<AssetCategory> AssetCategory { get; set; }
        //  public virtual DbSet<ChurchAsset> ChurchAsset { get; set; }
        //  public virtual DbSet<ChurchAssetStock> ChurchAssetStock { get; set; }
        //  public virtual DbSet<ChurchAssociate> ChurchAssociate { get; set; }
        //  public virtual DbSet<ChurchAttendAttendees> ChurchAttendAttendees { get; set; }
        //  public virtual DbSet<ChurchAttendHeadCount> ChurchAttendHeadCount { get; set; }
        //  public virtual DbSet<ChurchAttendance> ChurchAttendance { get; set; }
         
        //  public virtual DbSet<ChurchTransfer> ChurchTransfer { get; set; }
        //  public virtual DbSet<ChurchTransferDesignation> ChurchTransferDesignation { get; set; }  
        //  public virtual DbSet<TransferTypeChurchLevel> TransferTypeChurchLevel { get; set; }


        //  public virtual DbSet<AppSubscription> AppSubscription { get; set; } ************
        //  public virtual DbSet<SubscriptionChurchBody> SubscriptionChurchBody { get; set; }*************
        //  public virtual DbSet<AppSubscriptionPackage> AppSubscriptionPackage { get; set; }*************

        //  public virtual DbSet<ChurchFaithType> ChurchFaithType { get; set; }********
        //  public virtual DbSet<ChurchUnitType> ChurchUnitType { get; set; }   **********  
        //  public virtual DbSet<ChurchDivision> ChurchDivision { get; set; }***********
        //  public virtual DbSet<ChurchDivisionType> ChurchDivisionType { get; set; }**********
        //  public virtual DbSet<ChurchPosition> ChurchPosition { get; set; }**********
        //  public virtual DbSet<ChurchSector> ChurchSector { get; set; }***********
        //  public virtual DbSet<ChurchSectorCategory> ChurchSectorCategory { get; set; }**********
        //  public virtual DbSet<LeaderRole> LeaderRole { get; set; }******
        //  public virtual DbSet<LeaderRoleCategory> LeaderRoleCategory { get; set; }*********
        //  public virtual DbSet<SectorLeaderRole> SectorLeaderRole { get; set; }*********
        //  public virtual DbSet<ActivityPeriod> ActivityPeriod { get; set; }*****
        //  public virtual DbSet<AccountYear> AccountYear { get; set; }******


        /// finance-module -----------
        /// 
        //  public virtual DbSet<AccountType> AccountType { get; set; }
        //  public virtual DbSet<AppControlAccount> AppControlAccount { get; set; }
        //  public virtual DbSet<AccountTypeCategory> AccountTypeCategory { get; set; }
        //  public virtual DbSet<AccountCategory> AccountCategory { get; set; }
        //  public virtual DbSet<AccountMaster> AccountMaster { get; set; }   
        //  public virtual DbSet<TransControlAccount> TransControlAccount { get; set; }
        //  public virtual DbSet<AccountBalance> AccountBalance { get; set; }
        //  public virtual DbSet<AccountTrans> AccountTrans { get; set; }
        //  public virtual DbSet<TitheTrans> TitheTrans { get; set; }
        //  public virtual DbSet<TitheTransDetail> TitheTransDetail { get; set; }
        //  public virtual DbSet<OffertoryTrans> OffertoryTrans { get; set; }
        //  public virtual DbSet<OffertoryTransDetail> OffertoryTransDetail { get; set; }        
        //  public virtual DbSet<DonationTrans> DonationTrans { get; set; }




        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                #warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                //   optionsBuilder.UseSqlServer("Server=RHEMA-SDARTEH;Database=DBRCMS_CL_TEST;User Id=sa;Password=sadmin;Trusted_Connection=True;MultipleActiveResultSets=true");

                if (!string.IsNullOrEmpty(_connectionString))
                {
                    optionsBuilder.UseSqlServer(_connectionString);
                    base.OnConfiguring(optionsBuilder);
                }
                else
                {
                    if (_configuration != null)
                        optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DefaultConnection_CLNT"));
                }

            }
        }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{ 

        //    OnModelCreatingPartial(modelBuilder);
        //}

        //partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
