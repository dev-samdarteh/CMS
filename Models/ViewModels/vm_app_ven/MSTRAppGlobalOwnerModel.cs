using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using RhemaCMS.Models.Adhoc;
//using RhemaCMS.Models.CLNTModels;
using RhemaCMS.Models.MSTRModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RhemaCMS.Models.ViewModels
{
    public class MSTRAppGlobalOwnerModel
    {
        public MSTRAppGlobalOwnerModel() { }
         
        public string strAppName { get; set; }
        public string strAppNameMod { get; set; }
        public string strAppCurrUser { get; set; }
        public string strCurrTask { get; set; }
        // 

      //  public int? oAppGlolOwnId { get; set; }
      //  public int? oChurchBodyId { get; set; }
       // public MSTRAppGlobalOwner oAppGlobalOwn { get; set; }
       // public ChurchLevel oChurchLevel { get; set; }
       // public ChurchBody oChurchBody { get; set; }  // grace
        //
        public int? oAppGloOwnId_Logged { get; set; }
        public int? oChurchBodyId_Logged { get; set; }
        public int? oCurrMemberId_Logged { get; set; }
       // public int? oCurrUserId_Logged { get; set; }
        public int? oMemberId_Logged { get; set; }
        public int? oUserId_Logged { get; set; }
        public string oUserRole_Logged { get; set; }

        public MSTRAppGlobalOwner oAppGlobalOwn_Logged { get; set; }
        public MSTRChurchBody oChurchBody_Logged { get; set; }
        public CLNTModels.ChurchMember oCurrLoggedMember { get; set; }         
        public UserProfile oChurchAdminProfile { get; set; }
        public int setIndex { get; set; }
        public int subSetIndex { get; set; }

        //
        public List<MSTRAppGlobalOwnerModel> lsAppGlobalOwnModels { get; set; }
        public List<MSTRAppGlobalOwner> lsAppGlobalOwns { get; set; }
        public MSTRAppGlobalOwner oAppGlobalOwn { get; set; }

        public string strChurchLogo { get; set; }

        public string strSlogan { get; set; }
        public string strSloganResponse { get; set; }
        public string strChurchStream { get; set; }
        public bool blStatusActivated { get; set; }

        public string strStatus { get; set; }
        public string strCountry { get; set; }
        public string strFaithCategory { get; set; }
        public string strAppGloOwn { get; set; }

        [Display(Name = "Church logo")]
        public IFormFile ChurchLogoFile { get; set; }
         
        public List<MSTRChurchLevel> lsChurchLevels { get; set; }
        public List<MSTRChurchBody> lsChurchBodies { get; set; }
        public int TotalChurchLevels { get; set; }
        public int TotalCongregations { get; set; }

        public List<ChurchLevelModel> lsChurchLevelModels { get; set; }
        public List<ChurchBodyModel> lsChurchBodyModels { get; set; }
        
        public List<UserProfileModel> lsUserProfileModels { get; set; }
        public List<AppSubscriptionModel> lsAppSubscriptionModels { get; set; }

        public List<SelectListItem> lkpAppGlobalOwns { set; get; }
        public List<SelectListItem> lkpChurchBodies { set; get; }
        public List<SelectListItem> lkpStatuses { set; get; }
        public List<SelectListItem> lkpCountries { set; get; }
        public List<SelectListItem> lkpFaithCategories { set; get; }
    }
}
