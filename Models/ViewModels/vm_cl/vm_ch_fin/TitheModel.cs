using Microsoft.AspNetCore.Mvc.Rendering;
using RhemaCMS.Models.Adhoc;
using RhemaCMS.Models.CLNTModels;
using RhemaCMS.Models.MSTRModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RhemaCMS.Models.ViewModels
{
    public class TitheModel
    {
        public TitheModel() { }


        public int? oAppGlolOwnId { get; set; }
        public ChurchBody oChurchBody { get; set; }  // grace
        public AppGlobalOwner oAppGlolOwn { get; set; }
        public int? oAppGloOwnId_Logged { get; set; }
        public ChurchBody oChurchBody_Logged { get; set; }
        //public ChurchMember oCurrLoggedMember { get; set; } 
        public int? oChurchBodyId { get; set; }

        public TitheTrans oTitheTrans { get; set; }  // grace
                                                             // public TitheModel oTitheModel { get; set; }  // grace
        public List<TitheModel> lsTitheModel { get; set; }  // grace

        public string strAppName { get; set; }
        public string strAppNameMod { get; set; }
        public string strAppCurrUser { get; set; }
        public string strCurrTask { get; set; }

        public int? oAppGlolOwnId_Logged { get; set; }
        public int? oChurchBodyId_Logged { get; set; }
        public int? oMemberId_Logged { get; set; }
        public int? oUserId_Logged { get; set; }
        public string strConfirmUserCode { get; set; }


        public UserProfile oUserLogged { get; set; }

        public int setIndex { get; set; }
        //public int? oMemberId_Logged { get; set; }
        //public int? oUserId_Logged { get; set; }
        public string oUserRole_Logged { get; set; }
        //    

        public List<SelectListItem> lkpChurchMembers_Local { set; get; }
        public List<SelectListItem> lkpTitheModes { set; get; }
        public List<SelectListItem> lkpTitherScopes { set; get; }
        public List<SelectListItem> lkpCurrencies { set; get; }
        public List<SelectListItem> lkpAccountPeriods { set; get; }
        public List<SelectListItem> lkpChurchEvents { set; get; }
        public List<SelectListItem> lkpPaymentModes { set; get; }
        public List<SelectListItem> lkpStatuses { set; get; }


        public string strReceivedBy { get; set; }
        public string strTitheMode { get; set; }
        public string strRelatedEvent { get; set; }
        public string strTitheDate { get; set; }
        public string strPostDate { get; set; }
        public string strPostStatus { get; set; }
        public string strAccountPeriod { get; set; }
        public string strCurrency { get; set; }
        public string strAmount { get; set; }
        public string strTithedBy { get; set; } 
        public string strTitherScope { get; set; }

        public DateTime? dt_TitheDate { get; set; }
    }
}
