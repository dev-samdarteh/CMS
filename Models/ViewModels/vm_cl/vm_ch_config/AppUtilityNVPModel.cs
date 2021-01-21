using Microsoft.AspNetCore.Mvc.Rendering;
using RhemaCMS.Models.CLNTModels;
using System;
using System.Collections.Generic;

namespace RhemaCMS.Models.ViewModels.vm_cl
{
    public class AppUtilityNVPModel
    {
        public AppUtilityNVPModel() { }

        public string strAppName { get; set; }
        public string strAppNameMod { get; set; }
        public string strAppCurrUser { get; set; }
        public string strCurrTask { get; set; }
        // 

        public int? oAppGloOwnId { get; set; }
        //  public int? oChurchBodyId { get; set; }
        //  public AppGlobalOwner oAppGlobalOwn { get; set; }
        //  public ChurchLevel oChurchLevel { get; set; }
        //  public ChurchBody oChurchBody { get; set; }  // grace

        //
        public int? oAppGloOwnId_Logged { get; set; }
        public int? oChurchBodyId_Logged { get; set; }
        public int? oCurrMemberId_Logged { get; set; }
        // public int? oCurrUserId_Logged { get; set; }
        public int? oMemberId_Logged { get; set; }
        public int? oUserId_Logged { get; set; }
        public string oUserRole_Logged { get; set; }

        public AppGlobalOwner oAppGlobalOwn_Logged { get; set; }
        public ChurchBody oChurchBody_Logged { get; set; }
        public ChurchMember oCurrLoggedMember { get; set; }
        public MSTRModels.UserProfile oChurchAdminProfile { get; set; }
        public int setIndex { get; set; }
        public int subSetIndex { get; set; }
        public int PageIndex { get; set; }

        //
        public List<AppUtilityNVPModel> lsAppUtilityNVPModels { get; set; }
        public List<AppUtilityNVP> lsAppUtilityNVPs { get; set; }
        public AppUtilityNVP oAppUtilityNVP { get; set; }

        public string strAppGloOwn { get; set; }
        public string strNVPTag { get; set; }
       // public string strNVPCode { get; set; }
       // public int numOrderIndex { get; set; }
       // public int numAppUtilityNVP { get; set; }
       // public string strAppUtilityNVPName { get; set; }    
        public string strNVPCategory { get; set; } 
        public string strNVPStatus { get; set; }

        public List<SelectListItem> lkpAppGlobalOwns { set; get; }
        public List<SelectListItem> lkpStatuses { set; get; }
        public List<SelectListItem> lkpAppParameterTags { set; get; }
        public List<SelectListItem> lkpNVPCategories { set; get; }
    }
}

