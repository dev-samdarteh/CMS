using Microsoft.AspNetCore.Mvc.Rendering;
using RhemaCMS.Models.Adhoc;
using RhemaCMS.Models.CLNTModels;
using RhemaCMS.Models.MSTRModels;
using System.Collections.Generic;

namespace RhemaCMS.Models.ViewModels
{
    public class AccountCategoryModel
    {
        public AccountCategoryModel() { }


        public int? oAppGlolOwnId { get; set; }
        public ChurchBody oChurchBody { get; set; }  // grace
        public AppGlobalOwner oAppGlolOwn { get; set; }
        public int? oAppGloOwnId_Logged { get; set; }
        public ChurchBody oChurchBody_Logged { get; set; }
        //public ChurchMember oCurrLoggedMember { get; set; } 
        public int? oChurchBodyId { get; set; }

        public AccountCategory oAccountCategory { get; set; }  // grace
                                                           // public AccountTypeModel oAccountTypeModel { get; set; }  // grace
        public List<AccountCategoryModel> lsAccountCategoryModel { get; set; }  // grace

        public string strAppName { get; set; }
        public string strAppNameMod { get; set; }
        public string strAppCurrUser { get; set; }
        public string strCurrTask { get; set; }

        public int? oAppGlolOwnId_Logged { get; set; }
        public int? oChurchBodyId_Logged { get; set; }
        public int? oMemberId_Logged { get; set; }
        public int? oUserId_Logged { get; set; }


        public UserProfile oUserLogged { get; set; }

        public int setIndex { get; set; }
        //public int? oMemberId_Logged { get; set; }
        //public int? oUserId_Logged { get; set; }
        public string oUserRole_Logged { get; set; }
        //    
        public List<SelectListItem> lkpParentCategories { set; get; }
        public List<SelectListItem> lkpAccountTypes { set; get; }
        public List<SelectListItem> lkpReportAreas { set; get; }
        public List<SelectListItem> lkpBalanceTypes { set; get; }
        public List<SelectListItem> lkpStatuses { set; get; }


        public string strAccountCategory { get; set; }
        public string strParentCategory { get; set; }
        public string strAccountType { get; set; }
        public string strReportArea { get; set; }
        public string strBalanceType { get; set; }
        public string strStatus { get; set; }

    }
}



