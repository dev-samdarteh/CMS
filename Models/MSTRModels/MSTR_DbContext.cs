using System;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration; 

namespace RhemaCMS.Models.MSTRModels
{
    public partial class MSTR_DbContext : DbContext
    {
       private readonly IConfiguration _configuration;
       private readonly string _connectionString;

       // private IDbConnection DbConnection { get; }

        public MSTR_DbContext() 
        {}

        public MSTR_DbContext(DbContextOptions<MSTR_DbContext> options)  : base(options)
        { }

        public MSTR_DbContext(DbContextOptions<MSTR_DbContext> options, IConfiguration configuration) : base(options)
        {
            if (configuration != null)
                _configuration = configuration;

            //, IConfiguration configuration
            //this._configuration = configuration;
            //DbConnection = new SqlConnection(this._configuration.GetConnectionString("Connection1"));
        }
        public MSTR_DbContext(string connectionString)  
        {
            this._connectionString = connectionString;
        }
         

        public virtual DbSet<MSTRAppGlobalOwner> MSTRAppGlobalOwner { get; set; }
        public virtual DbSet<AppSubscription> AppSubscription { get; set; }
        public virtual DbSet<AppSubscriptionPackage> AppSubscriptionPackage { get; set; }
       public virtual DbSet<SubscriptionChurchBody> SubscriptionChurchBody { get; set; }

        public virtual DbSet<MSTRAppUtilityNVP> AppUtilityNVP { get; set; }        
        public virtual DbSet<ChurchFaithType> ChurchFaithType { get; set; }
        public virtual DbSet<MSTRChurchLevel> MSTRChurchLevel { get; set; }
        public virtual DbSet<MSTRChurchBody> MSTRChurchBody { get; set; }
        public virtual DbSet<ClientAppServerConfig> ClientAppServerConfig { get; set; }
        public virtual DbSet<MSTRContactInfo> MSTRContactInfo { get; set; }
        public virtual DbSet<MSTRCountry> MSTRCountry { get; set; }
        public virtual DbSet<MSTRCountryCustom> MSTRCountryCustom { get; set; }
        public virtual DbSet<MSTRCountryRegion> MSTRCountryRegion { get; set; }       
        public virtual DbSet<UserAuditTrail> UserAuditTrail { get; set; }
        public virtual DbSet<UserGroup> UserGroup { get; set; }
        public virtual DbSet<UserGroupPermission> UserGroupPermission { get; set; }
        public virtual DbSet<UserPermission> UserPermission { get; set; }
        public virtual DbSet<UserProfile> UserProfile { get; set; }
        public virtual DbSet<UserProfileGroup> UserProfileGroup { get; set; }
        public virtual DbSet<UserProfileRole> UserProfileRole { get; set; }
        public virtual DbSet<UserRole> UserRole { get; set; }
        public virtual DbSet<UserRolePermission> UserRolePermission { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                #warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
              
               // optionsBuilder.UseSqlServer("Server=RHEMA-SDARTEH;Database=DBRCMS_MS_DEV;User Id=sa;Password=sadmin;Trusted_Connection=True;MultipleActiveResultSets=true");
                ///
                //  optionsBuilder.UseSqlServer("Server=CLSVR-01;Database=DBRCMS_MS_TEST;User Id=sa;Password=sa;Trusted_Connection=True;MultipleActiveResultSets=true");   // PROD

                ///
                //  optionsBuilder.UseSqlServer("Server=RHEMA-SDARTEH;Database=DBRCMS_MS_TEST;User Id=sa;Password=sadmin;Trusted_Connection=True;MultipleActiveResultSets=true");  // DEV

                //if (this._configuration != null)
                //   optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));

                // optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DefaultConnection_CLNT"));


                if (!string.IsNullOrEmpty(_connectionString))
                {
                    optionsBuilder.UseSqlServer(_connectionString);
                   // base.OnConfiguring(optionsBuilder);
                }
                else
                {
                    if (_configuration != null)
                        optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DefaultConnection")); 
                } 
            }
        }

        //public DbSet<RhemaCMS.Models.CLNTModels.ChurchMember> ChurchMember { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{

        //}

        //partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
