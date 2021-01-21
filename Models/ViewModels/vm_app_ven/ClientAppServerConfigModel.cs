using Microsoft.AspNetCore.Mvc.Rendering;
using RhemaCMS.Models.MSTRModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RhemaCMS.Models.ViewModels
{
    public class ClientAppServerConfigModel
    {
        public ClientAppServerConfigModel() { }

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
        public int? oCurrUserId_Logged { get; set; }
        public int? oMemberId_Logged { get; set; }
        public int? oUserId_Logged { get; set; }
        public string oUserRole_Logged { get; set; }

        public MSTRAppGlobalOwner oAppGlobalOwn_Logged { get; set; }
        public MSTRChurchBody oChurchBody_Logged { get; set; }
        public RhemaCMS.Models.CLNTModels.ChurchMember oCurrLoggedMember { get; set; }
        public UserProfile oChurchAdminProfile { get; set; }
        public int setIndex { get; set; }
        public int subSetIndex { get; set; }
        public int PageIndex { get; set; }
        //
        public List<ClientAppServerConfigModel> lsCASConfigModels { get; set; }
        public List<ClientAppServerConfig> lsCASConfigs { get; set; }
        public ClientAppServerConfig oCASConfig { get; set; }
         
        public string strAppGloOwn { get; set; }
       // public string strChurchBody { get; set; }
        public string strConfigDate { get; set; }
        public string strStatus { get; set; }

        public List<SelectListItem> lkpAppGlobalOwns { set; get; } 
        public List<SelectListItem> lkpAppServers { set; get; } 
        public List<SelectListItem> lkpClientDatabases { set; get; } 
         
        public List<SelectListItem> lkpStatuses { set; get; }
    }
}
