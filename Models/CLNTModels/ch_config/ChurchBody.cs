﻿// using RhemaCMS.Models.CLNTModels; 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RhemaCMS.Models.CLNTModels
{
    public partial class ChurchBody
    {
        public ChurchBody()
        { }

        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Secondary keys from Vendor CB
        /// 

        public int? MSTR_AppGlobalOwnerId { get; set; }  // == MSTR_AppGlobalOwner.Id   ... central AGO
        public int? MSTR_ChurchBodyId { get; set; }  // == MSTR_ChurchBody.Id   ... central CB
        public int? MSTR_ParentChurchBodyId { get; set; }  // == MSTR_ChurchBody.Id   ... central CB
        public int? MSTR_ChurchLevelId { get; set; }    // == MSTR_ChurchLevel.Id ... local copy

        /// 
        /// </summary>


        public int? AppGlobalOwnerId { get; set; }   // local copy synced

        public int? ChurchLevelId { get; set; }    // local copy synced

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Display(Name = "Church Code")]   // READ-ONLY --- GlobalChurchCode ::>>  [ RootChurchCode /Acronym ] - [7-digit] ... RCM0000000, RCM0000001, PCG1234567, COP1000000, ICGC9999999
        [StringLength(20)]
        public string GlobalChurchCode { get; set; }

        [Display(Name = "Relative Church Code")]  
        [StringLength(200)]
        public string RootChurchCode { get; set; }  // READ ONLY --- ChurchCodeFullPath ::>> RCM-000001--RCM-000001--RCM-000001--RCM-000001

        //[Display(Name = "Custom Code")]
        //[StringLength(10)]
        //public string ChurchCodeCustom { get; set; }


        //[StringLength(50)]
        //public string ChurchGlobalName { get; set; }

        //public DateTime? Formed { get; set; }
        //public DateTime? Innaug { get; set; }

        //[StringLength(2)]
        //public string ChurchType { get; set; }  // to be deleted later

        [StringLength(2)]
        public string OrganisationType { get; set; }    // Church Root, [ GB--Government Body ], CU--Congregation Head-unit, CN--Congregation

        [Display(Name = "Subscription key")]  // may have to be encrypted
        [StringLength(50)]
        public string SubscriptionKey { get; set; }

        public int? ParentChurchBodyId { get; set; }
        public int? ContactInfoId { get; set; }

        //[StringLength(500)]
        //public string BriefHistory { get; set; }

       // public int? CountryId { get; set; }

        [StringLength(3)]
        public string  CtryAlpha3Code { get; set; }
        public int? CountryRegionId { get; set; }
        // public double? ChurchUnitIndex { get; set; }

        [StringLength(200)]
        public string Comments { get; set; }

        //public bool? IsActivated { get; set; }

        // public string ChurchUnitLogo { get; set; }

        // public int? OwnedByChurchBodyId { get; set; }
        //[StringLength(1)]
        //public string SharingStatus { get; set; }
        //public int? ChurchUnitTypeId { get; set; }

        //[StringLength(1)]
        //public string ChurchWorkStatus { get; set; }   // Operationalized - O, Structure only - S

        [StringLength(1)]
        public string Status { get; set; } // A - Active, B-Blocked, C-Closed

        //[StringLength(1)]
        //public string GenderStatus { get; set; }
        //public string GroupLogo { get; set; }
        //public bool? IsUnitGenerational { get; set; }
        //public int? UnitMaxAge { get; set; }
        //public int? UnitMinAge { get; set; }
        //public bool IsFullAutonomy { get; set; }
        //public int? SupervisedByChurchBodyId { get; set; }
        //[StringLength(30)]
        //public string TagName { get; set; }




        /// <summary>
        /// Perculiar to Client .....
        /// </summary>



        //public int? AppGlobalOwnerId { get; set; }
        //public int? ChurchLevelId { get; set; }

        //[Display(Name = "Church Code")]   // GlobalChurchCode ::>>  [ RootChurchCode /Acronym ] - [7-digit] ... RCM0000000, RCM0000001, PCG1234567, COP1000000, ICGC9999999
        //[StringLength(20)]
        //public string GlobalChurchCode { get; set; }

        //[Display(Name = "Relative Church Code")]
        //[StringLength(150)]
        //public string RootChurchCode { get; set; }  // ChurchCodeFullPath ::>> RCM-000001--RCM-000001--RCM-000001--RCM-000001

        [Display(Name = "Custom Code")]
        [StringLength(20)]
        public string ChurchCodeCustom { get; set; }

        //[Required]
        //[StringLength(100)]
        //public string Name { get; set; }

        //[StringLength(50)]
        //public string ChurchGlobalName { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d-MMM-yyyy}", ApplyFormatInEditMode = false)]
        public DateTime? Formed { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d-MMM-yyyy}", ApplyFormatInEditMode = false)]
        public DateTime? Innaug { get; set; }

        [StringLength(2)]
        public string ChurchType { get; set; }  // to be deleted later

        //public int? ParentChurchBodyId { get; set; }
        //public int? ContactInfoId { get; set; }

        [StringLength(500)]
        public string BriefHistory { get; set; }

        //public int? CountryId { get; set; }
        //public int? CountryRegionId { get; set; }        

        public double? ChurchUnitIndex { get; set; }

        [StringLength(200)]
        public string Description { get; set; }

        public bool? IsActivated { get; set; }
        //[StringLength(2)]
        //public string OrganisationType { get; set; }
        public int? OwnedByChurchBodyId { get; set; }

        [StringLength(1)]
        public string SharingStatus { get; set; }
       // public int? ChurchUnitTypeId { get; set; }

        [StringLength(1)]
        public string ChurchWorkStatus { get; set; }   // Operationalized - O, Structure only - S
        //[StringLength(1)]
        //public string Status { get; set; }
        [StringLength(1)]
        public string GenderStatus { get; set; }
        public string ChurchUnitLogo { get; set; }

        public bool? IsUnitGenerational { get; set; }   // 
        public int? ChurchUnitMaxAge { get; set; }  // --- CG >=  CS - 0, JY - 12, YPG - 18, YAF - 30, WF/MF - 40   
        public int? ChurchUnitMinAge { get; set; }    // --- CG < CS - 12, JY - 18, YPG - 30, YAF - 40, WF/MF - ??

        public bool IsFullAutonomy { get; set; }   // Preaching Points are not full autonomy... they are supervised
        ///
        public int GradeLevel { get; set; }  // Grade the CP's  ... Minister is higher than Catechist, then Presyter, then Group Presidents...
        public bool ApplyToClergyOnly { get; set; }    // CP :-- Minister, Catechist
        ///
        public int? SupervisedByChurchBodyId { get; set; }

        public int? MaxNumAllowed { get; set; }  // --- CP ... President - 1, Presbyter - 13
        public int? MinNumAllowed { get; set; }    // ---  CP ... President - 1, Presbyter - 5

        [StringLength(50)]
        public string TagName { get; set; }

        [StringLength(200)]
        public string PrimaryFunction { get; set; }   // for committee and related


        //  [StringLength(50)]
        //public string SubscriptionKey { get; set; }
        //public DateTime? Created { get; set; }
        //public DateTime? LastMod { get; set; }
        //public int? CreatedByUserId { get; set; }
        //public int? LastModByUserId { get; set; }
         
        [StringLength(1)]
        public string OwnershipStatus { get; set; }  // I -- Inherited, O -- Originated   i.e. currChurchBody == OwnedByChurchBody

        public DateTime? Created { get; set; }
        public DateTime? LastMod { get; set; }
        public int? CreatedByUserId { get; set; }
        public int? LastModByUserId { get; set; }






        [NotMapped] //[ForeignKey(nameof(CreatedByUserId))]
        public virtual MSTRModels.UserProfile CreatedByUser { get; set; }
        [NotMapped] // [ForeignKey(nameof(LastModByUserId))]
        public virtual MSTRModels.UserProfile LastModByUser { get; set; }


        /// <summary>
        /// Secondary keys from Vendor CB
        /// 
         
        [NotMapped] // [ForeignKey(nameof(MSTR_AppGlobalOwnerId))]
        public virtual MSTRModels.MSTRAppGlobalOwner MSTRAppGlobalOwner { get; set; } 

        [NotMapped] // [ForeignKey(nameof(MSTR_ChurchBodyId))]
        public virtual MSTRModels.MSTRChurchBody MSTRChurchBody { get; set; }

        [NotMapped] // [ForeignKey(nameof(MSTR_ChurchBodyId))]
        public virtual MSTRModels.MSTRChurchBody MSTRParentChurchBody { get; set; }

        [NotMapped] // [ForeignKey(nameof(MSTR_ChurchLevelId))]
        public virtual MSTRModels.MSTRChurchLevel MSTRChurchLevel { get; set; }

        /// 
        /// </summary>


        [ForeignKey(nameof(AppGlobalOwnerId))]
        public virtual AppGlobalOwner AppGlobalOwner { get; set; }


        [ForeignKey(nameof(ChurchLevelId))]
        //[InverseProperty("OwnedByChurchBody_CL")]
        public virtual ChurchLevel ChurchLevel { get; set; }

        [ForeignKey(nameof(ContactInfoId))]
        public virtual ContactInfo ContactInfo { get; set; }

        [ForeignKey(nameof(CtryAlpha3Code))]
        public virtual Country Country { get; set; }


        [ForeignKey(nameof(CountryRegionId))]
        public virtual CountryRegion CountryRegion { get; set; }




        //public virtual List<AppUtilityNVP> AppUtilityNVPs { get; set; } 

        //[InverseProperty("OwnedByChurchBody")]
        //public virtual List<ChurchLevel> ChurchLevels { get; set; } 
        //public virtual List<ClientAppServerConfig> ClientAppServerConfigs { get; set; } 
        //public virtual List<ContactInfo> ContactInfos { get; set; } 
        //public virtual List<Country> Countries { get; set; } 
        //public virtual List<CountryRegion> CountryRegions { get; set; } 
        //public virtual List<SubscriptionChurchBody> SubscriptionChurchBodies { get; set; } 
        //public virtual List<UserGroup> UserGroups { get; set; } 
        //public virtual List<UserGroupPermission> UserGroupPermissions { get; set; } 
        //public virtual List<UserPermission> UserPermissions { get; set; } 
        //public virtual List<UserProfile> UserProfiles { get; set; } 
        //public virtual List<UserProfileGroup> UserProfileGroups { get; set; } 
        //public virtual List<UserProfileRole> UserProfileRoles { get; set; } 
        //public virtual List<UserRole> UserRoles { get; set; } 
        //public virtual List<UserRolePermission> UserRolePermissions { get; set; }


        [NotMapped][ForeignKey(nameof(ParentChurchBodyId))]
        public virtual ChurchBody ParentChurchBody { get; set; }


        [NotMapped]
        public virtual List<ChurchBody> SubChurchUnits { get; set; }



        //[ForeignKey(nameof(OwnedByChurchBodyId))]
        //public virtual ChurchBody OwnedByChurchBody { get; set; }


        //[NotMapped]
        //public virtual List<ChurchBody> GoverningBodies { get; set; }
        //[NotMapped]
        //public virtual List<ChurchBody> ChurchDepartments { get; set; }
        //[NotMapped]
        //public virtual List<ChurchBody> ChurchOffices { get; set; }
        //[NotMapped]
        //public virtual List<ChurchBody> ChurchGroupings { get; set; }
        //[NotMapped]
        //public virtual List<ChurchBody> StandingCommittees { get; set; }
        //[NotMapped]
        //public virtual List<ChurchBody> ChurchEnterprises { get; set; }
        //[NotMapped]
        //public virtual List<ChurchBody> Teams { get; set; }
        //[NotMapped]
        //public virtual List<ChurchBody> ChurchPositions { get; set; }
        //[NotMapped]
        //public virtual List<ChurchBody> IndependentChurchUnits { get; set; }
        //[NotMapped]
        //public virtual List<ChurchBody> Congregations { get; set; }   // i.e. CF's


        //[NotMapped]
        //public string strChurchUnit { get; set; }
        //[NotMapped]
        //public string strCH_InCharge { get; set; }

        //[NotMapped]
        //public MemberChurchRole oCH_InChargeMLR { get; set; }

        //[NotMapped]
        //public virtual List<ChurchBody> SubChurchUnits_CHCF { get; set; } // of CH or CF type  ... where members are

        //[NotMapped] 
        //public virtual List<ChurchBody> ChurchMembers_CHCF { get; set; } // of CH or CF type  ... where members are

        //[NotMapped]
        //public long CH_TotSubUnits { get; set; }

        //[NotMapped]
        //public List<ChurchMember> CH_TotMemList { get; set; }

        //[NotMapped]
        //public long CH_TotMem { get; set; }
        //[NotMapped]
        //public long CH_TotNewMem { get; set; }
        //[NotMapped]
        //public long CH_TotMaleMem { get; set; }
        //[NotMapped]
        //public long CH_TotFemMem { get; set; }
        //[NotMapped]
        //public long CH_TotOtherMem { get; set; }


        //[NotMapped]
        //public List<ChurchVisitor> CH_TotMemList_Visitor { get; set; }

        //[NotMapped]
        //public List<ChurchAttendAttendees> CH_TotList_Attendees { get; set; }

        //[NotMapped]
        //public List<ChurchAttendHeadCount> CH_TotList_Headcount { get; set; }




        //[NotMapped]
        //public string strOrgType { get; set; }
        //[NotMapped]
        //public string strChurchUnitType { get; set; }
        //[NotMapped]
        //public string strParentUnit { get; set; }
        //[NotMapped]
        //public string strChurchLevel { get; set; }

        //[NotMapped]
        //public int intChurchLevel_Index { get; set; }

        //[NotMapped]
        //public string strChurchWorkStatus { get; set; }

        [NotMapped]
        public string strStatus { get; set; }

        //
        [NotMapped]
        public string strAppGlobalOwn { get; set; }

        //[NotMapped]
        //public string strAssociationType { get; set; }
        //[NotMapped]
        //public string strChurchType { get; set; }

        [NotMapped]
        public string strParentChurchBody { get; set; }

        [NotMapped]
        public string strCountry { get; set; }
        [NotMapped]
        public string strCountryRegion { get; set; }
        [NotMapped]
        public string strContactDetail { get; set; }

          [NotMapped]
        public string strFaithTypeCategory { get; set; }   // get the faith category stuff from AGO

         




        /// <summary>
        /// /////////////////////////
        /// /////////////////////////
        /// </summary>




      


        //[NotMapped] //[ForeignKey(nameof(CreatedByUserId))]
        //public virtual UserProfile CreatedByUser { get; set; }
        //[NotMapped] // [ForeignKey(nameof(LastModByUserId))]
        //public virtual UserProfile LastModByUser { get; set; }


        //// [NotMapped]
        //[ForeignKey(nameof(AppGlobalOwnerId))]
        //public virtual AppGlobalOwner AppGlobalOwner { get; set; }


       // [ForeignKey(nameof(ChurchLevelId))]
       //// [InverseProperty("OwnedByChurchBody_CL")]
       // public virtual ChurchLevel ChurchLevel { get; set; }

        //[ForeignKey(nameof(ContactInfoId))]
        //public virtual ContactInfo ContactInfo { get; set; }
        //[ForeignKey(nameof(CountryId))]
        //public virtual Country Country { get; set; }
        //[ForeignKey(nameof(CountryRegionId))]
        //public virtual CountryRegion CountryRegion { get; set; }




        //public virtual List<AppUtilityNVP> AppUtilityNVPs { get; set; } 

        //[InverseProperty("OwnedByChurchBody")]
        //public virtual List<ChurchLevel> ChurchLevels { get; set; } 
        //public virtual List<ClientAppServerConfig> ClientAppServerConfigs { get; set; } 
        //public virtual List<ContactInfo> ContactInfos { get; set; } 
        //public virtual List<Country> Countries { get; set; } 
        //public virtual List<CountryRegion> CountryRegions { get; set; } 
        //public virtual List<SubscriptionChurchBody> SubscriptionChurchBodies { get; set; } 
        //public virtual List<UserGroup> UserGroups { get; set; } 
        //public virtual List<UserGroupPermission> UserGroupPermissions { get; set; } 
        //public virtual List<UserPermission> UserPermissions { get; set; } 
        //public virtual List<UserProfile> UserProfiles { get; set; } 
        //public virtual List<UserProfileGroup> UserProfileGroups { get; set; } 
        //public virtual List<UserProfileRole> UserProfileRoles { get; set; } 
        //public virtual List<UserRole> UserRoles { get; set; } 
        //public virtual List<UserRolePermission> UserRolePermissions { get; set; }

        

        //[NotMapped] // [ForeignKey(nameof(ParentChurchBodyId))]
        //public virtual ChurchBody ParentChurchBody { get; set; }

        //public virtual List<ChurchBody> SubChurchUnits { get; set; }


        [ForeignKey(nameof(OwnedByChurchBodyId))]
        public virtual ChurchBody OwnedByChurchBody { get; set; }


        [NotMapped]
        public virtual List<ChurchBody> GoverningBodies { get; set; }
        [NotMapped]
        public virtual List<ChurchBody> ChurchDepartments { get; set; }
        [NotMapped]
        public virtual List<ChurchBody> ChurchOffices { get; set; }
        [NotMapped]
        public virtual List<ChurchBody> ChurchGroupings { get; set; }
        [NotMapped]
        public virtual List<ChurchBody> StandingCommittees { get; set; }
        [NotMapped]
        public virtual List<ChurchBody> ChurchEnterprises { get; set; }
        [NotMapped]
        public virtual List<ChurchBody> Teams { get; set; }
        [NotMapped]
        public virtual List<ChurchBody> ChurchPositions { get; set; }
        [NotMapped]
        public virtual List<ChurchBody> IndependentChurchUnits { get; set; }
        [NotMapped]
        public virtual List<ChurchBody> Congregations { get; set; }   // i.e. CF's


        [NotMapped]
        public string strChurchUnit { get; set; }
        [NotMapped]
        public string strCH_InCharge { get; set; }
        [NotMapped]
        public MemberChurchRole oCH_InChargeMLR { get; set; }
        //[NotMapped]
        //public virtual List<ChurchBody> SubChurchUnits_CHCF { get; set; } // of CH or CF type  ... where members are

        //[NotMapped] 
        //public virtual List<ChurchBody> ChurchMembers_CHCF { get; set; } // of CH or CF type  ... where members are

        [NotMapped]
        public long CH_TotSubUnits { get; set; }

        [NotMapped]
        public List<ChurchMember> CH_TotMemList { get; set; }

        [NotMapped]
        public long CH_TotMem { get; set; }
        [NotMapped]
        public long CH_TotNewMem { get; set; }
        [NotMapped]
        public long CH_TotMaleMem { get; set; }
        [NotMapped]
        public long CH_TotFemMem { get; set; }
        [NotMapped]
        public long CH_TotOtherMem { get; set; }


        [NotMapped]
        public List<ChurchVisitor> CH_TotMemList_Visitor { get; set; }

        [NotMapped]
        public List<ChurchAttendAttendees> CH_TotList_Attendees { get; set; }

        [NotMapped]
        public List<ChurchAttendHeadCount> CH_TotList_Headcount { get; set; }




        [NotMapped]
        public string strOrgType { get; set; }
        [NotMapped]
        public string strChurchUnitType { get; set; }
        [NotMapped]
        public string strParentUnit { get; set; }
        [NotMapped]
        public string strChurchLevel { get; set; }

        [NotMapped]
        public int numChurchLevel_Index { get; set; }

        [NotMapped]
        public string strChurchWorkStatus { get; set; }
        //[NotMapped]
        //public string strStatus { get; set; }

        //
        //[NotMapped]
        //public string strAppGlobalOwn { get; set; }
        [NotMapped]
        public string strAssociationType { get; set; }
        [NotMapped]
        public string strChurchType { get; set; }
        //[NotMapped]
        //public string strParentChurchBody { get; set; }
        //[NotMapped]
        //public string strCountry { get; set; }
        //[NotMapped]
        //public string strCountryRegion { get; set; }
        //[NotMapped]
        //public string strContactDetail { get; set; }

        //
        [NotMapped]
        public string strSharingStatus { get; set; }
      
        [NotMapped]
        public string strOwnershipStatus { get; set; }   // I -- Inherited, O -- Originated   i.e. currChurchBody == OwnedByChurchBody
        [NotMapped]
        public string strOwnerChurchBody { get; set; }

         

    }
}
