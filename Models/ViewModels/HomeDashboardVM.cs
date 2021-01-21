
using RhemaCMS.Models.Adhoc;
using RhemaCMS.Models.MSTRModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RhemaCMS.Models.ViewModels
{
    public class HomeDashboardVM
    {
        public HomeDashboardVM() { }

       // public AppGlobalOwner oAppGlobalOwner { get; set; }
        public MSTRChurchBody oChurchBody { get; set; }  //RequestCongregation
        public int? oChurchBodyId { get; set; }
        public MSTRChurchBody oLoggedChurchBody { get; set; }
       // public ChurchMember oChurchMember { get; set; }
        public int? oLoggedUserId { get; set; }
        // public int currAttendTask { get; set; }
        //  public string strCurrTaskDesc { get; set; }

        public int? oAppGlolOwnId { get; set; }
       // public ChurchBody oChurchBody { get; set; }  // grace
        public MSTRAppGlobalOwner oAppGlolOwn { get; set; }
        public int? oAppGloOwnId_Logged { get; set; }
        public MSTRChurchBody oChurchBody_Logged { get; set; }
        //public ChurchMember oCurrLoggedMember { get; set; } 
        //   public int? oChurchBodyId { get; set; }

        public int? oMemberId_Logged { get; set; }
        public int? oUserId_Logged { get; set; }
        public string oUserRole_Logged { get; set; }

        // dashboard counts   
        public string strAppName { get; set; }
        public string strAppNameMod { get; set; }
        public string strChurchType { get; set; }
        public string strChuBodyDenomLogged { get; set; }
        public string strChuBodyLogged { get; set; }
        public string strCB_CurrUsed { get; set; }
        public string strAppCurrUser { get; set; }
        public string strAppCurrUser_ChRole { get; set; }
        public string strAppCurrUser_PhotoFilename { get; set; }
        public string strTodaysAuditCount { get; set; }


        // Client Dashboard
        public string strCL_SubCong { get; set; }
        public string strCB_SubCongCount { get; set; }   
        public string strCB_MemListCount { get; set; } 
        public string strCBWeek_NewMemListCount { get; set; } 
        public string strCBWeek_VisitorsCount { get; set; } 
        public string strCBWeek_ReceiptsAmt { get; set; }
        public string strCBWeek_PaymentsAmt { get; set; }
        public string strCBWeek_NewConvertsCount { get; set; } 


        // Admin Palette
        public string TotalSubsDenom { get; set; }
        public string TotalSubsCong { get; set; }
        public string TotalSysRoles { get; set; }
        public string TotalSysPriv { get; set; }
        public string TotSysProfiles { get; set; }
        public string TotClientProfiles { get; set; }
        public string TotClientProfiles_Admins { get; set; }
        public string TotSubscribers { get; set; }
        public string TotDbaseCount { get; set; }
        
         
    }
}
