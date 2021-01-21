using Microsoft.AspNetCore.Mvc.Rendering;
using RhemaCMS.Models.Adhoc;
using RhemaCMS.Models.CLNTModels;
using RhemaCMS.Models.MSTRModels; 
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RhemaCMS.Models.ViewModels 
{
    public class AccountMasterModel
    {
        public AccountMasterModel() { }


        public int? oAppGlolOwnId { get; set; }
        public ChurchBody oChurchBody { get; set; }  // grace
        public AppGlobalOwner oAppGlolOwn { get; set; }
        public int? oAppGloOwnId_Logged { get; set; }
        public ChurchBody oChurchBody_Logged { get; set; }
        //public ChurchMember oCurrLoggedMember { get; set; } 
        public int? oChurchBodyId { get; set; }

        public AccountMaster oAccountMaster { get; set; }  // grace
                                                             // public AccountMasterModel oAccountMasterModel { get; set; }  // grace
        public List<AccountMasterModel> lsAccountMasterModel { get; set; }  // grace

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
        public List<SelectListItem> lkpAccountTypes { set; get; } //INC, EXP, CAS, CLB, FAS, ELB 
        public List<SelectListItem> lkpReportAreas { set; get; }
        public List<SelectListItem> lkpAccountTypeCategories { set; get; } //
        public List<SelectListItem> lkpGLAccounts { set; get; } //
        public List<SelectListItem> lkpCurrencies { set; get; } //
        public List<SelectListItem> lkpBalanceTypes { set; get; } //
        public List<SelectListItem> lkpStatuses { set; get; }

        [StringLength(3)]   //100000-000  ... post to only leaf/childless accounts
        public string SubAccountNo { get; set; }
        public string ReportAreaCode { get; set; }
        public string AccountTypeCode { get; set; }
       // public string AccountTypeCategoryCode { get; set; }


        public string strAccountName { get; set; }
        public string strAccountType { get; set; }
        public string strAccountTypeCategory { get; set; }
        public string strAccountCategory { get; set; }       
        public string strStatus { get; set; }

      //  public string strAccountType { get; set; }
        public string strReportArea { get; set; }
        public string strBalanceCode { get; set; }
        public string strBalanceType { get; set; }
        public string strCurrency { get; set; }
        //
        public string strAccBalance { get; set; }
        public string strAccBudget { get; set; }
        public string strAccVariance { get; set; }

    }
}
