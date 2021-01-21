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
    public class ChurchFaithTypeModel
    {
        public ChurchFaithTypeModel() { }


        public string strAppName { get; set; }
        public string strAppNameMod { get; set; }
        public string strAppCurrUser { get; set; }
        public string strCurrTask { get; set; }
        // 
       
        public int? oAppGloOwnId { get; set; }
        public int? oChurchBodyId { get; set; }
        public AppGlobalOwner oCurrAppGlobalOwner { get; set; }
        public MSTRChurchLevel oChurchLevel { get; set; }
        public MSTRChurchBody oChurchBody { get; set; }  // grace
        //
        public int? oAppGloOwnId_Logged { get; set; }        
        public int? oChurchBodyId_Logged { get; set; } 
        public int? oCurrMemberId_Logged { get; set; }
        public int? oCurrUserId_Logged { get; set; } 
        public int? oMemberId_Logged { get; set; }
        public int? oUserId_Logged { get; set; }
        public string oUserRole_Logged { get; set; }  

        public MSTRChurchBody oChurchBody_Logged { get; set; }      
        public ChurchMember oCurrLoggedMember { get; set; }

       

        
        public UserProfile oChurchAdminProfile { get; set; }
        
        public int setIndex { get; set; }
        public int subSetIndex { get; set; }

        //
        //public int? oAppGlolOwnId_Logged { get; set; }
        // public int? oChurchBodyId_Logged { get; set; }
        // public int? oCurrMemberId_Logged { get; set; }
        // public int? oCurrUserId_Logged { get; set; }
        //

        public List<ChurchFaithTypeModel> lsChurchFaithTypeModels { get; set; }
        public List<ChurchFaithType> lsChurchFaithTypes { get; set; }
        public ChurchFaithType oChurchFaithType { get; set; }
        public List<SelectListItem> lkpFaithTypeClasses { set; get; }
        public string strFaithTypeClass { get; set; }
    }
}
