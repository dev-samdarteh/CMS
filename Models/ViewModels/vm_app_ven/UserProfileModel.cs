using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using RhemaCMS.Models.Adhoc;
using RhemaCMS.Models.CLNTModels;
using RhemaCMS.Models.MSTRModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations; 



namespace RhemaCMS.Models.ViewModels
{
    public class UserProfileModel
    {
        public UserProfileModel() { }

        public string strAppName { get; set; }
        public string strAppNameMod { get; set; }
        public string strAppCurrUser { get; set; }
        public string strCurrTask { get; set; }
        public string strAppCurrUser_RoleCateg { get; set; }
        // 

          public int? oAppGloOwnId { get; set; }
          public int? oChurchBodyId { get; set; }
         public AppGlobalOwner oAppGlobalOwn { get; set; }
        // public ChurchLevel oChurchLevel { get; set; }
         public MSTRChurchBody oChurchBody { get; set; }  // grace
        //
        public int? oAppGloOwnId_Logged { get; set; }
        public int? oChurchBodyId_Logged { get; set; }
        public int? oCurrMemberId_Logged { get; set; }
        public int? oCurrUserId_Logged { get; set; }
        public int? oMemberId_Logged { get; set; }
        public int? oUserId_Logged { get; set; }
        public string oUserRole_Logged { get; set; }
        public int? oUserProfileLevel_Logged { get; set; }

        public AppGlobalOwner oAppGlobalOwn_Logged { get; set; }
        public MSTRChurchBody oChurchBody_Logged { get; set; }
        public ChurchMember oCurrLoggedMember { get; set; }
        public UserProfile oChurchAdminProfile { get; set; }
        public int setIndex { get; set; }
        public int subSetIndex { get; set; }
        public int pageIndex { get; set; }

      //  public int numCLIndex { get; set; }


        //
        public List<UserProfileModel> lsUserProfileModels { get; set; }
        public List<UserProfile> lsUserProfiles { get; set; }
        public UserProfile oUserProfile { get; set; }
        //
       // public List<UserRoleModel> lsUserRoleModels { get; set; }
        public List<UserRole> lsUserRoles { get; set; }
        public UserRole oUserRole { get; set; }
        //
       // public List<UserPermissionModel> lsUserPermissionModels { get; set; }
        public List<UserPermission> lsUserPermissions { get; set; }
        public UserPermission oUserPermission { get; set; }

        public List<UserAuditTrail> lsUserAuditTrails { get; set; }
        public UserAuditTrail oUserAuditTrail { get; set; }
        //

        [StringLength(1)]
        public string profileScope { get; set; }
        [StringLength(1)]
        public string subScope { get; set; }

        //    
        public List<SelectListItem> lkpDenominations { set; get; }
        public List<SelectListItem> lkpChurchMembers { set; get; }
        public List<SelectListItem> lkpOwnerUsers { set; get; }
        public List<SelectListItem> lkpCongregations { set; get; }
        public List<SelectListItem> lkpTargetCongregations { set; get; }
        public List<SelectListItem> lkpCongNextCategory { set; get; }
        public List<SelectListItem> lkpStatuses { set; get; }
        public List<SelectListItem> lkpUserTypes { set; get; }
        public List<SelectListItem> lkpPwdSecQueList { set; get; }
        public List<SelectListItem> lkpPwdSecAnsList { set; get; }

        public List<SelectListItem> lkp_CongNextCategory { set; get; }
        public List<SelectListItem> lkp_ToCongregations { set; get; }  // of same denomination except curr cong

         
        public int oCBLevelCount { get; set; }

        public string strChurchLevel_1 { get; set; }
        public string strChurchLevel_2 { get; set; }
        public string strChurchLevel_3 { get; set; }
        public string strChurchLevel_4 { get; set; }
        public string strChurchLevel_5 { get; set; }
        public int? ChurchBodyId_1 { get; set; }
        public int? ChurchBodyId_2 { get; set; }
        public int? ChurchBodyId_3 { get; set; }
        public int? ChurchBodyId_4 { get; set; }
        public int? ChurchBodyId_5 { get; set; }
        
        //
        //  public int? oChurchBodyId_1 { set; get; }
        public string strChurchBody_1 { set; get; }
        //
        public List<SelectListItem> lkp_ChurchBodies_1 { set; get; }
        public List<SelectListItem> lkp_ChurchBodies_2 { set; get; }
        public List<SelectListItem> lkp_ChurchBodies_3 { set; get; }
        public List<SelectListItem> lkp_ChurchBodies_4 { set; get; }
        public List<SelectListItem> lkp_ChurchBodies_5 { set; get; }



        public string strChurchBody { get; set; }
        public string strAppGloOwn { get; set; }
        public string strChurchLevel { get; set; } 
        public string strUserProfile { get; set; }
       // public string strDenomination { get; set; }
        public string strChurchMember { get; set; }
        public string strOwnerUser { get; set; } 
        public string strUserStatus { get; set; }

        [Display(Name = "User Photo")]
        public IFormFile UserPhotoFile { get; set; }
        public string strUserPhoto { get; set; }
        // 
        public List<UserProfileGroup> lsUserProfileGroups { get; set; }
        public List<UserProfileRole> lsUserProfileRoles { get; set; }
        public List<UserRolePermission> lsUserRolePermissions { get; set; }
        public List<UserGroupPermission> lsUserGroupPermissions { get; set; } 
        //
        public List<UserGroup> lsUserGroups { get; set; }
        //public List<UserRole> lsUserRoles { get; set; }
        //public List<UserPermission> lsUserPermissions { get; set; }
        public List<UserSessionPrivilege> lsUserPrivileges { get; set; }
       // public UserRole oUserRole { get; set; }
        public UserGroup oUserGroup { get; set; }
      //  public UserPermission oUserPermission { get; set; }

        public UserProfileRole oUserProfileRole { get; set; }
        public UserProfileGroup oUserProfileGroup { get; set; }
        public UserGroupPermission oUserGroupPermission { get; set; }
        public UserRolePermission oUserRolePermission { get; set; }

        public List<SelectListItem> lkpAppGlobalOwns { get; set; }
        public List<SelectListItem> lkpChurchLevels { get; set; }
        public List<SelectListItem> lkpUserProfiles { get; set; }
        public List<SelectListItem> lkpUserRoles { set; get; }
        public List<SelectListItem> lkpUserGroups { set; get; }
        public List<SelectListItem> lkpUserPermissions { set; get; }
        public string strUserRole { get; set; }
        public string strUserGroup { get; set; }
        public string strUserPermission { get; set; }
    }
}
