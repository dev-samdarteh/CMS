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
    public class ChurchBodyModel
    {
        public ChurchBodyModel() { }

        public string strAppName { get; set; }
        public string strAppNameMod { get; set; }
        public string strAppCurrUser { get; set; }
        public string strCurrTask { get; set; }
        // 

        public int? oAppGlolOwnId { get; set; }
        public int? oChurchBodyId { get; set; }
        public AppGlobalOwner oAppGlobalOwn { get; set; }
        public ChurchLevel oChurchLevel { get; set; }
       // public ChurchBody oChurchBody { get; set; }  // grace
        //
        public int? oAppGloOwnId_Logged { get; set; }
        public int? oChurchBodyId_Logged { get; set; }
        public int? oCurrMemberId_Logged { get; set; }
        public int? oCurrUserId_Logged { get; set; }
        public int? oMemberId_Logged { get; set; }
        public int? oUserId_Logged { get; set; }
        public string oUserRole_Logged { get; set; }

        public AppGlobalOwner oAppGlobalOwn_Logged { get; set; }
        public ChurchBody oChurchBody_Logged { get; set; }
        public ChurchMember oCurrLoggedMember { get; set; }
        public UserProfile oChurchAdminProfile { get; set; }
        public int setIndex { get; set; }
        public int subSetIndex { get; set; }

        //
        public List<ChurchBodyModel> lsChurchBodyModels { get; set; }
        public List<ChurchBody> lsChurchBodyies { get; set; }
        public ChurchBody oChurchBody { get; set; }
        //
        public List<ChurchBodyModel> lsSubChurchBodyModels { get; set; }
        public List<ChurchBody> lsSubChurchBodyies { get; set; }
        public ChurchBody oSubChurchBody { get; set; }
        //
        public string strChurchBody { get; set; }
        public string strAppGlobalOwn { get; set; }
        public string strChurchLevel { get; set; }
        public string strAssociationType { get; set; }
        public string strChurchType { get; set; }
        public string strParentChurchBody { get; set; }
        public string strCountry { get; set; }
        public string strCountryRegion { get; set; }
        public string strContactDetail { get; set; }
        public string strChurchLogo { get; set; }
        public string strStatus { get; set; }
         
        public int? oCurrAppGloId_Filter5 { get; set; }
        public int? oCurrChuCategId_Filter5 { get; set; }
        public bool oCurrShowAllCong_Filter5 { get; set; }   

        //         
        public List<ChurchBody> lsChurchBody { get; set; }
        public List<SelectListItem> lkpAppGlobalOwns { set; get; }
        public List<SelectListItem> lkpChurchBodies { set; get; }
        public List<SelectListItem> lkpChurchLevels { set; get; }
        public List<SelectListItem> lkpAssociationTypes { set; get; }
        public List<SelectListItem> lkpChurchTypes { set; get; }
        public List<SelectListItem> lkpChurchCategories { set; get; }
        public List<SelectListItem> lkpCountries { set; get; }
        public List<SelectListItem> lkpCountryRegions { set; get; }
        public List<SelectListItem> lkpContactDetails { set; get; }
                
        
    }
}
