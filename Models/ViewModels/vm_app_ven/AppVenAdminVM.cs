using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using RhemaCMS.Models.Adhoc;
//using RhemaCMS.Models.CLNTModels;
using RhemaCMS.Models.MSTRModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RhemaCMS.Models.ViewModels
{
    public class AppVenAdminVM
    {
        public string strAppName { get; set; }
        public string strAppNameMod { get; set; }
        public string strAppCurrUser { get; set; }
        //
        public MSTRChurchBody oChurchBody { get; set; }  // grace
        public int? oAppGloOwnId_Logged { get; set; }
        public MSTRChurchBody oChurchBody_Logged { get; set; }
        public int? oChurchBodyId_Logged { get; set; }
        //public ChurchMember oCurrLoggedMember { get; set; }
        public int? oAppGloOwnId { get; set; }
        public int? oChurchBodyId { get; set; }
        public int? oMemberId_Logged { get; set; }
        public int? oUserId_Logged { get; set; }
        public string oUserRole_Logged { get; set; }   //can be more than 1
        public List<UserRole> oUserRolesAssigned_Logged  { get; set; }   //can be more than 1
        public List<UserPermission> oUserPrivsAssigned_Logged { get; set; }   //use the code
        public DenominationVM oCurrDenomVM { get; set; }
        public MSTRChurchLevel oChurchLevel_Logged { get; set; } 
        public UserProfile oChurchAdminProfile_Logged { get; set; }

        


        public int pageIndex { get; set; }  // 1-Index, 2-User detail
        public int setIndex { get; set; }
        public int subSetIndex { get; set; }
        public string strCurrTask { get; set; }
        public string strSubCurrTask { get; set; }

        public UserProfileModel oUserProfileModel { get; set; }  // user profile... [ user roles, user permissions, user groups ]
        public List<UserProfileModel> lsUserProfileModel { get; set; }

        public List<UserProfileVM> lsUserProfiles{ get; set; }
        public List<UserPermission> lsPermissions{ get; set; }
        public List<UserRole> lsRoles { get; set; }

        public List<UserProfileRole> lsProfileRoles { get; set; }
        public List<UserProfileGroup> lsProfileGroups { get; set; }
        public List<UserPermission> lsProfilePermissions { get; set; }

        public List<FaithCategoryVM> lsFaithCategories { get; set; }
        public List<DenominationVM> lsDenominations { get; set; }
        public List<SubscriptionVM> lsSubscriptions { get; set; }
        public List<MSTRCountry> lsCountries { get; set; }
                                 
        public class FaithCategoryVM
        {
            public MSTRAppGlobalOwner oDenomination { get; set; }
            public MSTRChurchLevel oChurchLevel { get; set; }
            public MSTRChurchBody oChurchBody { get; set; }
            public UserProfile oChurchAdminProfile { get; set; }
            public ChurchFaithType oChurchFaithType { get; set; }            
            public string strFaithTypeClass { get; set; }
            public List<ChurchFaithType> ChurchFaithTypes { get; set; }
            public List<SelectListItem> lkpFaithTypeClasses { set; get; }
        }

        public class DenominationVM
        {
            public int oAppGloOwnId { get; set; }
            public MSTRAppGlobalOwner oDenomination { get; set; }
            public MSTRChurchLevel oChurchLevel { get; set; }
            public MSTRChurchBody oChurchBody { get; set; }
            public UserProfile oChurchAdminProfile { get; set; }
            public string strChurchLogo { get; set; }
            public string strStatus { get; set; }
            public string strCountry { get; set; }
            public string strFaithTypeCategory { get; set; }
            public string strAppGloOwn { get; set; }

          //  public string strDenomination { get; set; }
            [Display(Name = "Church logo")]
            public IFormFile ChurchLogoFile { get; set; }

            //public List<AppGlobalOwner> Denominations { get; set; }
            public List<MSTRChurchLevel> lsChurchLevels { get; set; }
            public List<ChurchBodyVM> lsChurchBodies { get; set; }
            public List<UserProfileVM> lsChurchAdminProfiles { get; set; }
            public List<SubscriptionVM> lsSubscriptions { get; set; }

            public List<SelectListItem> lkpDenominations { set; get; }  
            public List<SelectListItem> lkpCongregations { set; get; }
            public List<SelectListItem> lkpStatuses { set; get; }
            public List<SelectListItem> lkpCountries { set; get; }
            public List<SelectListItem> lkpFaithTypeCategories { set; get; }
        }

        public class SubscriptionVM
        {
            public MSTRAppGlobalOwner oDenomination { get; set; }

        }

        public class ChurchBodyVM
        {
            public MSTRAppGlobalOwner oDenomination { get; set; } // curr logged
            public int? oCurrAppGloId_Filter5 { get; set; }
            public int? oCurrChuCategId_Filter5 { get; set; }
            public bool oCurrShowAllCong_Filter5 { get; set; }
            public MSTRChurchBody oCurrChurchBody { get; set; }
            public CLNTModels.ChurchMember oCurrLoggedMember { get; set; }
            public int? oCurrLoggedMemberId { get; set; }
            public int setIndex { get; set; }
            //
            public List<ChurchBodyVM> lsChurchBody_MDL { get; set; } // public public List<ChurchLevel> lsChurchLevels { get; set; } 
            public ChurchBodyVM oChurchBodyVM { get; set; }
            public MSTRChurchBody oChurchBody { get; set; }

            //         
            public List<MSTRChurchBody> lsChurchBody { get; set; }
            public List<SelectListItem> lkpDenominations { set; get; }
            public List<SelectListItem> lkpChurchBodies { set; get; }
            public List<SelectListItem> lkpChurchLevels { set; get; }
            public List<SelectListItem> lkpAssociationTypes { set; get; }
            public List<SelectListItem> lkpChurchTypes { set; get; }
            public List<SelectListItem> lkpChurchCategories { set; get; }
            public List<SelectListItem> lkpCountries { set; get; }
            public List<SelectListItem> lkpCountryRegions { set; get; }
            public List<SelectListItem> lkpContactDetails { set; get; }

            public string strAppGloOwn { get; set; }
            public string strChurchLevel { get; set; }
            public string strAssociationType { get; set; }
            public string strChurchType { get; set; }
            public string strParentChurchBody { get; set; }
            public string strCountry { get; set; }
            public string strCountryRegion { get; set; }
            public string strContactDetail { get; set; }

            //
            public List<MSTRChurchBody> lsSubChurchBodies { get; set; }
            public MSTRChurchBody oSubChurchBody { get; set; }
            //  public List<SelectListItem> lkpCongregations { set; get; }
            public string strChurchBody { get; set; }
        }

        public class UserProfileVM
        {
            public int? oAppGlolOwnId { get; set; }
            public int? oChurchBodyId { get; set; }
            public int? oCurrLoggedMemberId { get; set; }
            public int? oCurrLoggedUserId { get; set; }
            public string strConfirmUserCode { get; set; }

           // public AppGlobalOwner oDenomination { get; set; }
            public MSTRAppGlobalOwner oAppGlobalOwn { get; set; }
            public MSTRChurchLevel oChurchLevel { get; set; }
            public MSTRChurchBody oChurchBody { get; set; }
            public UserProfile oChurchAdminProfile { get; set; }
             
            public CLNTModels.ChurchMember oCurrLoggedMember { get; set; } 
            public int setIndex { get; set; }
            public int subSetIndex { get; set; }
            [StringLength(1)]
            public string profileScope { get; set; }

            //   
            public List<UserProfile> lsUserProfiles { get; set; }
            public UserProfile oUserProfile { get; set; }
          //  public List<SelectListItem> lkpDenominations { set; get; }
            public List<SelectListItem> lkpAppGlobalOwns { set; get; }
            public List<SelectListItem> lkpChurchMembers { set; get; }
            public List<SelectListItem> lkpOwnerUsers { set; get; }
            public List<SelectListItem> lkpCongregations { set; get; }
            public List<SelectListItem> lkpTargetCongregations { set; get; }
            public List<SelectListItem> lkpCongNextCategory { set; get; }
            public List<SelectListItem> lkpStatuses { set; get; }
            public List<SelectListItem> lkpUserTypes { set; get; }
            public List<SelectListItem> lkpPwdSecQueList { set; get; }
            public List<SelectListItem> lkpPwdSecAnsList { set; get; }

            public string strToChurchLevel { get; set; }
            public string strToChurchLevel_1 { get; set; }
            public string strToChurchLevel_2 { get; set; }
            public string strToChurchLevel_3 { get; set; }
            public string strToChurchLevel_4 { get; set; }
            public string strToChurchLevel_5 { get; set; }
            public string strToChurchLevel_6 { get; set; }
            public string strToChurchLevel_7 { get; set; }
            public int? ToChurchBodyId_Categ1 { get; set; }
            public int? ToChurchBodyId_Categ2 { get; set; }
            public int? ToChurchBodyId_Categ3 { get; set; }
            public int? ToChurchBodyId_Categ4 { get; set; }
            public int? ToChurchBodyId_Categ5 { get; set; }
            public int? ToChurchBodyId_Categ6 { get; set; }
            public int? ToChurchBodyId_Categ7 { get; set; }

            public string strUserProfile { get; set; }
           // public string strDenomination { get; set; }
             
            public string strChurchMember { get; set; }
            public string strOwnerUser { get; set; }
            public string strChurchBody { get; set; }
            public string strAppGloOwn { get; set; }
            public string strUserStatus { get; set; }

            [Display(Name = "User Photo")]
            public IFormFile UserPhotoFile { get; set; }
            public string strUserPhoto { get; set; } 
            // 
            public List<UserGroup> lsUserGroups { get; set; }
            public List<UserRole> lsUserRoles { get; set; }
            public List<UserPermission> lsUserPermissions { get; set; }
            public List<UserSessionPrivilege> lsUserPrivileges { get; set; }
            public UserRole oUserRole { get; set; }
            public UserGroup oUserGroup { get; set; }
            public UserPermission oUserPermission { get; set; }

            public UserProfileRole oUserProfileRole { get; set; }
            public UserProfileGroup oUserProfileGroup { get; set; }
            public UserGroupPermission oUserGroupPermission { get; set; }
            public UserRolePermission oUserRolePermission { get; set; }

            public List<SelectListItem> lkpUserProfiles { get; set; }
            public List<SelectListItem> lkpUserRoles { set; get; }
            public List<SelectListItem> lkpUserGroups { set; get; }
            public List<SelectListItem> lkpUserPermissions { set; get; }
            public string strUserRole { get; set; }
            public string strUserGroup { get; set; }
            public string strUserPermission { get; set; }
             
        }

        public class ResetUserProfilePwdVM
        {
            [Required]
            [Display(Name = "User name")]
            public string Username { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Current Password")]
            public string CurrentPassword { get; set; }
            
            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "New Password")]
            public string NewPassword { get; set; }
            
            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Repeat Password")]
            public string RepeatPassword { get; set; }
             
            //  [Required]
            [Display(Name = "Church code")]
            public string ChurchCode { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }

            // public int BasicCredentialsVerified { get; set; }
            public string strLogUserDesc { get; set; }
            public string IsValidated { get; set; }


            public string currChurchCode { get; set; }

            
            [Display(Name = "Verification code")]
            public string VerificationCode { get; set; } 

            //allow security que option if set by user
            [Display(Name = "Authentication Type")]
            public int?  AuthTypeUsed { get; set; }
            
            [Display(Name = "Security Question")]
            public string  SecurityQue { get; set; }

            [Display(Name = "Security Answer")]
            public string SecurityAns { get; set; }  //encrypted  [que + ans]

            //  public string currUsername { get; set; }
            //  public string currPassword { get; set; }

            public int? oAppGlolOwnId { get; set; }
            public int? oChurchBodyId { get; set; }
            public MSTRAppGlobalOwner oDenomination { get; set; }
            public MSTRChurchLevel oChurchLevel { get; set; }
            public MSTRChurchBody oChurchBody { get; set; }
            public UserProfile oUserProfile { get; set; }
            public CLNTModels.ChurchMember oCurrLoggedMember { get; set; }
            public int setIndex { get; set; }


            public List<SelectListItem> lkpAuthTypes { set; get; }
            public List<SelectListItem> lkpSecurityQuestions { set; get; }
        }


        //public class ChurchFaithTypeVM
        //{
        //    public AppGlobalOwner oCurrAppGlobalOwner { get; set; } // pcg
        //    public ChurchMember oCurrLoggedMember { get; set; }
        //    public int? oCurrLoggedMemberId { get; set; }
        //    public MSTRChurchBody oChurchBody { get; set; }
        //    public int setIndex { get; set; }
        //    //
        //    public List<ChurchFaithType> lsChurchFaithTypes { get; set; }
        //    public ChurchFaithType oChurchFaithType { get; set; }
        //    public List<SelectListItem> lkpFaithTypeClasses { set; get; }
        //    public string strFaithTypeClass { get; set; }

        //}
    }
}
