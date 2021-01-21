using Microsoft.AspNetCore.Mvc.Rendering;
using RhemaCMS.Models.CLNTModels;
using System;
using System.Collections.Generic; 

namespace RhemaCMS.Models.ViewModels.vm_cl
{
    public class ChurchLevelModel
    {
        public ChurchLevelModel() { }

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
        public List<ChurchLevelModel> lsChurchLevelModels { get; set; }
        public List<ChurchLevel> lsChurchLevels { get; set; }
        public ChurchLevel oChurchLevel { get; set; }

        public int numChurchLevel { get; set; }
        public string strChurchLevelName { get; set; }
        public string strAppGloOwn { get; set; }
       // public string strStatus { get; set; }

        public List<SelectListItem> lkpAppGlobalOwns { set; get; }
        public List<SelectListItem> lkpStatuses { set; get; }
    }
}
