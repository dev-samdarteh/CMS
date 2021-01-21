//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using RhemaCMS.Models.Adhoc;
// using RhemaCMS.Models.CLNTModels;
//using RhemaCMS.Models.MSTRModels;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;

//namespace RhemaCMS.Models.ViewModels.vm_app_ven
//{
//    public class ChurchBodyConfigMDL
//    {
//        public ChurchBodyConfigMDL() { }

//        public AppGlobalOwner oCurrAppGlobalOwner { get; set; } // pcg
//        public ChurchBody oCurrChurchBody { get; set; }  // grace
//        public ChurchMember oCurrLoggedMember { get; set; }
//        public int? oCurrLoggedMemberId { get; set; }
//        public int  setIndex  { get; set; }
//        public string strCurrTask { get; set; }


//        /* Service Provider Tasks */ 
//        public List<ChurchFaithType_MDL> lsChurchFaithTypes { get; set; }
//        public AppGlobalOwner_MDL oAppGlobalOwn_MDL { get; set; } //public List<AppGlobalOwner_MDL> oAppGlobalOwn_MDL { get; set; } 
//        public ChurchLevel_MDL oChurchLevel_MDL { get; set; }
//        public  ChurchBody_MDL oChurchBody_MDL { get; set; }  //public List<ChurchBody_MDL> lsCongregations { get; set; } 
//        public List<Country_MDL> lsCountries { get; set; }
//        public List<CountryRegion_MDL> lsCountryRegions { get; set; }
//        public List<ChurchLevel_MDL> lsChurchLevels { get; set; } 
//        public List<AppSubscription_MDL> lsAppSubscriptions { get; set; } 
//        public List<UserProfile_MDL> lsUserProfiles { get; set; } 


//        /* CB ? configuration */
//        //public AppGlobalOwner oAppGlobalOwner { get; set; } // pcg
//        //public ChurchBody oChurchBody { get; set; }  // grace
//        public string strChurchLevel {  get;set;}  // congregation
//        public string strChurchCategory { get; set; }  // ramseyer, taifa 
//        public string strChurchCategoryLevel { get; set; }  // district, presbytery
//        public string strCongregationType { get; set; }  //networked, freelance


//        /* Church life configuration */
//        public List<ChurchBodyService> lsChurchBodyServices { get; set; } // 1st service, 2nd service, communion service
//        public ChurchBodyService oChurchBodyService{ get; set; }
//        public List<ChurchPosition> lsChurchPositions { get; set; } // 
//        public ChurchPosition oChurchPositions { get; set; } // 
//        public List<ChurchMemStatus> lsChurchMemStatuses { get; set; } // 
//        public ChurchMemStatus oChurchMemStatus { get; set; } // 
//        public List<LeaderRole> lsLeaderRole { get; set; } // 
//        public LeaderRole oLeaderRoles { get; set; } // 
//        public List<ChurchSector> lsChurchSectors { get; set; } //  
//        public ChurchSector oChurchSector { get; set; } // 
//        public List<SectorLeaderRole> lsSectorLeaderRoles { get; set; } // 
//        public SectorLeaderRole oSectorLeaderRole { get; set; } // 
//        public List<ChurchSectorCategory> lsChurchSectorCategories { get; set; } // 
//        public ChurchSectorCategory oChurchSectorCategory { get; set; } // 
//        public List<LeaderRoleCategory> lsLeaderRoleCategories { get; set; } // 
//        public LeaderRoleCategory oLeaderRoleCategory { get; set; } // 
//        public List<ChurchLifeActivity> lsChurchLifeActivities { get; set; } //
//        public ChurchLifeActivity oChurchLifeActivity { get; set; } // 
//        public List<ChurchLifeActivityReqDef> lsChurchLifeActivityReqDefs { get; set; } // 
//        public ChurchLifeActivityReqDef oChurchLifeActivityReqDef { get; set; } // 

//        //public List<EventActivityReqDetail> oLeaderRoleCategories { get; set; } // 
//        //public List<EventActivityReqLog> oLeaderRoleCategories { get; set; } // 
//        //public List<EventReqMemberHeader> oLeaderRoleCategories { get; set; } // 
//        //public List<LeaderRoleCategory> oLeaderRoleCategories { get; set; } // 


//        /* General configuration */

//    }

//    public class ChurchFaithType_MDL
//    {
//        public MSTRAppGlobalOwner oCurrAppGlobalOwner { get; set; } // pcg
//        public ChurchMember oCurrLoggedMember { get; set; }
//        public int? oCurrLoggedMemberId { get; set; }
//        public MSTRChurchBody oChurchBody { get; set; }
//        public int setIndex { get; set; }
//        //
//        public List<ChurchFaithType> lsChurchFaithTypes { get; set; }
//        public ChurchFaithType oChurchFaithType { get; set; }
//        public List<SelectListItem> lkpFaithTypeClasses { set; get; }
//        public string strFaithTypeClass { get; set; } 

//    }
//    public class AppGlobalOwner_MDL
//    {
//        public AppGlobalOwner oCurrAppGlobalOwner { get; set; } // pcg... curr logged user
//        public ChurchMember oCurrLoggedMember { get; set; }
//        public int? oCurrLoggedMemberId { get; set; }
//        public ChurchBody oCurrChurchBody { get; set; }
//        public int setIndex { get; set; }
//        //
//        public List<AppGlobalOwner_MDL> lsAppGlobalOwn_MDL { get; set; }  
//        public List<AppGlobalOwner> lsAppGlobalOwn { get; set; }        
//        public AppGlobalOwner_MDL  oAppGlobalOwn_MDL { get; set; } 
//        public AppGlobalOwner oAppGlobalOwn { get; set; }         
//        public string strStatus { get; set; }
//        public string strCountry { get; set; }
//        public string strFaithTypeCategory { get; set; }
//        public string strDenomination { get; set; }

//        [Display(Name = "ChurchLogo")]
//        public IFormFile ChurchLogoFile { get; set; }
//        public string strChurchLogo { get; set; }

//        //
//        public List<ChurchLevel> lsChurchLevels { get; set; }
//        //public ChurchLevel oChurchLevel { get; set; }        
//        public List<SelectListItem> lkpAppGlobalOwn { set; get; }  //public List<SelectListItem> lkpDenominations { set; get; }
//        public List<SelectListItem> lkpStatuses { set; get; }
//        public List<SelectListItem> lkpCountries { set; get; }
//        public List<SelectListItem> lkpFaithTypeCategories { set; get; }
        
//    }
//    public class ChurchLevel_MDL
//    {
//        public AppGlobalOwner oCurrAppGlobalOwner { get; set; } // pcg
//        public ChurchMember oCurrLoggedMember { get; set; }
//        public int? oCurrLoggedMemberId { get; set; }
//        public ChurchBody oCurrChurchBody { get; set; }
//        public int setIndex { get; set; }
//        //
//        public List<ChurchLevel_MDL> lsChurchLevel_MDL { get; set; } // public public List<ChurchLevel> lsChurchLevels { get; set; } 
//        public ChurchLevel_MDL oChurchLevel_MDL { get; set; }
//        public ChurchLevel oChurchLevel { get; set; }         
//        public List<SelectListItem> lkpAppGlobalOwn { set; get; }
//        //public string strChurchBody { get; set; }
//        public string strAppGlobalOwn { get; set; }
//        public int? oCurrAppGloId_Filter4 { get; set; }
//    }
//    public class Country_MDL
//    {
//        public AppGlobalOwner oCurrAppGlobalOwner { get; set; } // pcg... curr logged user
//        public ChurchBody oCurrChurchBody { get; set; }
//        public ChurchMember oCurrLoggedMember { get; set; }
//        public int? oCurrLoggedMemberId { get; set; }        
//        public int setIndex { get; set; }
//        //
//        public List<Country> lsCountries { get; set; }
//        public Country oCountry { get; set; }

//        //
//        public List<CountryRegion> lsCountryRegions { get; set; }
//        public CountryRegion oCountryRegion { get; set; }
//        public List<SelectListItem> lkpCountries { set; get; }
//        public string strCountry { get; set; }
//    }
//    public class CountryRegion_MDL
//    {
//        public ChurchBody oCurrChurchBody { get; set; }
//        public AppGlobalOwner oCurrAppGlobalOwner { get; set; } // pcg... curr logged user
//        public ChurchMember oCurrLoggedMember { get; set; }
//        public int? oCurrLoggedMemberId { get; set; }        
//        public int setIndex { get; set; }
//        //
//        public List<CountryRegion> lsCountryRegions { get; set; }
//        public CountryRegion oCountryRegion { get; set; }

//        //
//        public List<Country> lsCountries { get; set; }
//        public Country oCountry { get; set; }
//        public List<SelectListItem> lkpCountries { set; get; }
//        public string strCountry { get; set; }
//    }
//    public class ChurchBody_MDL
//    {
//        public AppGlobalOwner oCurrAppGlobalOwner { get; set; } // curr logged
//        public int?  oCurrAppGloId_Filter5 { get; set; }
//        public int? oCurrChuCategId_Filter5 { get; set; }
//        public bool oCurrShowAllCong_Filter5 { get; set; }
//        public ChurchBody oCurrChurchBody { get; set; }
//        public ChurchMember oCurrLoggedMember { get; set; }
//        public int? oCurrLoggedMemberId { get; set; }        
//        public int setIndex { get; set; }
//        //
//         public List<ChurchBody_MDL> lsChurchBody_MDL { get; set; } // public public List<ChurchLevel> lsChurchLevels { get; set; } 
//        public ChurchBody_MDL oChurchBody_MDL { get; set; }
//        public ChurchBody oChurchBody { get; set; }
        
//        //         
//        public List<ChurchBody> lsChurchBody { get; set; } 
//        public List<SelectListItem> lkpAppGlobalOwn { set; get; }
//        public List<SelectListItem> lkpChurchBodies { set; get; }
//        public List<SelectListItem> lkpChurchLevels { set; get; }
//        public List<SelectListItem> lkpAssociationTypes { set; get; }
//        public List<SelectListItem> lkpChurchTypes { set; get; }
//        public List<SelectListItem> lkpChurchCategories { set; get; }
//        public List<SelectListItem> lkpCountries { set; get; }
//        public List<SelectListItem> lkpCountryRegions { set; get; }
//        public List<SelectListItem> lkpContactDetails { set; get; }

//        public string strAppGlobalOwn { get; set; }
//        public string strChurchLevel { get; set; }
//        public string strAssociationType { get; set; }
//        public string strChurchType { get; set; }
//        public string strParentChurchBody { get; set; }
//        public string strCountry { get; set; }
//        public string strCountryRegion { get; set; }
//        public string strContactDetail { get; set; }

//        //
//        public List<ChurchBody> lsSubChurchBodies { get; set; }
//        public ChurchBody oSubChurchBody { get; set; }
//     //  public List<SelectListItem> lkpCongregations { set; get; }
//        public string strCongregation { get; set; }
//    }    
//    public class AppSubscription_MDL
//    {
//        public AppGlobalOwner oAppGlobalOwner { get; set; } // pcg  
//        public ChurchBody oCurrChurchBody { get; set; }
//        public ChurchMember oCurrLoggedMember { get; set; }
//        public int? oCurrLoggedMemberId { get; set; }      
//        public int setIndex { get; set; }
//        //       
//        public List<AppSubscription> lsAppSubscriptions { get; set; }
//        public AppSubscription oAppSubscription { get; set; } 

//        public List<SelectListItem> lkpDenominations { set; get; }
//        public List<SelectListItem> lkpCongregations { set; get; }
//        public List<SelectListItem> lkpAppSubscriptionPackages { set; get; }
//        public List<SelectListItem> lkpChurchLevels { set; get; }       
//        public List<SelectListItem> lkpSLAStatuses { set; get; }
//        public string strDenomination { get; set; }
//        public string strCongregation { get; set; }
//        public string strAppSubscriptionPackage { get; set; }
//        public string strChurchLevel { get; set; }        
//        public string strSLAUserStatus { get; set; }
//    }  
//    public class UserProfile_MDL
//    {
//        public AppGlobalOwner oCurrAppGlobalOwner { get; set; } // pcg
//        public ChurchBody oCurrChurchBody { get; set; }
//        public ChurchMember oCurrLoggedMember { get; set; }
//        public int? oCurrLoggedMemberId { get; set; }        
//        public int setIndex { get; set; }

//        //   
//        public List<UserProfile> lsUserProfiles { get; set; }
//        public UserProfile oUserProfile { get; set; }
//        public List<SelectListItem> lkpDenominations { set; get; }
//        public List<SelectListItem> lkpChurchMembers { set; get; }
//        public List<SelectListItem> lkpOwnerUsers { set; get; }
//        public List<SelectListItem> lkpCongregations { set; get; }
//        public List<SelectListItem> lkpTargetCongregations { set; get; }
//        public List<SelectListItem> lkpCongNextCategory { set; get; }
//        public List<SelectListItem> lkpStatuses { set; get; }
//        public List<SelectListItem> lkpUserTypes { set; get; }

//        public string strToChurchLevel { get; set; }
//        public string strToChurchLevel_1 { get; set; }
//        public string strToChurchLevel_2 { get; set; }
//        public string strToChurchLevel_3 { get; set; }
//        public string strToChurchLevel_4 { get; set; }
//        public string strToChurchLevel_5 { get; set; }
//        public string strToChurchLevel_6 { get; set; }
//        public string strToChurchLevel_7 { get; set; }
//        public int? ToChurchBodyId_Categ1 { get; set; }
//        public int? ToChurchBodyId_Categ2 { get; set; }
//        public int? ToChurchBodyId_Categ3 { get; set; }
//        public int? ToChurchBodyId_Categ4 { get; set; }
//        public int? ToChurchBodyId_Categ5 { get; set; }
//        public int? ToChurchBodyId_Categ6 { get; set; }
//        public int? ToChurchBodyId_Categ7 { get; set; }

//        public string strUserProfile { get; set; }
//        public string strDenomination { get; set; }
//        public string strChurchMember { get; set; }
//        public string strOwnerUser { get; set; }
//        public string strCongregation { get; set; }
//        public string strUserStatus { get; set; }

//        [Display(Name = "User Photo")]
//        public IFormFile UserPhotoFile { get; set; }
//        public string strUserPhoto { get; set; }
        

//        // 
//        public List<UserGroup> lsUserGroups { get; set; }
//        public List<UserRole> lsUserRoles { get; set; }
//        public List<UserPermission> lsUserPermissions { get; set; }
//        public List<UserSessionPrivilege> lsUserPrivileges { get; set; }
//        public UserRole oUserRole { get; set; }
//        public UserGroup oUserGroup { get; set; }
//        public UserPermission oUserPermission { get; set; }

//        public UserProfileRole oUserProfileRole { get; set; }
//        public UserProfileGroup oUserProfileGroup { get; set; }
//        public UserGroupPermission oUserGroupPermission { get; set; }
//        public UserRolePermission oUserRolePermission { get; set; }

//        public List<SelectListItem> lkpUserProfiles { get; set; }
//        public List<SelectListItem> lkpUserRoles { set; get; }
//        public List<SelectListItem> lkpUserGroups { set; get; }
//        public List<SelectListItem> lkpUserPermissions { set; get; }
//        public string strUserRole { get; set; }
//        public string strUserGroup { get; set; }
//        public string strUserPermission { get; set; } 
//    }


//    public class ChurchBodyConfigCong_MDL
//    {
//        public AppGlobalOwner oCurrAppGlobalOwner { get; set; } // pcg
//        public ChurchMember oCurrLoggedMember { get; set; }
//        public int? oCurrLoggedMemberId { get; set; }
//        public ChurchBody oCurrChurchBody { get; set; }
//        public string strCurrChurchBody { get; set; }
       

//        public int setIndex { get; set; }   //from 20... leave first 20 for admin stuff
//        public bool isEditable { get; set; } 
//        public List<SelectListItem> lkpSharingStatus { set; get; }
//        public List<SelectListItem> lkpOwnerStatus { set; get; }
//        public List<SelectListItem> lkpGenStatuses { set; get; }   //Active, Deactive
//        public List<SelectListItem> lkpExtStatuses { set; get; }   //Active, Deactive, Pending, Expired
//        public List<SelectListItem> lkpGenderTypes { set; get; } // Male, Female, Other
//      //  public List<SelectListItem> lkpChuWorkStatuses { set; get; }   //Operationalized, Structure only


//        // !-- church year --!
//        public List<ActivityPeriod> lsActivityPeriods { get; set; }
//        public ActivityPeriod  oActivityPeriod { get; set; }
//        public List<SelectListItem> lkpPeriodTypes { set; get; }  //church year, account period
//        public List<SelectListItem> lkpPeriodStatus { set; get; }

//        // !-- church service category--!
//        public List<ChurchBodyService> lsChurchServiceCategories { get; set; }
//        public ChurchBodyService oChurchServiceCategory { get; set; }

//        // !-- church services --!
//        public List<ChurchBodyService> lsChurchBodyServices { get; set; }
//        public ChurchBodyService oChurchBodyService { get; set; }
//        public List<SelectListItem> lkpServiceCategories { set; get; }
//        public List<SelectListItem> lkpMeetingDays { set; get; }
//        public List<SelectListItem> lkpFrequency { set; get; }
//        public string strServiceCategory { get; set; }

//        // !-- church Positions (positions) .. member, elder, pastor, senior minister --!
//        public List<ChurchPosition> lsChurchPositions { get; set; }
//        public ChurchPosition oChurchPosition { get; set; }
//       // public List<SelectListItem> lkpPositionTypes { set; get; }

//        // !-- church Positions (positions) .. member, elder, pastor, senior minister --!
//        public List<ChurchRank> lsChurchRanks { get; set; }
//        public ChurchRank oChurchRank { get; set; }


//        // !-- church types ... regular, passed, invalid --!   
//        public List<ChurchMemType> lsChurchMemTypes { get; set; }
//        public ChurchMemType oChurchMemType { get; set; }

//        // !-- church status ... regular, passed, invalid --!
//        public List<ChurchMemStatus> lsChurchMemStatuses { get; set; }
//        public ChurchMemStatus  oChurchMemStatus { get; set; } 
        
//        // !-- church life activities .. baptism, confirmation, eucharist, naming... --!
//        public List<ChurchLifeActivity> lsChurchLifeActivities { get; set; }
//        public ChurchLifeActivity oChurchLifeActivity { get; set; }
//        public List<SelectListItem> lkpActivityTypes { set; get; }

//        // !-- church life activities prerequisites .. premarital counselling for weddings... --!
//        public List<ChurchLifeActivityReqDef> lsChurchLifeActivityReqDefs { get; set; }
//        public ChurchLifeActivityReqDef oChurchLifeActivityReqDef { get; set; }
//        public List<SelectListItem> lkpChurchLifeActivities { set; get; }
//       // public List<SelectListItem> lkpFrequencies { set; get; }

//        // !-- event category   .. almanac activities, revivals, sermons ... --!
//        public List<ChurchEventCategory> lsChurchEventCategories { get; set; }
//        public ChurchEventCategory oChurchEventCategory { get; set; } 
        
//        // !-- church department category ... --!
//        public List<ChurchSectorCategory>  lsChurchSectorCategories { get; set; }
//        public ChurchSectorCategory  oChurchSectorCategory { get; set; } 
        
//         // !-- church department  ... --!
//        public List<ChurchSector>  lsChurchSectors { get; set; }
//        public ChurchSector  oChurchSector  { get; set; } 
//        public List<SelectListItem> lkpSectorCategories { set; get; }
//        public List<SelectListItem> lkpSectors { set; get; }        

//        // !-- church role category ... --!
//        public List<LeaderRoleCategory>  lsLeaderRoleCategories { get; set; }
//        public LeaderRoleCategory oLeaderRoleCategory { get; set; }

//        // !-- church roles  ... --!
//        public List<LeaderRole>  lsLeaderRoles { get; set; }
//        public LeaderRole  oLeaderRole  { get; set; } 
//        public List<SelectListItem> lkpRoleCategories { set; get; } 
        
//        // !-- church dept roles  ... --!
//        public List<SectorLeaderRole> lsSectorLeaderRoles { get; set; }
//        public SectorLeaderRole  oSectorLeaderRole  { get; set; } 
//        public List<SelectListItem> lkpLeaderRoles { set; get; }
//        public List<SelectListItem> lkpChurchSectors { set; get; }
//        public List<SelectListItem> lkpOfficeTermTypes { set; get; }

//        // !-- Church division types // U: Council, Assembly, G: Church Groups, Cells, Clubs, Society  ..or NULL --!
//        public List<ChurchUnitType> lsChurchUnitTypes { get; set; }
//        public ChurchUnitType oChurchUnitType { get; set; }  
//        public List<SelectListItem> lkpOrganisationTypes { set; get; }  //U-Church Unit, D-Church Department, G-Church Grouping, O-Office, C-Standing Committee [permanent, others to go to the specific church body]
//    }
       
//    public class ChurchBodyConfig_CBStructure_MDL 
//    {
//        public AppGlobalOwner oCurrAppGlobalOwner { get; set; } // pcg
//        public ChurchMember oCurrLoggedMember { get; set; }
//        public int? oCurrLoggedMemberId { get; set; }
//        public ChurchBody oCurrChurchBody { get; set; }
//        public string strCurrChurchBody { get; set; }
         

//        public int setIndex { get; set; }   //from 20... leave first 20 for admin stuff
//        public int? currParId { get; set; }
//        public string strCurrPar { get; set; }
//        public bool isEditable { get; set; }
//        public List<ChurchBody> lsChurchUnits { get; set; }
//        public ChurchBody oChurchUnit { get; set; }

//        public List<SelectListItem> lkpSharingStatus { set; get; }
//        public List<SelectListItem> lkpOwnerStatus { set; get; }
//        public List<SelectListItem> lkpGenStatuses { set; get; }   //Active, Deactive
//        public List<SelectListItem> lkpOrganisationTypes { set; get; }  //U-Church Unit, D-Church Department, G-Church Grouping, O-Office, C-Standing Committee [permanent, others to go to the specific church body]
//        public List<SelectListItem> lkpChurchUnitTypes { set; get; }  // U: Council, Assembly, G: Church Groups, Cells, Clubs, Society  ..or NULL
//       // public List<SelectListItem> lkpChurchLevels { set; get; }
//        public List<SelectListItem> lkpParentUnits { set; get; }
//        public List<SelectListItem> lkpChuWorkStatuses { set; get; }   //Operationalized, Structure only

//        public List<SelectListItem> lkpAppGlobalOwn { set; get; }
//        public List<SelectListItem> lkpChurchBodies { set; get; }
//        public List<SelectListItem> lkpChurchLevels { set; get; }
//        public List<SelectListItem> lkpAssociationTypes { set; get; }
//        public List<SelectListItem> lkpChurchTypes { set; get; }
//        public List<SelectListItem> lkpChurchCategories { set; get; }
//        public List<SelectListItem> lkpCountries { set; get; }
//        public List<SelectListItem> lkpCountryRegions { set; get; }
//        public List<SelectListItem> lkpContactDetails { set; get; }


//        // !-- ChurchUnit --!
//        // public List<ChurchDivision> lsChurchUnits { get; set; }
//        //  public ChurchDivision oChurchUnit { get; set; }

//        // !-- ChurchDepartment --!
//        public List<ChurchBody> lsChurchDepartments { get; set; }
//       // public ChurchDivision oChurchDepartment { get; set; }

//        // !-- ChurchOffice --!
//        public List<ChurchBody> lsChurchOffices { get; set; }
//       // public ChurchDivision oChurchOffice { get; set; }

//        // !-- ChurchGrouping --!
//        public List<ChurchBody> lsChurchGroupings { get; set; }
//       // public ChurchDivision oChurchGrouping { get; set; }

//        // !-- ChurchStandingCommittee --!
//        public List<ChurchBody> lsChurchStandingCommittees { get; set; }
//        // public ChurchDivision oChurchStandingCommittee { get; set; }


//        //public object GetDivTypeDetail(string oCode, bool setIndex)
//        //{
//        //    switch (oCode)
//        //    {
//        //        case "CR": if (setIndex) return 0; else return "Church Root";
//        //        case "GB": if (setIndex) return 1; else return "Governing Body";
//        //        case "CO": if (setIndex) return 2; else return "Church Office";
//        //        case "DP": if (setIndex) return 3; else return "Church Department";
//        //        case "CG": if (setIndex) return 4; else return "Church Grouping";
//        //        case "SC": if (setIndex) return 5; else return "Standing Committee";
//        //        case "CE": if (setIndex) return 6; else return "Church Enterprise";
//        //        case "TM": if (setIndex) return 7; else return "Team";
//        //        //case "CP": if (setIndex) return 8; else return "Church Position";
//        //        case "IB": if (setIndex) return 9; else return "Independent Unit";
//        //        case "CN": if (setIndex) return 10; else return "Congregation";

//        //        default: return oCode;
//        //    }
//        //}
//    }


//}
