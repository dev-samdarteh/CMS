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
    public class MSTRChurchBodyModel
    { 
        public MSTRChurchBodyModel() { }

        public string strAppName { get; set; }
        public string strAppNameMod { get; set; }
        public string strAppCurrUser { get; set; }
        public string strCurrTask { get; set; }
        // 

        public int? oAppGloOwnId { get; set; }
        public int? oChurchBodyId { get; set; }
        public MSTRAppGlobalOwner oAppGlobalOwn { get; set; }
        // public MSTRChurchLevel oChurchLevel { get; set; }
        // public MSTRChurchBody oChurchBody { get; set; }  // grace
        //
        public int? oAppGloOwnId_Logged { get; set; }
        public int? oChurchBodyId_Logged { get; set; }
        public int? oCurrMemberId_Logged { get; set; }
        public int? oCurrUserId_Logged { get; set; }
        public int? oMemberId_Logged { get; set; }
        public int? oUserId_Logged { get; set; }
        public string oUserRole_Logged { get; set; }

        [Display(Name = "Church unit logo")]
        public IFormFile ChurchLogoFile { get; set; }

        public MSTRAppGlobalOwner oAppGlobalOwn_Logged { get; set; }
        public MSTRChurchBody oChurchBody_Logged { get; set; }
       // public ChurchMember oCurrLoggedMember { get; set; }
        public UserProfile oChurchAdminProfile { get; set; }
        public int setIndex { get; set; }
        public int subSetIndex { get; set; }
        public int PageIndex { get; set; }

        //
        public List<MSTRChurchBodyModel> lsChurchBodyModels { get; set; }
        public List<MSTRChurchBody> lsChurchBodies { get; set; }
        public MSTRChurchBody oChurchBody { get; set; }
        //
        public List<MSTRChurchBodyModel> lsSubChurchBodyModels { get; set; }
        public List<MSTRChurchBody> lsSubChurchBodies { get; set; }
        public MSTRChurchBody oSubChurchBody { get; set; }
        //
        public string strChurchBody { get; set; }
        public string strAppGloOwn { get; set; }
        public string strChurchLevel { get; set; }
        public int numCLIndex { get; set; }
      //  public string strAssociationType { get; set; }
        public string strOrgType { get; set; }
        public string strParentChurchBody { get; set; }
        public string strCountry { get; set; }
        public string strCountryRegion { get; set; }
        public string strContactDetail { get; set; }
        public string strCongLoc{ get; set; }
        public string strCongLoc2{ get; set; }
        public string strChurchLogo { get; set; }
        public string strStatus { get; set; }
        public string strParentCB_HeaderDesc { get; set; }
        public bool blStatusActivated { get; set; }
        public DateTime? dtCreated { get; set; }

        public int? oCurrAppGloId_Filter5 { get; set; }
        public int? oCurrChuCategId_Filter5 { get; set; }
        public bool oCurrShowAllCong_Filter5 { get; set; }
         

        //         

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
        //
        public List<MSTRChurchBody> lsChurchBody { get; set; }

        public List<SelectListItem> lkpAppGlobalOwns { set; get; }
        public List<SelectListItem> lkpChurchBodies { set; get; }
        public List<SelectListItem> lkpChurchLevels { set; get; }
         public List<SelectListItem> lkpStatuses { set; get; }
        public List<SelectListItem> lkpOrgTypes { set; get; }
        public List<SelectListItem> lkpChurchCategories { set; get; }
        public List<SelectListItem> lkpCountries { set; get; }
        public List<SelectListItem> lkpCountryRegions { set; get; }
        public List<SelectListItem> lkpContactDetails { set; get; }

    }
}
