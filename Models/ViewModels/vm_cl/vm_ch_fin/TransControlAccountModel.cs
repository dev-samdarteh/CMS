using Microsoft.AspNetCore.Mvc.Rendering;
using RhemaCMS.Models.Adhoc;
using RhemaCMS.Models.CLNTModels;
using RhemaCMS.Models.MSTRModels;
using System.Collections.Generic;

namespace RhemaCMS.Models.ViewModels
{
    public class TransControlAccountModel 
    {
        public TransControlAccountModel() { }
        public int? oAppGlolOwnId { get; set; }
        public ChurchBody oChurchBody { get; set; }  // grace
        public AppGlobalOwner oAppGlolOwn { get; set; }
        public int? oAppGloOwnId_Logged { get; set; }
        public ChurchBody oChurchBody_Logged { get; set; }
        //public ChurchMember oCurrLoggedMember { get; set; } 
        public int? oChurchBodyId { get; set; }

        public TransControlAccount oTransControlAccount { get; set; }  // grace
                                                               // public AccountTypeModel oAccountTypeModel { get; set; }  // grace
        public List<TransControlAccountModel> lsTransControlAccountModel { get; set; }  // grace

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

        public List<SelectListItem> lkpGLAccounts { set; get; }
        public List<SelectListItem> lkpContraGLAccounts { set; get; }

        public List<SelectListItem> lkpTransModules { set; get; }
        public List<SelectListItem> lkpReportAreas { set; get; }
        public List<SelectListItem> lkpStatuses { set; get; }
        public List<SelectListItem> lkpAccountTypes { set; get; }
        public List<SelectListItem> lkpAccountCategories { set; get; } 
        public List<SelectListItem> lkpContraAccountTypes { set; get; }
        public List<SelectListItem> lkpContraAccountCategories { set; get; }


        public string strAccountTypeCode { get; set; }
        public int? AccountCategoryId { get; set; }
        public string strContraAccountTypeCode { get; set; }
        public int? ContraAccountCategoryId { get; set; }


        // public string strTransControlAccount { get; set; }
        public string strControlAccount { get; set; }
        public string strContraAccount { get; set; }
        public string strTransModule { get; set; }
        public string strStatus { get; set; }
    }
    
}
